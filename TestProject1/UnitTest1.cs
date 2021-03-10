using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApplication2.Controllers;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EmployeeListUnitTest()
        {
            //Arrange
            var obj = new HomeController();

            //Act
            var result = obj.EmployeeLists();
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public void EmployeeFilterUnitTest()
        {
            //Arrange
            var obj = new HomeController();
            string emp = "tester";

            //Act
            var result = obj.EmployeeFilter(empSelected: emp);
            var okResult = result as OkObjectResult;

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}