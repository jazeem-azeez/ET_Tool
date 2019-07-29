using System.Collections.Generic;
using ET_Tool.Business;
using ET_Tool.Business.Mappers;
using ET_Tool.Common.Logger;
using ET_Tool.Common.Models;
using LumenWorks.Framework.IO.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ET_Tool.L0.Tests.DataResolvers
{
    [TestClass]
    public class DataLookUpCollectionTests
    {
        private MockRepository mockRepository;

        private Mock<IDataSource> mockDataSource;
        private Mock<IEtLogger> mockEtLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockDataSource = this.mockRepository.Create<IDataSource>();
            this.mockEtLogger = new Mock<IEtLogger>();
        }

        [TestCleanup]
        public void TestCleanup() => this.mockRepository.VerifyAll();

        private DataLookUpCollection CreateDataLookUpCollection() => new DataLookUpCollection(
                this.mockDataSource.Object,
                this.mockEtLogger.Object);

        [TestMethod]
        public void LookUp_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            this.mockDataSource.Setup((ctx) => ctx.GetHeaders()).Returns(new string[] { "col1", "col2" });

            DataCell cell1 = new DataCell(new Column() { Name = "col1" }, "", "val1");
            DataCell cell2 = new DataCell(new Column() { Name = "col2" }, "", "val2");

            DataCell cell3 = new DataCell(new Column() { Name = "col1" }, "", "val3");
            DataCell cell4 = new DataCell(new Column() { Name = "col2" }, "", "val4");

            DataCellCollection row1 = new DataCellCollection
            {
                Cells = new List<DataCell> { cell1, cell2 }
            };
            DataCellCollection row2 = new DataCellCollection
            {
                Cells = new List<DataCell> { cell3, cell4 }
            };
            List<DataCellCollection> rows = new List<DataCellCollection>()
            {
                row1 ,row2
            };

            this.mockDataSource.Setup((ctx) => ctx.GetDataRowEntries()).Returns(rows);

            DataLookUpCollection dataLookUpCollection = this.CreateDataLookUpCollection();
            string keyColumn = "col1";
            string valueColumn = "col2";
            string key = "val1";

            // Act
            string result = dataLookUpCollection.LookUp(
                keyColumn,
                valueColumn,
                key);

            // Assert
            Assert.IsTrue(result=="val2");
        }
    }
}
