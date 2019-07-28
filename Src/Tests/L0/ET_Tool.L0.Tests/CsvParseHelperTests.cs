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
            var csvParseHelper = this.CreateCsvParseHelper();
            string line = @",AT,MA5,Maria Alm am Steinernen Meer,Maria Alm am Steinernen Meer,5,RL,--3-----,1601,,4724N 01254E,";
           

            // Act
            var result = csvParseHelper.GetFields(
                line);

            // Assert
            Assert.IsTrue(result.Length==12);
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
