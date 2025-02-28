﻿using DatasetParser.Factories;
using Newtonsoft.Json.Linq;
using Shared.ComponentInterfaces;
using Shared.Models;
using Shared.Models.Output;
using Shared.Models.Output.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetParser
{
    public class DatasetParser : IDatasetParser
    {

        public DatasetParser()
        {
        }

        public async Task<OutputDataset> Parse(DatasetObject dataset, int iteration)
        {
            OutputDataset res = new OutputDataset(dataset.originalName, dataset.originalExtensionName);
            SpecializationDescription? description = null;
            switch (dataset.DatasetType)
            {
                case DatasetType.Parking:
                    description = new SpecializationDescription()
                    {
                        GeoFeatureType = GeoFeatureType.MultiPolygon,
                        Properties = new List<SpecializationPropertyDescription>()
                        {
                            new SpecializationPropertyDescription("Spots", new List<string>(){ "Amount" })
                        }
                    };
                    break;
                case DatasetType.Route:
                    description = new SpecializationDescription()
                    {
                        GeoFeatureType = GeoFeatureType.LineString,
                        Properties = new List<SpecializationPropertyDescription>()
                        {
                            new SpecializationPropertyDescription("Name", new List<string>(){ "Name" }),
                            new SpecializationPropertyDescription("Length", new List<string>(){ "Length" }),
                        }
                    };
                    break;
            }

            var factory = new GenericFactory(description);
            res.Objects = await factory.BuildDataset(dataset, iteration);
            return res;
        }
    }
}
