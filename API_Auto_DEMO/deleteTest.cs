using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAutoTestDelete
{

    [TestClass]
    public class APITestDelete
    {
        HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        string deleteURL = "api/TodoItems/";
        private int id = 0;
        private object? res = null;
        public TestContext TestContext { get; set; }


        [TestMethod]
        [Description("Delete with exist id and check success")]
        public async Task Test_Delete_By_Id_Valid()
        {
            // insert an item to delete
            var jsonData = "{ \"name\": \"TestDelete\",\"isComplete\": true}";
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");
            dynamic? actual = JsonConvert.DeserializeObject(result.ResponseBody!);
            if (actual == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            id = actual.id ?? 0;
            //Test Delete API
            var deleteResult = await RequestMethodClass.DeleteAsync(httpClient, deleteURL + id, true);
            TestContext.WriteLine($"Status Code: {deleteResult.StatusCode}");
            TestContext.WriteLine($"Response Body: {deleteResult.ResponseBody}");
        }

        [TestMethod]
        [Description("Delete by not exit Id")]
        public async Task Test_Delete_By_Id_Not_Exit()
        {
            var result = await RequestMethodClass.DeleteAsync(httpClient, deleteURL + "11111", false);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");
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