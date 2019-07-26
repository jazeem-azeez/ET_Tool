using System.Collections.Generic;

using ET_Tool.Business.Mappers;
using ET_Tool.Common.Logger;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace ET_Tool.L0.Tests.Et_Engine
{
    [TestClass]
    public class SourceToSinkDataChainBuilderTests
    {
        private Mock<IEtLogger> mockEtLogger;
        private MockRepository mockRepository;

        [TestMethod]
        public void BuildChain_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            SourceToSinkDataChainBuilder sourceToSinkDataChainBuilder = this.CreateSourceToSinkDataChainBuilder();

            //sourceToSinkDataChainBuilder.AddSourceColumns(@"Change,Country,Location,Name,NameWoDiacritics,Subdivision,Status,Function,Date,IATA,Coordinates,Remarks".Split(','));
            sourceToSinkDataChainBuilder.AddSourceColumns(@"Change,Country_code,Location_code,Location_Name,NameWoDiacritics,Subdivision,Status,FunctionCode,Date,IATA,Coordinates,Remarks".Split(','));
            sourceToSinkDataChainBuilder.AddSinkColumns(@"Country_code,country_name,Location_code,Location_Name,Location_Type,Longitude,Latitude".Split(',') );
            sourceToSinkDataChainBuilder.LookUps.Add("firstLkp", new HashSet<string>(@"Country_code,country_name".Split(',')) );
            sourceToSinkDataChainBuilder.LookUps.Add("secondLkp", new HashSet<string>(@"FunctionCode,Location_Type".Split(',')));
            sourceToSinkDataChainBuilder.LookUps.Add("coordinate_transformation", new HashSet<string>(@"Coordinates,Longitude,Latitude".Split(',')));

            // Act
            sourceToSinkDataChainBuilder.BuildChain();

            // Assert
            Assert.AreEqual(7, sourceToSinkDataChainBuilder.Chain.Count); 
        }

        [TestCleanup]
        public void TestCleanup() => this.mockRepository.VerifyAll();

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockEtLogger = new Mock<IEtLogger>();
        }

        private SourceToSinkDataChainBuilder CreateSourceToSinkDataChainBuilder() => new SourceToSinkDataChainBuilder(
                this.mockEtLogger.Object);
    }
}