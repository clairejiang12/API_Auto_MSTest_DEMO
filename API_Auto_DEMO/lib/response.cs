using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace RequestMethodLibrary
{
    public static class ResponseHelper
    {
        public static void LogResult(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext, string statusCode, string responseBody)
        {
            testContext.WriteLine($"Status Code: {statusCode}");
            testContext.WriteLine($"Response Body: {responseBody}");
        }

        public static T AssertAndDeserialize<T>(string? responseBody)
        {
            if (string.IsNullOrEmpty(responseBody))
            {
                Assert.Fail("ResponseBody is null or empty");
            }
            var obj = JsonConvert.DeserializeObject<T>(responseBody!);
            if (obj == null)
            {
                Assert.Fail("Deserialized object is null");
            }
            return obj!;
        }
    }
}
