# .NET Core Credit Card by Genocs


This repo shown how to integrate credit card payment with Nexi. The repo is a POC. 

PLEASE: DO NOT USE IT IN PRODUCTION 

. The package version is hosted on [nuget](https://www.nuget.org/packages).


To build the project type following command
```ps
dotnet build .\src
```

To pack the project type following command
```ps
dotnet pack ./src

cd src/Genocs.Core
dotnet pack -p:NuspecFile=./Genocs.Core.nuspec --no-restore -o .
```


To push the project type following command
```ps
dotnet nuget push
dotnet nuget push *.nupkg -k $NUGET_API_KEY -s $NUGET_SOURCE
```

