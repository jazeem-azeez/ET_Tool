using ET_Tool.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ET_Tool.L0.Tests
{
    [TestClass]
    public class CsvParseHelperTests
    {
        private MockRepository mockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private CsvParseHelper CreateCsvParseHelper()
        {
            return new CsvParseHelper();
        }

        [TestMethod]
        public void GetFields_StateUnderTest_ExpectedBehavior()
        {
            // Arrange 
            string line = ",AT,MLD,\"Mollersdorf, Baden\",\"Mollersdorf, Baden\",3,RL,--3-----,1301,,4801N 01618E,";


            // Act
            var result = CsvParseHelper.GetAllFields(
                line);
            // Assert
            Assert.IsTrue(result.Length==12);
            // Arrange 
            line = "FunctionCode,FunctionDescription";


            // Act
            result = CsvParseHelper.GetAllFields(
                line);
            // Assert

            Assert.IsTrue(result.Length == 2);   
            // Arrange 
            line = ",MA,MDT,Midelt,Midelt,KHN,AA,--3-----,1607,,3240N 00444W,\"";


            // Act
            result = CsvParseHelper.GetAllFields(
                line);
            // Assert

            Assert.IsTrue(result.Length == 12);


        }

        //[TestMethod]
        //public void GetAllFields_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var csvParseHelper = this.CreateCsvParseHelper();
        //    string line = null;

        //    // Act
        //    var result = csvParseHelper.GetAllFields(
        //        line);

        //    // Assert
        //    Assert.Fail();
        //}
    }
}
