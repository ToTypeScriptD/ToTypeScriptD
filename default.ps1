properties {
  $cleanMessage = 'Executed Clean!'
  $msbuildConfiguration = "Debug"
  $msbuildPlatform = "Any CPU"
}

task default -depends Test

task Test -depends Compile, Clean { 
  # TODO:
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
