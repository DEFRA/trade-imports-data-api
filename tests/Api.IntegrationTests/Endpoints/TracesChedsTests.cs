using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Trade.Gateway.Api.Contract.Certificate;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class TracesChedsTests(ITestOutputHelper testOutputHelper) : SqsTestBase(testOutputHelper)
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var body = new DefraUNVTDCHEDProfile()
        {
            ExchangedDocument = new ExchangedDocument() { Identifier = "chedId" },
            SpecifiedConsignment = new Consignment(),
            Model = "Test",
        };
        var chedRef = ImportPreNotificationIdGenerator.Generate();
        var client = CreateDataApiClient();
        var httpClient = CreateHttpClient();

        var result = await client.GetTracesChed(chedRef, CancellationToken.None);
        result.Should().BeNull();

        await client.PutTracesChed(chedRef, body, null, CancellationToken.None);

        result = await client.GetTracesChed(chedRef, CancellationToken.None);
        result.Should().NotBeNull();

        var allResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.GetAll(chedRef)
        );
        allResourceEvents.Length.Should().Be(1);
        var unpublishedResourceEvents = await httpClient.GetFromJsonAsyncSafe<object[]>(
            Testing.Endpoints.ResourceEvents.Unpublished(chedRef)
        );
        unpublishedResourceEvents.Length.Should().Be(0);
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        var result = await client.GetTracesChed(chedRef, CancellationToken.None);
        result.Should().BeNull();

        await client.PutTracesChed(
            chedRef,
            new DefraUNVTDCHEDProfile()
            {
                ExchangedDocument = new ExchangedDocument() { Identifier = chedRef },
                SpecifiedConsignment = new Consignment(),
                Model = "Test",
            },
            null,
            CancellationToken.None
        );

        result = await client.GetTracesChed(chedRef, CancellationToken.None);
        result.Should().NotBeNull();
        result.Ched.Model.Should().Be("Test");
        result.Created.Should().BeAfter(DateTime.MinValue);
        result.Updated.Should().BeAfter(DateTime.MinValue);

        await client.PutTracesChed(
            chedRef,
            new DefraUNVTDCHEDProfile()
            {
                ExchangedDocument = new ExchangedDocument() { Identifier = chedRef },
                SpecifiedConsignment = new Consignment(),
                Model = "Test1",
            },
            result.ETag,
            CancellationToken.None
        );

        var result2 = await client.GetTracesChed(chedRef, CancellationToken.None);
        result2.Should().NotBeNull();
        result2.Ched.Model.Should().Be("Test1");
        result2.Created.Should().Be(result.Created);
        result2.Updated.Should().BeAfter(result.Updated);
    }

    [Fact]
    public async Task WhenRelatedCustomsDeclarationsDoNotExist_ShouldReturnAnEmptyList()
    {
        var client = CreateDataApiClient();
        var chedRef = ImportPreNotificationIdGenerator.Generate();

        var result = await client.GetTracesChed(chedRef, CancellationToken.None);
        result.Should().BeNull();

        await client.PutTracesChed(
            chedRef,
            new DefraUNVTDCHEDProfile()
            {
                ExchangedDocument = new ExchangedDocument() { Identifier = chedRef },
                SpecifiedConsignment = new Consignment(),
            },
            null,
            CancellationToken.None
        );

        var cdResult = await client.GetCustomsDeclarationsByTracesChedId(chedRef, CancellationToken.None);
        cdResult.Should().NotBeNull();
        cdResult.CustomsDeclarations.Count.Should().Be(0);
    }

    [Fact]
    public async Task WhenRelatedCustomsDeclarationsExists_ShouldRead()
    {
        var client = CreateDataApiClient();
        var chedRef = "CHEDA.GB.2025.1234567";
        var mrn = "testmrn123";

        var result = await client.GetTracesChed(chedRef, CancellationToken.None);

        if (result is null)
        {
            await client.PutTracesChed(
                chedRef,
                new DefraUNVTDCHEDProfile()
                {
                    ExchangedDocument = new ExchangedDocument() { Identifier = chedRef },
                    SpecifiedConsignment = new Consignment(),
                },
                null,
                CancellationToken.None
            );
        }

        var cdResult = await client.GetCustomsDeclaration(mrn, CancellationToken.None);

        if (cdResult is null)
        {
            await client.PutCustomsDeclaration(
                mrn,
                new CustomsDeclaration
                {
                    ClearanceRequest = new ClearanceRequest
                    {
                        ExternalVersion = 1,
                        Commodities =
                        [
                            new Commodity
                            {
                                Documents =
                                [
                                    new ImportDocument
                                    {
                                        DocumentReference = new ImportDocumentReference(chedRef),
                                        DocumentCode = "C640",
                                    },
                                ],
                            },
                        ],
                    },
                },
                null,
                CancellationToken.None
            );
        }

        var actualResult = await client.GetCustomsDeclarationsByTracesChedId(chedRef, CancellationToken.None);
        actualResult.Should().NotBeNull();
        actualResult.CustomsDeclarations.Count.Should().Be(1);
    }
}
