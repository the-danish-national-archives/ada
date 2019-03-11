param([String]$buildNumber = '1')


$fileName = '*/AssemblySharedInfo.cs'
$regexA = '(AssemblyVersion|AssemblyFileVersionAttribute|AssemblyFileVersion)\("([0-9]+\.[0-9]+\.[0-9]+)(\.[0-9]+)"\)'
$regexReplace = '$1("$2.' + $buildNumber + '")'

Write-Host ('$regexReplace is ' + $regexReplace)

Get-ChildItem  "AssemblySharedInfo.cs" | % {
  $c = (Get-Content $_.FullName) -replace $regexA,$regexReplace -join "`r`n"
  [IO.File]::WriteAllText($_.FullName, $c)
}
