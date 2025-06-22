using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAutoTestGetAll
{
    [TestClass]
    public class APITestGetAll
    {
        HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        string getAllURL = "api/TodoItems";
        private int id = 0;
        private object? res = null;
        public TestContext TestContext { get; set; }


        [TestMethod]
        [Description("Get All and check success")]
        public async Task Test_Get_All()
        {
            // Add new record for test Get ALL
            var jsonData = "{ \"name\": \"TestGet\",\"isComplete\": true}";
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");
            if (string.IsNullOrEmpty(result.ResponseBody))
            {
                Assert.Fail("ResponseBody is null or empty");
            }
            dynamic? actual = JsonConvert.DeserializeObject(result.ResponseBody!);
            if (actual == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            res = actual;


            //Test Get All API
            var getResult = await RequestMethodClass.GetAsync(httpClient, getAllURL, true);
            TestContext.WriteLine($"Status Code: {getResult.StatusCode}");
            TestContext.WriteLine($"Response Body: {getResult.ResponseBody}");

            // Assert
            getResult.ResponseBody.Should().NotBeNullOrEmpty();
            if (string.IsNullOrEmpty(getResult.ResponseBody))
            {
                Assert.Fail("ResponseBody is null or empty");
            }
            var array = JArray.Parse(getResult.ResponseBody!);
            if (res != null)
            {
                var expected = JObject.FromObject(res);
                array.Should().ContainEquivalentOf(expected);
            }
        }
    }

}