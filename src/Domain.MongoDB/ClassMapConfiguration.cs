using Defra.TradeImportsDataApi.Domain.Gvms;
using Defra.TradeImportsDataApi.Domain.Ipaffs;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Defra.TradeImportsDataApi.Domain.MongoDB;

public static class ClassMapConfiguration
{
    public static void Register()
    {
        RegisterGvms();
        RegisterIpaffs();
    }

    private static void RegisterIpaffs()
    {
        BsonClassMap.RegisterClassMap<Applicant>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.SampledOn)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });

        BsonClassMap.RegisterClassMap<JourneyRiskCategorisationResult>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.RiskLevelSetFor)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });

        BsonClassMap.RegisterClassMap<LaboratoryTests>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.TestedOn)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });

        BsonClassMap.RegisterClassMap<PartOne>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.ExitedPortOfOn)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
            cm.GetMemberMap(c => c.ArrivesAt)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
            cm.GetMemberMap(c => c.DepartedOn)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });

        BsonClassMap.RegisterClassMap<RiskAssessmentResult>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.AssessedOn)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });

        BsonClassMap.RegisterClassMap<SealCheck>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.CheckedOn)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });
    }

    private static void RegisterGvms()
    {
        BsonClassMap.RegisterClassMap<PlannedCrossing>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.DepartsAt)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });

        BsonClassMap.RegisterClassMap<ActualCrossing>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.ArrivesAt)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });

        BsonClassMap.RegisterClassMap<CheckedInCrossing>(cm =>
        {
            cm.AutoMap();
            cm.GetMemberMap(c => c.ArrivesAt)
                .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Unspecified)));
        });
    }
}
