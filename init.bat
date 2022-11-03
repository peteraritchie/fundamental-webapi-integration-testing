@echo off
setlocal

set solutionName=WebApiIntegrationTests
md %solutionName%
pushd %solutionName%

dotnet new solution >NUL 2>&1 || call :exit-error creating solution

@echo creating WebApi project...
dotnet new webapi -lang c# -o WebApi --no-restore --use-minimal-apis true --framework net6.0 --use-program-main false >NUL 2>&1 || call :exit-error creating WebApi project
echo public partial class Program { } >> WebApi\Program.cs
dotnet sln add WebApi\WebApi.csproj >NUL 2>&1 || call :exit-error adding WebApi to solution


@echo creating IntegrationTests project...
dotnet new xunit -lang c# -o IntegrationTests --no-restore --framework net6.0 >NUL 2>&1 || call :exit-error creating IntegrationTests project
del IntegrationTests\UnitTest1.cs >NUL 2>&1 || call :exit-error deleting unitest1.cs
copy ..\..\WebApplication3\TestProject1\WebApiShould*.cs IntegrationTests >NUL 2>&1 || call :exit-error copying WebApiShould*.cs 
copy ..\..\WebApplication3\TestProject1\MyApplicationFactory.cs IntegrationTests >NUL 2>&1 || call :exit-error MyApplicationFactory.cs
dotnet add IntegrationTests\IntegrationTests.csproj reference WebApi\WebApi.csproj >NUL 2>&1 || call :exit-error Adding WebApi project
dotnet add IntegrationTests\IntegrationTests.csproj package Microsoft.OpenApi.Readers >NUL 2>&1 || call :exit-error adding Readers package
dotnet add IntegrationTests\IntegrationTests.csproj package Microsoft.AspNetCore.Mvc.Testing  >NUL 2>&1 || call :exit-error adding Testing package
dotnet sln add IntegrationTests\IntegrationTests.csproj >NUL 2>&1 || call :exit-error adding IntegrationTests to solution

@echo creating UnitTests project...
dotnet new xunit -lang c# -o UnitTests --no-restore --framework net6.0 >NUL 2>&1 || call :exit-error creating UnitTests project
del UnitTests\UnitTest1.cs >NUL 2>&1 || call :exit-error deleting UnitTest1.cs
dotnet add UnitTests\UnitTests.csproj reference WebApi\WebApi.csproj >NUL 2>&1 || call :exit-error adding reference to WebApi
dotnet add UnitTests\UnitTests.csproj package Microsoft.OpenApi.Readers 1>NUL >NUL 2>&1 || call :exit-error adding Readers package
dotnet add UnitTests\UnitTests.csproj package Microsoft.AspNetCore.Mvc.Testing 1>NUL >NUL 2>&1 || call :exit-error adding Testing package
dotnet sln add UnitTests\UnitTests.csproj >NUL 2>&1 || call :exit-error adding UnitTests to solution

dotnet test

popd

goto:eof
:exit-error
echo error %0
@EXIT /b 1

