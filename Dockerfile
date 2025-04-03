# Base dotnet image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Add curl to template.
# CDP PLATFORM HEALTHCHECK REQUIREMENT
RUN apt update && \
    apt upgrade -y && \
    apt install curl -y && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG VACUUM_VERSION=0.14.2
WORKDIR /tmp/vacuum
RUN wget --secure-protocol=TLSv1_2 "https://github.com/daveshanley/vacuum/releases/download/v${VACUUM_VERSION}/vacuum_${VACUUM_VERSION}_linux_x86_64.tar.gz" -q -O vacuum.tar.gz && \
    tar zxvf "vacuum.tar.gz" && \
    mv vacuum /usr/bin/vacuum

WORKDIR /src

ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install -g --allow-roll-forward csharpier

COPY .config/dotnet-tools.json .config/dotnet-tools.json
COPY .csharpierrc .csharpierrc
COPY .vacuum.yml .vacuum.yml

RUN dotnet tool restore

COPY src/Api/Api.csproj src/Api/Api.csproj
COPY src/Domain/Domain.csproj src/Domain/Domain.csproj
COPY src/Data/Data.csproj src/Data/Data.csproj
COPY tests/Testing/Testing.csproj tests/Testing/Testing.csproj
COPY tests/Api.Tests/Api.Tests.csproj tests/Api.Tests/Api.Tests.csproj
COPY tests/Api.IntegrationTests/Api.IntegrationTests.csproj tests/Api.IntegrationTests/Api.IntegrationTests.csproj
COPY Defra.TradeImportsData.Api.sln Defra.TradeImportsData.Api.sln
COPY Directory.Build.props Directory.Build.props

RUN dotnet restore

COPY src/Api src/Api
COPY src/Domain src/Domain
COPY src/Data src/Data
COPY tests/Testing tests/Testing
COPY tests/Api.Tests tests/Api.Tests
COPY tests/Api.IntegrationTests tests/Api.IntegrationTests

RUN dotnet csharpier --check .

RUN dotnet build src/Api/Api.csproj --no-restore -c Release
RUN dotnet swagger tofile --output openapi.json ./src/Api/bin/Release/net9.0/Defra.TradeImportsData.Api.dll v1
#RUN vacuum lint -d -r .vacuum.yml openapi.json

RUN dotnet test --no-restore tests/Api.Tests
RUN dotnet test --no-restore tests/Api.IntegrationTests

FROM build AS publish

RUN dotnet publish src/Api -c Release -o /app/publish /p:UseAppHost=false

ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8085
ENTRYPOINT ["dotnet", "Defra.TradeImportsData.Api.dll"]
