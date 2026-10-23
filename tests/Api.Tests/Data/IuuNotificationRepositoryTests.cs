using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Iuu;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Data;

public class IuuNotificationRepositoryTests
{
    private IDbContext DbContext { get; }
    private IuuNotificationRepository Subject { get; }

    public IuuNotificationRepositoryTests()
    {
        DbContext = Substitute.For<IDbContext>();

        Subject = new IuuNotificationRepository(DbContext);
    }

    [Fact]
    public async Task Update_WhenNotExists_ShouldThrow()
    {
        var entity = new IuuNotificationEntity()
        {
            Id = "IUU.Reference",
            IuuNotification = new IuuNotification
            {
                Reference = "IUU.Reference",
                LastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
                IuuOption = "IUUOK",
            },
        };

        var act = async () => await Subject.Update(entity, "etag", CancellationToken.None);

        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}
