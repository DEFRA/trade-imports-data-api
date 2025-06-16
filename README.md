# Trade Imports API

The Trade Imports API is a .NET API application which manages the BTMS data and publishes events when data changes.

* [Prerequisites](#prerequisites) 
* [Setup Process](#setup-process)
* [How to run in development](#how-to-run-in-development)
* [How to run Tests](#how-to-run-tests)
* [Running](#running)
* [Deploying](#deploying)
* [SonarCloud](#sonarCloud)
* [Dependabot](#dependabot)
* [Tracing](#tracing)
* [Licence Information](#licence-information)
* [About the Licence](#about-the-licence)

### Prerequisites

- .NET 9 SDK
- Docker
  - localstack - used for local queuing
  - wiremock - used for mocking out http requests 
  - mongodb - used for data storage
  

### Setup Process

- Install the .NET 9 SDK
- Install Docker
  - Run the following Docker Compose to set up locally running queues for testing
  ```bash
  docker compose -f compose.yml up -d
  ```

### How to run in development

Run the application with the command:

```bash
dotnet run --project /src/Api/Api.csproj
```

### How to run Tests

Run the unit tests with:

```bash
dotnet test --filter "Category!=IntegrationTest"
```
Run the integration tests with:
```bash
dotnet test --filter "Category=IntegrationTest"
```
Run all tests with:
```bash
dotnet test 
```

#### Unit Tests
Some unit tests may run an in memory instance service. 

Unit tests that need to edit the value of an application setting can be done via the`appsettings.IntegrationTests.json`

#### Integration Tests
Integration tests run against the built docker image.

Because these use the built docker image, the `appsettings.json` will be used, should any values need to be overridden, then they can be injected as an environment variable via the`compose.yml`

### Deploying

Before deploying via CDP set the correct config for the environment as per the `appsettings.Development.json`.

### SonarCloud

Example SonarCloud configuration are available in the GitHub Action workflows.

### Dependabot

We are using dependabot

### Tracing
The out of the box CDP template doesn't provide any example of how to handle tracing for non Http communication.
This service will add the trace id to any messages that it publishes.  


### Licence Information

THIS INFORMATION IS LICENSED UNDER THE CONDITIONS OF THE OPEN GOVERNMENT LICENCE found at:

<http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3>

### About the licence

The Open Government Licence (OGL) was developed by the Controller of Her Majesty's Stationery Office (HMSO) to enable information providers in the public sector to license the use and re-use of their information under a common open licence.

It is designed to encourage use and re-use of information freely and flexibly, with only a few conditions.


