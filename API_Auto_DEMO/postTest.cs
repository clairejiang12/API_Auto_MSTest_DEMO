using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAutoTestPost
{
        [TestClass]
    public class APITestPost
    {
        HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        private int id = 0;
        private object? res = null;
        public TestContext TestContext { get; set; }


        [TestMethod]
        [Description("POST with valid payload and check success")]
        public async Task Test_Post_Valid()
        {
            // Arrange
            var jsonData = "{ \"name\": \"TestPost\",\"isComplete\": true}";

            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");

            // Assert
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
            id = actual.id ?? 0;
            ((string?)actual.name).Should().Be("TestPost");
            ((bool?)actual.isComplete).Should().BeTrue();
        }


        [TestMethod]
        [Description("POST with invalid payload and check failed")]
        public async Task Test_Post_Invalid()
        {
            // Arrange
            var jsonData = "{\"isComplete\": true}";

            //Call GetAsync method from RequestMethodClass
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, false);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");
        }

        [TestMethod]
        [Description("POST with invalid isComplete value and check failed")]
        public async Task Test_Post_String_IsComplete()
        {
            // Arrange
            var jsonData = "{ \"name\": \"TestPost\",\"isComplete\": \"aaa\"}";

            //Call GetAsync method from RequestMethodClass
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, false);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");

            if (string.IsNullOrEmpty(result.ResponseBody))
            {
                Assert.Fail("ResponseBody is null or empty");
            }
            var errorObj = JObject.Parse(result.ResponseBody!);
            errorObj["status"]?.Value<int>().Should().Be(400);
            var todoItemErrors = errorObj["errors"]?["todoItem"] as JArray;
            todoItemErrors.Should().NotBeNull();
            todoItemErrors!.First!.Value<string>().Should().Be("The todoItem field is required.");
        }
    }

}