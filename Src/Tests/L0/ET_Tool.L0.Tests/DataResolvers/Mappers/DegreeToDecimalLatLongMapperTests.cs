using System;
using System.Collections.Generic;

using ET_Tool.Business;
using ET_Tool.Business.Mappers;
using ET_Tool.Business.Mappers.Transformation;
using ET_Tool.Common.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace ET_Tool.L0.Tests.DataResolvers.Mappers
{
    [TestClass]
    public class DegreeToDecimalLatLongMapperTests
    {
        private MockRepository mockRepository;

        [TestMethod]
        public void BindToLookUpCollection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            DegreeToDecimalLatLongMapper degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();
            Dictionary<string, IDataLookUpCollection> globalLookUpCollection = new Dictionary<string, IDataLookUpCollection>();

            // Act
            try
            {
                degreeToDecimalLatLongMapper.BindToLookUpCollection(
                     globalLookUpCollection);
            }
            catch (Exception)
            {
                // Assert
                //NOP method
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ConvertDegreeAngleToDouble_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            DegreeToDecimalLatLongMapper degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();
            string point = "3519S";

            // Act
            string result = degreeToDecimalLatLongMapper.ConvertDegreeAngleToDouble(
                point);

            // Assert
            Assert.IsTrue(result == "-35.3166666666667");
        }

        [TestMethod]
        public void GetAssociatedColumns_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            DegreeToDecimalLatLongMapper degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();

            // Act
            HashSet<string> result = degreeToDecimalLatLongMapper.GetAssociatedColumns();

            // Assert
            Assert.IsTrue(result.Count == 3);
        }

        [TestMethod]
        public void Map_StateUnderTest_ExpectedBehavior_Latitude()
        {
            // Arrange
            DegreeToDecimalLatLongMapper degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();
            DataCellCollection sourceRows = new DataCellCollection();
            sourceRows.Add(new DataCell(new LumenWorks.Framework.IO.Csv.Column() { Name = Constants.columnkey }, "", "3519S 14804E"));

            string columnkey = Constants.latitudeKey;
            string value = "3519S 14804E";
            DataCellCollection currentState = new DataCellCollection();

            Dictionary<string, string> Context = new Dictionary<string, string>();
            // Act
            List<DataCell> result = degreeToDecimalLatLongMapper.Map(
                sourceRows,
                columnkey,
                value,
                Context,
                currentState);

            // Assert -35.3166666666667
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Value == "-35.3166666666667");
        }

        [TestMethod]
        public void Map_StateUnderTest_ExpectedBehavior_Longitude()
        {
            // Arrange
            DegreeToDecimalLatLongMapper degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();
            DataCellCollection sourceRows = new DataCellCollection();
            sourceRows.Add(new DataCell(new LumenWorks.Framework.IO.Csv.Column() { Name = Constants.columnkey }, "", "3519S 14804E"));

            string columnkey = Constants.longitudeKey;
            string value = "3519S 14804E";
            DataCellCollection currentState = new DataCellCollection();

            Dictionary<string, string> Context = new Dictionary<string, string>();
            // Act
            List<DataCell> result = degreeToDecimalLatLongMapper.Map(
                sourceRows,
                columnkey,
                value,
                Context,
                currentState);

            // Assert 148.066666666667
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result[0].Value == "148.066666666667");
        }

        [TestCleanup]
        public void TestCleanup() => this.mockRepository.VerifyAll();

        [TestInitialize]
        public void TestInitialize() => this.mockRepository = new MockRepository(MockBehavior.Strict);

        private DegreeToDecimalLatLongMapper CreateDegreeToDecimalLatLongMapper()
        {
            RuntimeArgs arg = new RuntimeArgs
            {
                DegreeToDecimalLatLongMapperSettings = new Dictionary<string, string>()
            {
                { Constants.columnkey, Constants.columnkey  },
                { Constants.latitudeKey, Constants.latitudeKey},
                { Constants.longitudeKey, Constants.longitudeKey }
            }
            };

            return new DegreeToDecimalLatLongMapper(
                arg);
        }
    }
}