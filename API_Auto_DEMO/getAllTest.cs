using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;

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
            ResponseHelper.LogResult(TestContext, result.StatusCode.ToString(), result.ResponseBody);
            var actual = ResponseHelper.AssertAndDeserialize<dynamic>(result.ResponseBody);
            res = actual;

            //Test Get All API
            var getResult = await RequestMethodClass.GetAsync(httpClient, getAllURL, true);
            ResponseHelper.LogResult(TestContext, getResult.StatusCode.ToString(), getResult.ResponseBody);
            getResult.ResponseBody.Should().NotBeNullOrEmpty();
            var array = JArray.Parse(getResult.ResponseBody!);
            if (res != null)
            {
                var expected = JObject.FromObject(res);
                array.Should().ContainEquivalentOf(expected);
            }
        }
    }
}