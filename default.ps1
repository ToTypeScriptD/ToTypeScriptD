properties {
    $msbuildConfiguration = "Debug"
    $msbuildPlatform = "Any CPU"
    $buildFolder = "bin"
    $packageFolder = "$buildFolder\ToTypeScriptD"
}

task default -depends Test

task Test -depends Compile { 
    & (ls ".\packages\xunit.runners*\tools\xunit.console.clr4.exe") ".\ToTypeScriptD.Tests\bin\$msbuildConfiguration\ToTypeScriptD.Tests.dll"
}

task Compile -depends Clean, Create-VersionInfo { 

    # the p:/VisualStudioEdition=v11.0 property is just to remove a warning when compiling the javascript tests project
    # http://www.interact-sw.co.uk/iangblog/2013/07/29/fix-appx2102

    msbuild ToTypeScriptD.sln /p:Platform="$msbuildPlatform" /p:Configuration=$msbuildConfiguration /verbosity:quiet /nologo /p:VisualStudioEdition=v11.0
}

task Package -depends Test, Compile {

    # 1. Get Assembly Version #
    # 2. Zip Release Files


    $version = get-formatted-assembly-version "$buildFolder\ToTypeScriptD.exe"

    mkdir $packageFolder -Force
    copy "$buildFolder\CommandLine.dll"          $packageFolder -Force
    copy "$buildFolder\CommandLine.xml"          $packageFolder -Force
    copy "$buildFolder\Mono.Cecil.dll"           $packageFolder -Force
    copy "$buildFolder\ToTypeScriptD.Core.dll"   $packageFolder -Force
    copy "$buildFolder\ToTypeScriptD.Core.pdb"   $packageFolder -Force
    copy "$buildFolder\ToTypeScriptD.exe"        $packageFolder -Force
    copy "$buildFolder\ToTypeScriptD.exe.config" $packageFolder -Force
    copy "$buildFolder\ToTypeScriptD.pdb"        $packageFolder -Force

    #Zip-Folder "$buildFolder\ToTypeScriptD" ((pwd).Path + "\bin\ToTypeScriptD.$version.zip")

    $nuspecFile = "$buildFolder\ToTypeScriptD.nuspec";
    cp .\chocolatey\ToTypeScriptD.nuspec $nuspecFile

    [xml](cat $nuspecFile)
    $nuspec = [xml](cat $nuspecFile)
    $nuspec.package.metadata.version = $version
    $nuspec.Save((get-item $nuspecFile))

    chocolatey pack "$buildFolder\ToTypeScriptD.nuspec"
}

task Publish -depends Package {


    $choices = [System.Management.Automation.Host.ChoiceDescription[]](
        (New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", ""),
        (New-Object System.Management.Automation.Host.ChoiceDescription "&No", "")
    )

    $result = $Host.UI.PromptForChoice("","Are you sure you want to publish?", $choices, 0)

    if($result -eq 0) {
        ls *.nupkg | %{
            ""
            Write-Host -Foreground Green "Publishing... $_"
            ""
            chocolatey push $_
        }
    } else {
        ""
        Write-Host -Foreground Yellow "Skipped publishing..."
        ""
    }
}

task Clean { 

    rm *.nupkg
    rm -force -ErrorAction SilentlyContinue bin\*.zip

# For some reason the msbuild step above won't compile the native assembly? (we'll come back to that)
#    msbuild ToTypeScriptD.sln /target:clean /p:Platform="$msbuildPlatform" /p:Configuration=$msbuildConfiguration /verbosity:quiet /nologo
#
#    if(test-path $buildFolder) {
#        rm $buildFolder -Force -Recurse -ErrorAction SilentlyContinue
#    }
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


function get-assembly-version() {
        param([string] $file)
        
        $fileStream = ([System.IO.FileInfo] (Get-Item $file)).OpenRead()
        $assemblyBytes = new-object byte[] $fileStream.Length
        $fileStream.Read($assemblyBytes, 0, $fileStream.Length) | Out-Null #out null this because this function should only return the version & this call was outputting some garbage number
        $fileStream.Close()
        $version = [System.Reflection.Assembly]::Load($assemblyBytes).GetName().Version;
        
        #format the version and output it...
        $version
}

function get-formatted-assembly-version() {
        param([string] $file)
        
        $version = get-assembly-version $file
        "$($version.Major).$($version.Minor).$($version.Build).$($version.Revision)"
}


function Zip-Folder($inFolder, $outZipFile) {
    # copied and modified from
    # http://stackoverflow.com/questions/11021879/creating-a-zipped-compressed-folder-in-windows-using-powershell-or-the-command-l

    $zipArchive = $outZipFile
    [System.Reflection.Assembly]::Load("WindowsBase,Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35") | out-null
    $ZipPackage=[System.IO.Packaging.ZipPackage]::Open($zipArchive, [System.IO.FileMode]::CreateNew)
    $in = ls "$inFolder\*" -recurse -exclude $outZipFile | ? { !$_.PSIsContainer } | select -expand fullName
    [array]$files = $in -replace "C:","" -replace "\\","/"
    ForEach ($file In $files) {
        $uri = "/" + [System.IO.Path]::GetFileName($file);
        $partName=New-Object System.Uri($uri, [System.UriKind]"Relative")
        $part=$ZipPackage.CreatePart($partName, "application/zip", [System.IO.Packaging.CompressionOption]"Maximum")
        $bytes=[System.IO.File]::ReadAllBytes($file)
        $stream=$part.GetStream()
        $stream.Write($bytes, 0, $bytes.Length)
        $stream.Close()
    }
    $ZipPackage.Close()
}
