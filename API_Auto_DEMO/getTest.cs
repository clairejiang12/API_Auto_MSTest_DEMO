using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RequestMethodLibrary;

namespace APIAutoTest
{
    [TestClass]
    public class GetAPITest
    {
        HttpClient httpClient = new HttpClient();
        string endpointUrl = "http://localhost:5089/api/TodoItems";

        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task Test_Get_Data_From_Endpoint()
        {
            //Call GetAsync method from RequestMethodClass
            var result = await RequestMethodClass.GetAsync(httpClient, endpointUrl);
            TestContext.WriteLine($"Status Code: {result.StatusCode}");
            TestContext.WriteLine($"Response Body: {result.ResponseBody}");


            // Assert
            Assert.IsNotNull(result.ResponseBody);
        }
    }
}