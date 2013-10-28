# Dev Setup, Contribution Guides, and Tips


### Dev Environment Setup

Tools:

- PowerShell
- Visual Studio (or a tool to build a MSBuild based project (Xamarin Studio? - Send a pull request with updates to get it to play nice with that environment)
- Something that will automatically install the NuGet dependencies

Get the code:

    git clone https://github.com/staxmanade/ToTypeScriptD.git
    cd ToTypeScriptD

Get the [psake](https://github.com/psake/psake) build tool: You can install [psake](https://github.com/psake/psake) easily with [Chocolatey](http://chocolatey.org)

    cinst psake


### List build tasks

    psake ?

### Project Setup

Before you can open the solution and build you must first get the build tool to generate a `SharedItems\VersionInfo.cs` file.

    psake Create-VersionInfo

Now you should be able to open and build the solution.
