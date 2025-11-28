dependencies:
	dotnet tool restore

generate-openapi-spec: dependencies
	dotnet build src/Api/Api.csproj -c Release -o publish
	cp ./src/Api/bin/Release/net9.0/appsettings.json .
	dotnet swagger tofile --output openapi.json ./src/Api/bin/Release/net9.0/Defra.TradeImportsDataApi.Api.dll v1
	rm appsettings.json

lint-openapi-spec: generate-openapi-spec
	docker run --rm -v "$(PWD):/work:ro" dshanley/vacuum lint -d -r .vacuum.yml openapi.json

lint-openapi-spec-errors: generate-openapi-spec
	docker run --rm -v "$(PWD):/work:ro" dshanley/vacuum lint -d -e -r .vacuum.yml openapi.json
