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
