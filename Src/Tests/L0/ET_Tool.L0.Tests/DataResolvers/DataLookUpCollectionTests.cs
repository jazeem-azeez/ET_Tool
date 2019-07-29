using ET_Tool.Business;
using ET_Tool.Business.Mappers;
using ET_Tool.Common.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

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
            this.mockEtLogger = this.mockRepository.Create<IEtLogger>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private DataLookUpCollection CreateDataLookUpCollection()
        {
            return new DataLookUpCollection(
                this.mockDataSource.Object,
                this.mockEtLogger.Object);
        }

        [TestMethod]
        public void LookUp_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataLookUpCollection = this.CreateDataLookUpCollection();
            string keyColumn = null;
            string valueColumn = null;
            string key = null;

            // Act
            var result = dataLookUpCollection.LookUp(
                keyColumn,
                valueColumn,
                key);

            // Assert
            Assert.Fail();
        }
    }
}
