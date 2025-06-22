using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;

namespace APIAutoTestDelete
{
    [TestClass]
    public class APITestDelete
    {
        HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        string deleteURL = "api/TodoItems/";
        string getURL = "api/TodoItems/";
        private int id = 0;
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("Delete with exist id and check success")]
        public async Task Test_Delete_By_Id_Valid()
        {
            // insert an item to delete
            var jsonData = "{ \"name\": \"TestDelete\",\"isComplete\": true}";
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            var actual = ResponseHelper.AssertAndDeserialize<dynamic>(result.ResponseBody);
            id = actual.id ?? 0;
            //Test Delete API
            var deleteResult = await RequestMethodClass.DeleteAsync(httpClient, deleteURL + id, true);
            ResponseHelper.LogResult(TestContext, deleteResult.StatusCode.ToString(), deleteResult.ResponseBody);
            deleteResult.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent); // 204

            // verify the item is deleted
            var getResult = await RequestMethodClass.GetAsync(httpClient, getURL + id, false);
            TestContext.WriteLine($"Status Code: {getResult.StatusCode}");
            TestContext.WriteLine($"Response Body: {getResult.ResponseBody}");
            getResult.ResponseBody.Should().NotBeNullOrEmpty();
            var jobj = JObject.Parse(getResult.ResponseBody!);
            jobj["status"]!.Value<int>().Should().Be(404);
        }

        [TestMethod]
        [Description("Delete by not exit Id")]
        public async Task Test_Delete_By_Id_Not_Exit()
        {
            var result = await RequestMethodClass.DeleteAsync(httpClient, deleteURL + "121212121", false);
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            result.ResponseBody.Should().NotBeNullOrEmpty();
            var jobj = JObject.Parse(result.ResponseBody!);
            jobj["status"]!.Value<int>().Should().Be(404);
        }
    }

}