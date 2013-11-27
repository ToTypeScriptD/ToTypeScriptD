# Dev Setup, Contribution Guides, and Tips

### Easy way to contribute.

If you notice the tool is not generating something correctly. The easiest way to contribute would be to 
(sign in with your github account). Edit a [test project's](https://github.com/staxmanade/ToTypeScriptD/tree/master/ToTypeScriptD.Tests)
`*.approved.txt` type definition specs to what it should be generating. You can submit 
that as a pull request (add some comments) all within your browser and I'll look to see
how to adjust the generation of TypeScript Definitions to support your scenario.

### The preferred way to contribute.

- Setup your dev environment
- Fix the issue
- Submit a pull request

#### Dev Environment Setup

Tools:

- PowerShell
- Visual Studio (or a tool to build a MSBuild based project (Xamarin Studio? - Send a pull request with updates to get it to play nice with that environment)
- [TypeScript Visual Studio Extension](http://typescriptlang.org)
- Something that will automatically install the NuGet dependencies (EX: Visual Studio)
- Get the [psake](https://github.com/psake/psake) build tool: You can install [psake](https://github.com/psake/psake) easily with [Chocolatey](http://chocolatey.org)

    `cinst psake`

#### Get the code:

    git clone https://github.com/staxmanade/ToTypeScriptD.git
    cd ToTypeScriptD

#### List build tasks

    psake ?

#### Put on a helmet

Once you have `psake` setup you can execute the below task one time. It will install a git pre-commit hook that will build and run the tests before your code is committed.

    psake putOnAHelmet

#### Build

`Psake` will pick the `default` task within the build file `default.ps1` an execute it.

    psake


