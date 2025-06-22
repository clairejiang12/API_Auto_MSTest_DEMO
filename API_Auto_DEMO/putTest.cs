using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestMethodLibrary;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAutoTestPut
{

    [TestClass]
    public class APITestPut
    {
        HttpClient httpClient = new HttpClient();
        string postURL = "api/TodoItems";
        string putURL = "api/TodoItems/";
        private int id = 0;
        private object? res = null;
        public TestContext TestContext { get; set; }


        [TestMethod]
        [Description("Put with valid payload")]
        public async Task Test_Put_Valid()
        {
            // insert an item to update
            var jsonData = "{ \"name\": \"TestPut\",\"isComplete\": true}";
            var result = await RequestMethodClass.PostAsync(httpClient, postURL, jsonData, true);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");
            dynamic? actual = JsonConvert.DeserializeObject(result.ResponseBody!);
            if (actual == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            id = actual.id ?? 0;

            //Test Put API
            var putPayload = "{ \"name\": \"TestPutNew\",\"isComplete\": false}";

            //Call PutAsync method from RequestMethodClass
            var putResponse = await RequestMethodClass.PutAsync(httpClient, putURL + id, putPayload, true);
            TestContext.WriteLine($"Status Code: {putResponse.StatusCode}");
            TestContext.WriteLine($"Response Body: {putResponse.ResponseBody}");

            // Assert
            if (string.IsNullOrEmpty(putResponse.ResponseBody))
            {
                Assert.Fail("ResponseBody is null or empty");
            }
            dynamic? actualPut = JsonConvert.DeserializeObject(putResponse.ResponseBody!);
            if (actualPut == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            res = actualPut;
            ((string?)actualPut.name).Should().Be("TestPutNew");
            ((bool?)actualPut.isComplete).Should().BeFalse();
        }
    }
}