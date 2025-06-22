using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Net;
using System.Text;

namespace APIAutoTestPost
{
    public class TodoItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isComplete { get; set; }
    }

    [TestClass]
    public class APITestPost
    {
        private static readonly HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        private int id = 0;
        private TodoItem? res = null;
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("POST with valid payload and check success")]
        public async Task Test_Post_Valid()
        {
            // Arrange
            var jsonData = "{ \"name\": \"TestPost\",\"isComplete\": true}";

            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);

            // Assert
            var actual = ResponseHelper.AssertAndDeserialize<TodoItem>(result.ResponseBody);
            res = actual;
            id = actual.id;
            actual.name.Should().Be("TestPost");
            actual.isComplete.Should().BeTrue();
        }

        [TestMethod]
        [Description("POST with invalid isComplete value and check failed")]
        public async Task Test_Post_String_IsComplete()
        {
            // Arrange
            var jsonData = "{ \"name\": \"TestPost\",\"isComplete\": \"aaa\"}";

            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, false);
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest); // 400
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