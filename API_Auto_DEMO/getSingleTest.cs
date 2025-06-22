using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAutoTestGetSingle
{

    [TestClass]
    public class APITestGetSingle
    {
        HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        string getURL = "api/TodoItems/";
        private int id = 0;
        private object? res = null;
        public TestContext TestContext { get; set; }


        [TestMethod]
        [Description("Get by Id and check success")]
        public async Task Test_Get_By_Id_Valid()
        {
            // insert an item to get
            var jsonData = "{ \"name\": \"TestGet\",\"isComplete\": true}";
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");
            dynamic? actual = JsonConvert.DeserializeObject(result.ResponseBody!);
            if (actual == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            id = actual.id ?? 0;

            //Test Get Single API
            var getResult = await RequestMethodClass.GetAsync(httpClient, getURL + id, true);
            TestContext.WriteLine($"Status Code: {getResult.StatusCode}");
            TestContext.WriteLine($"Response Body: {getResult.ResponseBody}");

            // Assert
            getResult.ResponseBody.Should().NotBeNullOrEmpty();
            if (string.IsNullOrEmpty(getResult.ResponseBody))
            {
                Assert.Fail("ResponseBody is null or empty");
            }
            dynamic? getActual = JsonConvert.DeserializeObject(getResult.ResponseBody!);
            if (getActual == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            ((string?)getActual.name).Should().Be("TestGet");
            ((bool?)getActual.isComplete).Should().BeTrue();
        }

        [TestMethod]
        [Description("Get by not exit id and check error")]
        public async Task Test_Get_By_Id_Not_Exist()
        {
            var result = await RequestMethodClass.GetAsync(httpClient, getURL + id, false);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");

            // Assert
            result.ResponseBody.Should().NotBeNullOrEmpty();
            if (string.IsNullOrEmpty(result.ResponseBody))
            {
                Assert.Fail("ResponseBody is null or empty");
            }
            dynamic? actual = JsonConvert.DeserializeObject(result.ResponseBody!);
            if (actual == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            actual.status.Should().Be(404);
        }

    }

}