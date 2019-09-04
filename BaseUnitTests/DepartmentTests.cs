using NUnit.Framework;
using System.Collections.Generic;
using VRS_Base.Controllers;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class DepartmentTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAllDepartments()
        {
            var departments = new DepartmentsController();
            var data = departments.Get().Value;
            IEnumerable<string> expectedData = new List<string>() { "a", "b", "c", "d", "e" };
            Assert.AreEqual(expectedData, data, "departments don't match");

        }
    }
}