using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;

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
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            var actual = ResponseHelper.AssertAndDeserialize<dynamic>(result.ResponseBody);
            id = actual.id ?? 0;

            //Test Get Single API
            var getResult = await RequestMethodClass.GetAsync(httpClient, getURL + id, true);
            ResponseHelper.LogResult(TestContext, getResult.StatusCode.ToString(), getResult.ResponseBody);
            getResult.ResponseBody.Should().NotBeNullOrEmpty();
            var getActual = ResponseHelper.AssertAndDeserialize<dynamic>(getResult.ResponseBody);
            ((string?)getActual.name).Should().Be("TestGet");
            ((bool?)getActual.isComplete).Should().BeTrue();
        }

        [TestMethod]
        [Description("Get by not exit id and check error")]
        public async Task Test_Get_By_Id_Not_Exist()
        {
            var result = await RequestMethodClass.GetAsync(httpClient, getURL + "11212121212", false);
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            result.ResponseBody.Should().NotBeNullOrEmpty();
            var jobj = JObject.Parse(result.ResponseBody!);
            jobj["status"]!.Value<int>().Should().Be(404);
        }
    }
}