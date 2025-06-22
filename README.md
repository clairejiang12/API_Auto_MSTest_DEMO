# API_Auto_MSTest_DEMO

## Introduction

API_Auto_MSTest_DEMO is a .NET 8 automated API testing project using MSTest as the test framework. It demonstrates how to perform integration tests for a RESTful API (such as a TodoItems API) with reusable HTTP request helpers, response validation, and strong-typed assertions. The project leverages FluentAssertions for expressive assertions and Newtonsoft.Json for JSON serialization/deserialization.

## Code Structure
API_Auto_MSTest_DEMO/  
&nbsp;API_Auto_DEMO/  
&nbsp;lib/  
&nbsp;&nbsp;request.cs         # HTTP request helper methods (GET, POST, PUT, DELETE)  
&nbsp;&nbsp;response.cs        # Response logging and deserialization helpers  
&nbsp;postTest.cs            # POST API test cases  
&nbsp;&nbsp;putTest.cs             # PUT API test cases  
&nbsp;&nbsp;deleteTest.cs          # DELETE API test cases  
&nbsp;&nbsp;getSingleTest.cs       # GET single item test cases  
&nbsp;&nbsp; getAllTest.cs          # GET all items test cases  
&nbsp;&nbsp;API_Auto_DEMO.csproj   # Project file with dependencies  
&nbsp;README.md  
## Dependencies

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [MSTest](https://www.nuget.org/packages/MSTest) (Unit test framework)
- [FluentAssertions](https://www.nuget.org/packages/FluentAssertions) (for expressive assertions)
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json) (for JSON serialization/deserialization)
- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk) (test runner)

All dependencies are managed via NuGet and specified in `API_Auto_DEMO.csproj`.

## Pre-conditions

- .NET 8 SDK must be installed on your machine.
- The target API (e.g., TodoItems API) should be running and accessible at `http://localhost:5089/`.
- NuGet packages will be restored automatically on build, but you can run `dotnet restore` manually if needed.

## How to Execute

1. **Clone the repository**  
    ``git clone git@github.com:clairejiang12/API_Auto_MSTest_DEMO.git``

    ``cd API_Auto_MSTest_DEMO``
2. **Build the project**  
   ``dotnet build API_Auto_DEMO``
3. **Run the tests**  
   Result mode 
   ``dotnet test API_Auto_DEMO``
   Details mode with console
   ``dotnet test --logger:"console;verbosity=detailed``
   This will execute all test cases in the project and output the results.

## Notes

- The API endpoint URL is hardcoded as `http://localhost:5089/` in `request.cs`. Update this if your API runs on a different address.
- The project uses MSTest attributes (`[TestClass]`, `[TestMethod]`) for test discovery.
- Helper methods for logging and deserialization are in `lib/response.cs` for code reuse.
- FluentAssertions is used for readable and maintainable assertions.

---

Feel free to further customize this README for your team or CI/CD environment!