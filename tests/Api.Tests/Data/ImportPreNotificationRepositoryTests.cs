using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Data;

public class ImportPreNotificationRepositoryTests
{
    private IDbContext DbContext { get; }
    private ImportPreNotificationRepository Subject { get; }

    public ImportPreNotificationRepositoryTests()
    {
        DbContext = Substitute.For<IDbContext>();

        Subject = new ImportPreNotificationRepository(DbContext, NullLogger<ImportPreNotificationRepository>.Instance);
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var entity = new ImportPreNotificationEntity { Id = "id", ImportPreNotification = new ImportPreNotification() };

        var act = async () => await Subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}
