# Blazor Client Web Assembly Deployment 

Initially created the MudBlazor admin dashboard sample using the following CLI command

```
dotnet new mudblazor --host wasm --name BlazorGolf -t admindashboard
```

This enabled local build. Checked in the solution.
Created resource group in Azure for the BlazorClient app
Created an Azure SWA app and then configured for the following:

- Used current GitHub repo
- specified blank Functions url
- specified /src/BlazorGolfClient as the app path
- specified /wwwroot as the output path
- Azure SWA configured the workflow (GH Action) and kicked of a deployment. 

Deploy was successful and could access app from Azure SWA endpoint.


## Links and References 
[Build Web Applications with Blazor](https://docs.microsoft.com/en-us/learn/modules/use-pages-routing-layouts-control-blazor-navigation/1-introduction)

[Designing and Building Enterprise Blazor Applications](https://app.pluralsight.com/library/courses/designing-building-enterprise-blazor-applications/table-of-contents)

[Alex Wolf Blazor Enterprise Repo](https://github.com/alex-wolf-ps/blazor-enterprise)