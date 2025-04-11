using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Api.IntegrationTests.Endpoints;

public class CustomsDeclarationTests : IntegrationTestBase
{
    [Fact]
    public async Task WhenDoesNotExist_ShouldCreateAndRead()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");

        var result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest() },
            null,
            CancellationToken.None
        );

        result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task WhenExists_ShouldUpdate()
    {
        var client = CreateDataApiClient();
        var mrn = Guid.NewGuid().ToString("N");

        var result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().BeNull();

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 1 } },
            null,
            CancellationToken.None
        );

        result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().NotBeNull();
        result.ClearanceRequest?.ExternalVersion.Should().Be(1);

        await client.PutCustomsDeclaration(
            mrn,
            new CustomsDeclaration { ClearanceRequest = new ClearanceRequest { ExternalVersion = 2 } },
            result.ETag,
            CancellationToken.None
        );

        result = await client.GetCustomsDeclaration(mrn, CancellationToken.None);
        result.Should().NotBeNull();
        result.ClearanceRequest?.ExternalVersion.Should().Be(2);
    }
}
