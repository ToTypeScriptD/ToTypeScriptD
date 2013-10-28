properties {
    $cleanMessage = 'Executed Clean!'
    $msbuildConfiguration = "Debug"
    $msbuildPlatform = "Any CPU"
}

task default -depends Test

task Test -depends Compile, Clean { 
    & (ls ".\packages\xunit.runners*\tools\xunit.console.clr4.exe") ".\ToTypeScriptD.Tests\bin\$msbuildConfiguration\ToTypeScriptD.Tests.dll"
}

task Compile -depends Clean { 
    msbuild ToTypeScriptD.sln /p:Platform="$msbuildPlatform" /p:Configuration=$msbuildConfiguration /verbosity:quiet /nologo
}

task Clean { 
    # TODO:
}

task ? -Description "Helper to display task info" {
    Write-Documentation
}



task Create-VersionInfo {

    $versionInfoFile = 'SharedItems/VersionInfo.cs'
    New-Item -ItemType file $versionInfoFile -Force | out-null

    $commit = Get-Git-Commit

    $asmInfo = "
[assembly: System.Reflection.AssemblyInformationalVersion(""$commit"")]
"
    Write-Output $asmInfo > $versionInfoFile
}

function Get-Git-Commit
{
    if(where.exe git) {
        return (git rev-parse --short HEAD)
    } 
    else {
        return "0000000"
    }
}
