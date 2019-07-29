using System.IO;
using ET_Tool.Business;
using ET_Tool.Common.IO;
using ET_Tool.Injection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ET_Tool.L1.Tests.Et_Engine
{
    [TestClass]
    public class ET_EngineTests
    {

        private readonly IET_Engine engine;
        private ServiceProvider serviceProvider;
        private RuntimeArgs runtimeSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            IDiskIOHandler diskIOHandler = new DiskIOHandler();
            runtimeSettings = JsonConvert.DeserializeObject<RuntimeArgs>(diskIOHandler.FileReadAllText("runtimeConfig.Json"));
            runtimeSettings.DataSourceFileName = Path.Combine(Directory.GetCurrentDirectory(), runtimeSettings.DataSourceFileName); 
            runtimeSettings.SourceDataFolder = Path.Combine(Directory.GetCurrentDirectory(), runtimeSettings.SourceDataFolder);
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(runtimeSettings);
            services.MainInjection();
            this.serviceProvider = services.BuildServiceProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        private IET_Engine CreateET_Engine() => this.serviceProvider.GetRequiredService<IET_Engine>();


        [TestMethod]
        public void InitializePrepocessing_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            IET_Engine eT_Engine = this.CreateET_Engine();

            // Act
            bool result = eT_Engine.InitializePrepocessing();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PerformAutoClean_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            IET_Engine eT_Engine = this.CreateET_Engine();
            
            string csvTypeDef = null;
            int attempt = 0;

            // Act
            bool result = eT_Engine.InitializePrepocessing();

             result = eT_Engine.PerformAutoClean(
                runtimeSettings.DataSourceFileName,
                csvTypeDef,
                attempt);

            // Assert            
            Assert.IsTrue(result);

        }

        [TestMethod]
        public void PerformTransformation_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            IET_Engine eT_Engine = this.CreateET_Engine();
            bool result = false;
            if (eT_Engine.RunDataAnalysis() && eT_Engine.InitializePrepocessing())
            {
                 
                result = eT_Engine.PerformTransformation();
            }
            // Act

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RunDataAnalysis_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            IET_Engine eT_Engine = this.CreateET_Engine();
            int attempt = 0;

            // Act
            bool result = eT_Engine.RunDataAnalysis(
                attempt);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
