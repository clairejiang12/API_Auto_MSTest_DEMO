using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Net;
using System.Text;

namespace APIAutoTestPut
{
    [TestClass]
    public class APITestPut
    {
        HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        string putURL = "api/TodoItems/";
        string getURL = "api/TodoItems/";
        private int id = 0;
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("Put with valid payload")]
        public async Task Test_Put_Valid()
        {
            // insert an item to update
            var jsonData = "{ \"name\": \"TestPut\",\"isComplete\": true}";
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            var actual = ResponseHelper.AssertAndDeserialize<dynamic>(result.ResponseBody);
            id = actual.id ?? 0;

            //Test Put API
            var putPayload = $"{{ \"id\": {id}, \"name\": \"TestPutNew\", \"isComplete\": false }}";
            var putResponse = await RequestMethodClass.PutAsync(httpClient, putURL + id, putPayload, true);
            ResponseHelper.LogResult(TestContext, putResponse.StatusCode.ToString(), putResponse.ResponseBody);
            putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent); // 204

            //verify the updated item correct
            var getResult = await RequestMethodClass.GetAsync(httpClient, getURL + id, true);
            ResponseHelper.LogResult(TestContext, getResult.StatusCode.ToString(), getResult.ResponseBody);
            getResult.ResponseBody.Should().NotBeNullOrEmpty();
            var getActual = ResponseHelper.AssertAndDeserialize<dynamic>(getResult.ResponseBody);
            ((string?)getActual.name).Should().Be("TestPutNew");
            ((bool?)getActual.isComplete).Should().BeFalse();
        }

        [TestMethod]
        [Description("Put with invalid payload and check failed")]
        public async Task Test_Put_Invalid()
        {
            // Arrange
            var jsonData = "{\"name\": \"TestPutInvalid\",\"isComplete\": true}";
            var result = await RequestMethodClass.PutAsync(httpClient, putURL + id, jsonData, false);
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            result.StatusCode.Should().Be(HttpStatusCode.NotFound); // 404
        }
    }
}