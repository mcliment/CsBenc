#!/usr/bin/env bash
set -euox pipefail

cd "$(dirname "${BASH_SOURCE[0]}")"

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

dotnet tool restore

dotnet build -c Release
dotnet test -c Release --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
dotnet reportgenerator "-reports:./test/**/coverage.cobertura.xml" "-targetdir:./coverage" "-reportTypes:lcov"
dotnet csmacnz.Coveralls --lcov -i ./coverage/lcov.info