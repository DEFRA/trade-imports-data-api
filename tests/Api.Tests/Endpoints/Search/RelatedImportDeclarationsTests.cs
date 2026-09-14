using System.Net;
using Defra.TradeImportsDataApi.Api.Endpoints.RelatedImportDeclarations;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using Defra.TradeImportsDataApi.Domain.Traces;
using Defra.TradeImportsDataApi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Trade.Gateway.Api.Contract.Certificate;
using WireMock.Server;
using Xunit.Abstractions;

namespace Defra.TradeImportsDataApi.Api.Tests.Endpoints.Search;

public class RelatedImportDeclarationsTests : EndpointTestBase, IClassFixture<WireMockContext>
{
    private IRelatedImportDeclarationsService MockSearchService { get; } =
        Substitute.For<IRelatedImportDeclarationsService>();
    private WireMockServer WireMock { get; }
    private readonly VerifySettings _settings;

    public RelatedImportDeclarationsTests(
        ApiWebApplicationFactory factory,
        ITestOutputHelper outputHelper,
        WireMockContext context
    )
        : base(factory, outputHelper)
    {
        WireMock = context.Server;
        WireMock.Reset();

        _settings = new VerifySettings();
        _settings.ScrubMember("traceId");
        _settings.DontScrubDateTimes();
        _settings.DontScrubGuids();
        _settings.DontIgnoreEmptyCollections();
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);

        services.AddTransient<IRelatedImportDeclarationsService>(_ => MockSearchService);
    }

    [Fact]
    public async Task Search_WhenFound_ShouldReturnContent()
    {
        var client = CreateClient();
        MockSearchService
            .Search(Arg.Any<RelatedImportDeclarationsRequest>(), Arg.Any<CancellationToken>())
            .Returns(
                new ValueTuple<
                    CustomsDeclarationEntity[],
                    ImportPreNotificationEntity[],
                    GmrEntity[],
                    ImportPreNotificationEntity[],
                    TracesChedEntity[],
                    ChedReservationEntity[]
                >(
                    [
                        new CustomsDeclarationEntity
                        {
                            Id = "Mrn1",
                            ClearanceRequest = new ClearanceRequest(),
                            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                            ETag = "etag",
                        },
                    ],
                    [
                        new ImportPreNotificationEntity
                        {
                            Id = "ChedId",
                            CustomsDeclarationIdentifier = "ChedId",
                            ImportPreNotification = new ImportPreNotification(),
                            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                            ETag = "etag",
                        },
                    ],
                    [
                        new GmrEntity
                        {
                            Id = "GmrId",
                            Gmr = new Gmr(),
                            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                            ETag = "etag",
                        },
                    ],
                    [],
                    [
                        new TracesChedEntity
                        {
                            Id = "ChedId",
                            Ched = new DefraUNVTDCHEDProfile()
                            {
                                ExchangedDocument = new ExchangedDocument() { Identifier = "ChedId" },
                                SpecifiedConsignment = new Consignment(),
                            },
                            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                            ETag = "etag",
                        },
                    ],
                    [
                        new ChedReservationEntity
                        {
                            Id = "ChedId_Mrn1",
                            Reservation = new Reservation
                            {
                                ChedId = "ChedId",
                                Mrn = "Mrn1",
                                Status = "Reserved",
                                Timestamp = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                                Commodities =
                                [
                                    new ReservationCommodity
                                    {
                                        CertificateLineNumber = 1,
                                        GoodsItemNumber = 1,
                                        CommodityCode = "123",
                                        UnitOfMeasure = "KGM",
                                        Quantity = 100,
                                    },
                                ],
                            },
                            Created = new DateTime(2025, 4, 3, 10, 0, 0, DateTimeKind.Utc),
                            Updated = new DateTime(2025, 4, 3, 10, 15, 0, DateTimeKind.Utc),
                            ETag = "etag",
                        },
                    ]
                )
            );

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.RelatedImportDeclarations.SearchByMrn("Mrn1")
        );

        await VerifyJson(await response.Content.ReadAsStringAsync(), _settings)
            .UseMethodName(nameof(Search_WhenFound_ShouldReturnContent));
        await Verify(response, _settings).UseMethodName($"{nameof(Search_WhenFound_ShouldReturnContent)}_response");
    }

    [Fact]
    public async Task Get_WhenUnauthorized_ShouldBeUnauthorized()
    {
        var client = CreateClient(addDefaultAuthorizationHeader: false);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.RelatedImportDeclarations.SearchByMrn("mrn")
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_WhenWriteOnly_ShouldBeForbidden()
    {
        var client = CreateClient(testUser: TestUser.WriteOnly);

        var response = await client.GetAsync(
            TradeImportsDataApi.Testing.Endpoints.RelatedImportDeclarations.SearchByMrn("mrn")
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
