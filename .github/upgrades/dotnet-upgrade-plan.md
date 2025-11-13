# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade GISBlox.Services.SDK\GISBlox.Services.SDK.csproj
4. Upgrade GISBlox.Services.SDK.Tests\GISBlox.Services.SDK.Tests.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

No projects are excluded from this upgrade.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                | Current Version | New Version | Description                                   |
|:--------------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.Extensions.Caching.Abstractions   | 9.0.9           | 10.0.0      | Recommended for .NET 10.0                     |
| Microsoft.Extensions.Caching.Memory         | 9.0.9           | 10.0.0      | Recommended for .NET 10.0                     |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### GISBlox.Services.SDK\GISBlox.Services.SDK.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`

NuGet packages changes:
  - Microsoft.Extensions.Caching.Abstractions should be updated from `9.0.9` to `10.0.0` (*recommended for .NET 10.0*)
  - Microsoft.Extensions.Caching.Memory should be updated from `9.0.9` to `10.0.0` (*recommended for .NET 10.0*)

#### GISBlox.Services.SDK.Tests\GISBlox.Services.SDK.Tests.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net10.0`
  - After upgrading, run the tests, but do not run any DataLakeTests.