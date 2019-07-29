using ET_Tool.Business;
using ET_Tool.Business.Mappers;
using ET_Tool.Common.IO;
using ET_Tool.Common.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ET_Tool.L1.Tests.Et_Engine
{
    [TestClass]
    public class ET_EngineTests
    {
        private MockRepository mockRepository;

        private Mock<IDataSourceFactory> mockDataSourceFactory;
        private Mock<IDataResolver> mockDataResolver;
        private Mock<IDataSinkFactory> mockDataSinkFactory;
        private Mock<IEtLogger> mockEtLogger;
        private Mock<IDiskIOHandler> mockDiskIOHandler;
        private Mock<RuntimeArgs> mockRuntimeArgs;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockDataSourceFactory = this.mockRepository.Create<IDataSourceFactory>();
            this.mockDataResolver = this.mockRepository.Create<IDataResolver>();
            this.mockDataSinkFactory = this.mockRepository.Create<IDataSinkFactory>();
            this.mockEtLogger = this.mockRepository.Create<IEtLogger>();
            this.mockDiskIOHandler = this.mockRepository.Create<IDiskIOHandler>();
            this.mockRuntimeArgs = this.mockRepository.Create<RuntimeArgs>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private ET_Engine CreateET_Engine()
        {
            return new ET_Engine(
                this.mockDataSourceFactory.Object,
                this.mockDataResolver.Object,
                this.mockDataSinkFactory.Object,
                this.mockEtLogger.Object,
                this.mockDiskIOHandler.Object,
                this.mockRuntimeArgs.Object);
        }

        [TestMethod]
        public void Dispose_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var eT_Engine = this.CreateET_Engine();

            // Act
            eT_Engine.Dispose();

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void InitializePrepocessing_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var eT_Engine = this.CreateET_Engine();

            // Act
            var result = eT_Engine.InitializePrepocessing();

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void PerformAutoClean_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var eT_Engine = this.CreateET_Engine();
            string dataSourceFileName = null;
            string csvTypeDef = null;
            int attempt = 0;

            // Act
            var result = eT_Engine.PerformAutoClean(
                dataSourceFileName,
                csvTypeDef,
                attempt);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void PerformTransformation_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var eT_Engine = this.CreateET_Engine();

            // Act
            eT_Engine.PerformTransformation();

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void RunDataAnalysis_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var eT_Engine = this.CreateET_Engine();
            int attempt = 0;

            // Act
            var result = eT_Engine.RunDataAnalysis(
                attempt);

            // Assert
            Assert.Fail();
        }
    }
}
