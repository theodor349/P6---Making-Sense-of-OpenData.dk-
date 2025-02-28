﻿using FluentAssertions;
using DatasetGenerator.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasetGenerator.Test.Json
{
    [TestClass]
    public class ParseRealExampleTests
    {
        [TestMethod]
        public void Test_Simple()
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            var setup = new TestSetup();

            var inputString = "{\"type\": \"FeatureCollection\",\"name\": \"34.20.12_Parkeringsarealer\",\"crs\": {\"type\": \"name\",\"properties\": {\"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\"}}}";
            var expected = new DatasetObject(fileExtension.ToLower(), fileName.ToLower(), new List<IntermediateObject>()
            {
                new IntermediateObject(new List<ObjectAttribute>()
                {
                    new TextAttribute("type", "FeatureCollection"),
                    new TextAttribute("name", "34.20.12_Parkeringsarealer"),
                })
            });

            var parser = setup.GetParseJson();
            var task = parser.Parse(inputString, fileExtension, fileName);
            task.Wait();
            var res = task.Result;

            res.Objects.Should().BeEquivalentTo(expected.Objects);
        }

        [TestMethod]
        public void Test_OneObjInList()
        {
            string fileName = "fileName";
            string fileExtension = ".geojson";
            var setup = new TestSetup();

            var inputString = "{\"features\": [{\"type\": \"Feature\",\"properties\": {\"OBJECTID\": 267,\"P_PLADS_NR\": \"RK 1\",\"INDKOERSEL_VEJ\": \"Torvegade_ Enghavevej\",\"UDKOERSEL_VEJ\": \"Enghavevej\",\"EJER\": \"Institution\",\"ANTAL_PLADSER\": \"92\",\"HANDICAP_PLADSER\": \"2\",\"TIDSZONE\": \"3 timer\",\"BELAEGNING\": \"Asfalt\",\"AFMAERKNING\": \"Hvid\",\"ANTAL_LYSMASTER\": \"s20\",\"URL\": \"https://gisext.rksk.dk/pdf/PPlads/RK_1_Torvegade_Enghavevej.pdf\",\"BEMAERKNINGER\": \"kommunen holder_aftaler?\",\"GlobalID\": \"{A0864547-1F2E-43F4-87F5-34B5ECACEC1A}\",\"created_user\": null,\"created_date\": null,\"last_edited_user\": \"RKSK\",\"last_edited_date\": \"2019-12-17T07:23:46Z\",\"Brugernavn\": null,\"ShapeSTArea\": 3950.336602756815,\"ShapeSTLength\": 437.8971282813771},\"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[8.240479957252727,56.09079007170635],[8.240318595897797,56.090837686750646],[8.240047779834569,56.090904377291984],[8.239825529070766,56.09075226701193],[8.239608726838116,56.090590206517284],[8.240022603150605,56.09043407736101],[8.24019233686947,56.09056237798621],[8.240395238968205,56.09046966900493],[8.240585490036453,56.09038274916644],[8.240779697910575,56.0902939934467],[8.241035223152073,56.090177248322696],[8.241072187406699,56.09020206775729],[8.24123737902105,56.090191366335105],[8.24145804337276,56.090481386660706],[8.241567533899898,56.09059793116452],[8.241617303686505,56.090584300935404],[8.241815237368847,56.09079504913395],[8.241851540593345,56.090833692352234],[8.241681157785298,56.09088098333104],[8.241343630277166,56.09052731829657],[8.241219705532911,56.09052280917696],[8.241138757384064,56.09054854466553],[8.241082890945895,56.090491987788184],[8.24068850056752,56.090605738043045],[8.240689451412898,56.090610262501514],[8.240634176396334,56.09055912707765],[8.240351930550798,56.090678658608425],[8.240479957252727,56.09079007170635]]]}}]}";
            var expected = new DatasetObject(fileExtension.ToLower(), fileName.ToLower(), new List<IntermediateObject>()
            {
                new IntermediateObject(new List<ObjectAttribute>()
                {
                    new ListAttribute("features", new List<ObjectAttribute>()
                    {
                        new ListAttribute("StartObject", new List<ObjectAttribute>()
                        {
                            new TextAttribute("type", "Feature"),
                            new ListAttribute("properties", new List<ObjectAttribute>()
                            {
                                new LongAttribute("OBJECTID", 267),
                                new TextAttribute("P_PLADS_NR", "RK 1"),
                                new TextAttribute("INDKOERSEL_VEJ", "Torvegade_ Enghavevej"),
                                new TextAttribute("UDKOERSEL_VEJ", "Enghavevej"),
                                new TextAttribute("EJER", "Institution"),
                                new TextAttribute("ANTAL_PLADSER", "92"),
                                new TextAttribute("HANDICAP_PLADSER", "2"),
                                new TextAttribute("TIDSZONE", "3 timer"),
                                new TextAttribute("BELAEGNING", "Asfalt"),
                                new TextAttribute("AFMAERKNING", "Hvid"),
                                new TextAttribute("ANTAL_LYSMASTER", "s20"),
                                new TextAttribute("URL", "https://gisext.rksk.dk/pdf/PPlads/RK_1_Torvegade_Enghavevej.pdf"),
                                new TextAttribute("BEMAERKNINGER", "kommunen holder_aftaler?"),
                                new TextAttribute("GlobalID", "{A0864547-1F2E-43F4-87F5-34B5ECACEC1A}"),
                                new NullAttribute("created_user"),
                                new NullAttribute("created_date"),
                                new TextAttribute("last_edited_user", "RKSK"),
                                new DateAttribute("last_edited_date", new DateTime(2019,12,17,7,23,46)),
                                new NullAttribute("Brugernavn"),
                                new DoubleAttribute("ShapeSTArea", 3950.336602756815),
                                new DoubleAttribute("ShapeSTLength", 437.8971282813771),
                            }),
                            new ListAttribute("geometry", new List<ObjectAttribute>()
                            {
                                new TextAttribute("type", "Polygon"),
                                new ListAttribute("coordinates", new List<ObjectAttribute>()
                                {
                                    new ListAttribute("StartArray", new List<ObjectAttribute>()
                                    {
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240479957252727), new DoubleAttribute("DoubleValue", 56.09079007170635),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240318595897797), new DoubleAttribute("DoubleValue", 56.090837686750646), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240047779834569), new DoubleAttribute("DoubleValue", 56.090904377291984), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.239825529070766), new DoubleAttribute("DoubleValue", 56.09075226701193),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.239608726838116), new DoubleAttribute("DoubleValue", 56.090590206517284), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240022603150605), new DoubleAttribute("DoubleValue", 56.09043407736101),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.24019233686947),  new DoubleAttribute("DoubleValue", 56.09056237798621),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240395238968205), new DoubleAttribute("DoubleValue", 56.09046966900493),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240585490036453), new DoubleAttribute("DoubleValue", 56.09038274916644),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240779697910575), new DoubleAttribute("DoubleValue", 56.0902939934467),   }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241035223152073), new DoubleAttribute("DoubleValue", 56.090177248322696), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241072187406699), new DoubleAttribute("DoubleValue", 56.09020206775729),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.24123737902105),  new DoubleAttribute("DoubleValue", 56.090191366335105), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.24145804337276),  new DoubleAttribute("DoubleValue", 56.090481386660706), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241567533899898), new DoubleAttribute("DoubleValue", 56.09059793116452),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241617303686505), new DoubleAttribute("DoubleValue", 56.090584300935404), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241815237368847), new DoubleAttribute("DoubleValue", 56.09079504913395),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241851540593345), new DoubleAttribute("DoubleValue", 56.090833692352234), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241681157785298), new DoubleAttribute("DoubleValue", 56.09088098333104),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241343630277166), new DoubleAttribute("DoubleValue", 56.09052731829657),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241219705532911), new DoubleAttribute("DoubleValue", 56.09052280917696),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241138757384064), new DoubleAttribute("DoubleValue", 56.09054854466553),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.241082890945895), new DoubleAttribute("DoubleValue", 56.090491987788184), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.24068850056752),  new DoubleAttribute("DoubleValue", 56.090605738043045), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240689451412898), new DoubleAttribute("DoubleValue", 56.090610262501514), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240634176396334), new DoubleAttribute("DoubleValue", 56.09055912707765),  }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240351930550798), new DoubleAttribute("DoubleValue", 56.090678658608425), }),
                                        new ListAttribute("StartArray", new List<ObjectAttribute>() { new DoubleAttribute("DoubleValue", 8.240479957252727), new DoubleAttribute("DoubleValue", 56.09079007170635),  }),
                                    }),
                                }),
                            }),
                        }),
                    }),
                }),
            });

            var parser = setup.GetParseJson();
            var task = parser.Parse(inputString, fileExtension, fileName);
            task.Wait();
            var res = task.Result;

            res.Should().BeEquivalentTo(expected);
        }
    }
}
