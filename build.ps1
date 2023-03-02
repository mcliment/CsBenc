$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

dotnet tool restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet build -c Release
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet test -c Release --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet reportgenerator "-reports:./test/**/coverage.cobertura.xml" "-targetdir:./coverage" "-reportTypes:lcov"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet csmacnz.Coveralls --lcov -i ./coverage/lcov.info
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }