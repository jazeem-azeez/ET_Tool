using ET_Tool.Business;
using ET_Tool.Business.DataCleaner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ET_Tool.L0.Tests.DataCleaner
{
    [TestClass]
    public class CsvDataCleanerTests
    {
        private MockRepository mockRepository;

        private Mock<IDataCleanerConfig> mockDataCleanerConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockDataCleanerConfig = this.mockRepository.Create<IDataCleanerConfig>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private CsvDataCleaner CreateCsvDataCleaner()
        {
            return new CsvDataCleaner(
                TODO,
                this.mockDataCleanerConfig.Object);
        }

        [TestMethod]
        public void CleanHeader_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var csvDataCleaner = this.CreateCsvDataCleaner();
            List columns = null;

            // Act
            csvDataCleaner.CleanHeader(
                columns);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void CleanRow_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var csvDataCleaner = this.CreateCsvDataCleaner();
            DataCellCollection result = null;

            // Act
            var result = csvDataCleaner.CleanRow(
                result);

            // Assert
            Assert.Fail();
        }
    }
}
