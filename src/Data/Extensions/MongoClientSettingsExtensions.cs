using MongoDB.Driver;

namespace Defra.TradeImportsDataApi.Data.Extensions;

public static class MongoClientSettingsExtensions
{
    /// <summary>
    /// Configures the MongoDB client for read-your-writes consistency by:
    /// - Reading from the primary (ReadPreference.Primary).
    /// - Acknowledging writes only after a majority of replica set members have replicated the write,
    ///   optionally waiting for the journal flush on those members (WriteConcern.WMajority with journaling).
    /// - Reading only majority-committed data (ReadConcern.Majority).
    ///
    /// This combination ensures that immediately after a write, later reads from this client
    /// observe the majority-committed state on the primary, avoiding stale reads from secondaries.
    /// </summary>
    /// <param name="settings">The MongoClientSettings to configure.</param>
    /// <param name="journal">
    /// When true, requires journaled durability alongside majority replication.
    /// In modern MongoDB deployments, majority writes are journaled by default; this makes it explicit.
    /// </param>
    /// <returns>The configured MongoClientSettings.</returns>
    public static MongoClientSettings UsePrimaryMajorityConsistency(
        this MongoClientSettings settings,
        bool journal = true
    )
    {
        settings.ReadPreference = ReadPreference.Primary;
        settings.WriteConcern = WriteConcern.WMajority.With(journal: journal);
        settings.ReadConcern = ReadConcern.Majority;

        return settings;
    }
}
