using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RequestMethodLibrary;

namespace APIAutoTestPost
{
    [TestClass]
    public class PostAPITest
    {
		HttpClient httpClient = new HttpClient();
		string endpointUrl = "http://localhost:5089/api/TodoItems";

		public TestContext TestContext { get; set; }

		[TestMethod]
        public async Task Test_Post_Data_From_Endpoint()
        {
            // Arrange
          
            var jsonData = "{ \"id\": 1, \"name\": \"TestPost\",\"isComplete\": true}";


			//Call GetAsync method from RequestMethodClass
			var result = await RequestMethodClass.PostAsync(httpClient, endpointUrl, jsonData, false);
			TestContext.WriteLine($"Status Code: {result.StatusCode}");
			TestContext.WriteLine($"Response Body: {result.ResponseBody}");


            // Assert
            //Assert.AreEqual(responseBody, []);
        }
    }
}