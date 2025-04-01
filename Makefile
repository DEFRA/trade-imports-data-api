dependencies:
	dotnet tool restore

generate-openapi-spec: dependencies
	dotnet build src/Api/Api.csproj --no-restore -c Release
	dotnet swagger tofile --output openapi.json ./src/Api/bin/Release/net9.0/Defra.TradeImportsData.Api.dll v1

lint-openapi-spec: generate-openapi-spec
	docker run --rm -v "$(PWD):/work:ro" dshanley/vacuum lint -d -r .vacuum.yml openapi.json

lint-openapi-spec-errors: generate-openapi-spec
	docker run --rm -v "$(PWD):/work:ro" dshanley/vacuum lint -d -e -r .vacuum.yml openapi.json
