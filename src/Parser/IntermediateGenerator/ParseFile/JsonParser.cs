﻿using Newtonsoft.Json.Linq;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ComponentInterfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models.ObjectAttributes;

namespace DatasetGenerator.ParseFile
{
    public interface IJsonParser : IFileParser
    {
    }

    public class JsonParser : IJsonParser
    {
        private readonly ILogger<JsonParser> _logger;
        public JsonParser(ILogger<JsonParser> logger)
        {
            _logger = logger;
        }

        public Task<DatasetObject> Parse(string stringFile, string extensionName, string fileName)
        {
            DatasetObject datasetObj = new DatasetObject(extensionName.ToLower(), fileName.ToLower());
            JsonTextReader reader = new JsonTextReader(new StringReader(stringFile));

            IntermediateObject intermediate = null;
            Stack<ListAttribute> currentListAttr = new Stack<ListAttribute>();
            string? propName = null;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    propName = HandleValueToken(reader, intermediate, currentListAttr, propName);
                    //_logger.LogInformation("Token: " + reader.TokenType);
                }
                else if (reader.TokenType.Equals(JsonToken.StartArray) || reader.TokenType.Equals(JsonToken.StartObject))
                {

                    if (propName == "crs")
                    {
                        GetCrs(reader, datasetObj);             
                    }

                    else if (intermediate == null)
                    {
                        intermediate = new IntermediateObject();
                        datasetObj.Objects.Add(intermediate);
                    }
                    else
                    {
                        ListAttribute newListAttr = (ListAttribute)GenerateAttribute(reader, intermediate, currentListAttr, propName);
                        currentListAttr.Push(newListAttr);
                        propName = null;
                    }
                }
                else if (reader.TokenType.Equals(JsonToken.EndArray) || reader.TokenType.Equals(JsonToken.EndObject))
                {
                    if (currentListAttr.Count > 0)
                    {
                        currentListAttr.Pop();
                    }
                }
                else if (reader.TokenType.Equals(JsonToken.Null))
                {
                    GenerateAttribute(reader, intermediate, currentListAttr, propName);
                }
            }
            return Task.FromResult(datasetObj);
        }

        private void GetCrs(JsonTextReader reader, DatasetObject datasetObj)
        {
            int depth = 0;
            bool propertiesFound = false;
            string? propName = null;

            do
            {
                if (reader.TokenType.Equals(JsonToken.PropertyName))
                {
                    propName = reader.Value.ToString();
                }

                if (reader.TokenType.Equals(JsonToken.StartObject))
                {
                    depth++;
                }

                else if (reader.TokenType.Equals(JsonToken.EndObject))
                {
                    depth--;
                }

                if (reader.Value != null && reader.Value.ToString() == "properties")
                {
                    propertiesFound = true;
                }
                if (propertiesFound && propName == "name" && reader.TokenType.Equals(JsonToken.String) && reader.Value != null)
                {
                    var crs = GetCoordinateReferenceSystem(reader.Value.ToString());
                    if(crs != null)
                        datasetObj.Properties.Add("CoordinateReferenceSystem", System.Text.Json.JsonSerializer.Serialize(crs));
                }
            }
            while (reader.Read() && depth != 0);
        }

        private CoordinateReferenceSystem GetCoordinateReferenceSystem(string? data)
        {
            if (data is null)
                return new CoordinateReferenceSystem(true);
            return new CoordinateReferenceSystem(data);
        }

        private string HandleValueToken(JsonTextReader reader, IntermediateObject? intermediate, Stack<ListAttribute> currentListAttr, string? propName)
        {
            if (reader.TokenType.Equals(JsonToken.PropertyName))
            {
                propName = reader.Value.ToString();
            }
            else
            {
                GenerateAttribute(reader, intermediate, currentListAttr, propName);
                propName = null;
            }
            //_logger.LogInformation("Token: " + reader.TokenType + " Value: " + reader.Value);
            return propName;
        }

        private ObjectAttribute GenerateAttribute(JsonTextReader reader, IntermediateObject? intermediate, Stack<ListAttribute> currentListAttr, string? propName)
        {
            ObjectAttribute attr = FindAndCreateType(propName, reader);

            if (currentListAttr.Count == 0)
            {
                intermediate.Attributes.Add(attr);
            }
            else
            {
                ((List<ObjectAttribute>)currentListAttr.Peek().Value).Add(attr);
            }

            return attr;
        }

        private ObjectAttribute FindAndCreateType(string propName, JsonTextReader reader)
        {
            if (propName == null)
            {
                if (reader.TokenType.Equals(JsonToken.StartArray) || reader.TokenType.Equals(JsonToken.StartObject))
                {
                    propName = reader.TokenType.ToString();
                }
                else if (reader.TokenType.Equals(JsonToken.Float))
                {
                    propName =  "DoubleValue";
                }
                else
                {
                    propName = reader.TokenType.ToString() + "Value";
                }
            }
            switch (reader.TokenType)
            {             
                case JsonToken.Integer:
                    return new LongAttribute(propName, (long)reader.Value);
                case JsonToken.Float:
                    return new DoubleAttribute(propName, (double)reader.Value);
                case JsonToken.String:
                    return new TextAttribute(propName, (string)reader.Value);
                case JsonToken.Null:
                    return new NullAttribute(propName);
                case JsonToken.Date:
                    return new DateAttribute(propName, (DateTime)reader.Value);
                case JsonToken.StartArray:
                case JsonToken.StartObject:
                    return CreateListType(propName, reader);
                case JsonToken.Boolean:
                    return new BoolAttribute(propName, (bool)reader.Value);

                default:
                    throw new Exception("Json token did not match any supported type: the type was " + reader.TokenType);
            }
        }


        private ListAttribute CreateListType(string propName, JsonTextReader reader)
        {
            if (propName != null)
            {
                return new ListAttribute(propName);
            }
            else
            {
                return new ListAttribute(reader.TokenType.ToString());
            }
        }
    }
}
