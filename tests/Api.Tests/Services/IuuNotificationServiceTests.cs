using Defra.TradeImportsDataApi.Api.Data;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Defra.TradeImportsDataApi.Domain.Iuu;
using FluentAssertions;
using NSubstitute;

namespace Defra.TradeImportsDataApi.Api.Tests.Services;

public class IuuNotificationServiceTests
{
    private IDbContext DbContext { get; }
    private IIuuNotificationRepository IuuNotificationRepository { get; }
    private IuuNotificationService Subject { get; }

    public IuuNotificationServiceTests()
    {
        DbContext = Substitute.For<IDbContext>();
        IuuNotificationRepository = Substitute.For<IIuuNotificationRepository>();

        Subject = new IuuNotificationService(DbContext, IuuNotificationRepository);
    }

    [Fact]
    public async Task Insert_ShouldInsert()
    {
        var entity = new IuuNotificationEntity
        {
            Id = "id",
            IuuNotification = new IuuNotification
            {
                Reference = "IUU.Reference",
                LastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
                IuuOption = "IUUOK",
            },
        };
        IuuNotificationRepository.Insert(entity).Returns(entity);

        await Subject.Insert(entity, CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        IuuNotificationRepository.Received().Insert(entity);
    }

    [Fact]
    public async Task Update_ShouldUpdate()
    {
        const string iuuReference = "IUU.Reference";
        var existing = new IuuNotificationEntity
        {
            Id = iuuReference,
            IuuNotification = new IuuNotification
            {
                Reference = "IUU.Reference",
                LastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
                IuuOption = "IUUOK",
            },
        };
        var entity = new IuuNotificationEntity
        {
            Id = iuuReference,
            IuuNotification = new IuuNotification
            {
                Reference = "IUU.Reference",
                LastUpdated = new DateTime(2026, 10, 16, 9, 15, 0, DateTimeKind.Utc),
                IuuOption = "IUUNA",
            },
        };
        IuuNotificationRepository.Update(entity, "etag", CancellationToken.None).Returns((existing, entity));

        await Subject.Update(entity, "etag", CancellationToken.None);

        await DbContext.Received(1).StartTransaction(CancellationToken.None);
        await DbContext.Received(1).SaveChanges(CancellationToken.None);
        await DbContext.Received(1).CommitTransaction(CancellationToken.None);

        await IuuNotificationRepository.Received().Update(entity, "etag", CancellationToken.None);
    }

    [Fact]
    public async Task GetIuuNotification_ShouldGet()
    {
        const string iuuReference = "IUU.Reference";
        IuuNotificationRepository
            .Get(iuuReference, CancellationToken.None)
            .Returns(
                new IuuNotificationEntity
                {
                    Id = iuuReference,
                    IuuNotification = new IuuNotification
                    {
                        Reference = "IUU.Reference",
                        LastUpdated = new DateTime(2026, 10, 16, 9, 0, 0, DateTimeKind.Utc),
                        IuuOption = "IUUOK",
                    },
                }
            );

        var result = await Subject.Get(iuuReference, CancellationToken.None);

        result.Should().NotBeNull();

        await IuuNotificationRepository.Received().Get(iuuReference, CancellationToken.None);
    }
}
