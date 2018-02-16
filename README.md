# butterfly-csharp
[![Build status](https://ci.appveyor.com/api/projects/status/2t22kvna6nokj80n?svg=true)](https://ci.appveyor.com/project/liuhaoyang/butterfly-csharp)  

A .NET client for Butterfly-APM.  

# Example
* start [butterfly-server](https://github.com/ButterflyAPM/butterfly)
* `git clone https://github.com/ButterflyAPM/butterfly-csharp.git`
* `cd butterfly-csharp`
* `dotnet run -p sample/Butterfly.Client.Sample.Frontend` and `dotnet run -p sample/Butterfly.Client.Sample.Backend`
* browse to [http://localhost:5001/api/values](http://localhost:5001/api/values)
* browse to [http://localhost:9618](http://localhost:9618) to view traces

# Quickstart
* create new Asp.Net Core web project.
* `Install-Package Butterfly.Client.AspNetCore`
* register `Butterfly services`  in `ConfigureServices` method (the `CollectorUrl` and `Service` options are required)
  ```
  public void ConfigureServices(IServiceCollection services)
  {
     //your other code 
    services.AddButterfly(option =>
    {
        option.CollectorUrl = "http://localhost:9618";
        option.Service = "my service";
    });
  }
  ```
* run your application and browse to [http://localhost:9618](http://localhost:9618) to view traces
