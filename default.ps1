properties {
  $cleanMessage = 'Executed Clean!'
}

task default -depends Test

task Test -depends Compile, Clean { 
  $testMessage
}

task Compile -depends Clean { 
  msbuild ToTypeScriptD.sln /p:Configuration=Debug /p:Platform="Any CPU"
}

task Clean { 
  $cleanMessage
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}
