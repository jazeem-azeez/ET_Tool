using ET_Tool.Business;
using ET_Tool.Business.Mappers.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ET_Tool.L0.Tests.DataResolvers.Mappers
{
    [TestClass]
    public class DegreeToDecimalLatLongMapperTests
    {
        private MockRepository mockRepository;

        private Mock<RuntimeArgs> mockRuntimeArgs;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockRuntimeArgs = this.mockRepository.Create<RuntimeArgs>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private DegreeToDecimalLatLongMapper CreateDegreeToDecimalLatLongMapper()
        {
            return new DegreeToDecimalLatLongMapper(
                this.mockRuntimeArgs.Object);
        }

        [TestMethod]
        public void BindToLookUpCollection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();
            Dictionary globalLookUpCollection = null;

            // Act
            degreeToDecimalLatLongMapper.BindToLookUpCollection(
                globalLookUpCollection);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void ConvertDegreeAngleToDouble_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();
            string point = null;

            // Act
            var result = degreeToDecimalLatLongMapper.ConvertDegreeAngleToDouble(
                point);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void GetAssociatedColumns_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();

            // Act
            var result = degreeToDecimalLatLongMapper.GetAssociatedColumns();

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void Map_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var degreeToDecimalLatLongMapper = this.CreateDegreeToDecimalLatLongMapper();
            DataCellCollection sourceRows = null;
            string columnkey = null;
            string value = null;
            Dictionary Context = null;
            DataCellCollection currentState = null;

            // Act
            var result = degreeToDecimalLatLongMapper.Map(
                sourceRows,
                columnkey,
                value,
                Context,
                currentState);

            // Assert
            Assert.Fail();
        }
    }
}
