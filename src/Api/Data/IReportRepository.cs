using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs;

namespace Defra.TradeImportsDataApi.Api.Data;

public interface IReportRepository
{
    void ClearanceRequest(string mrn);
    void ClearanceDecision(string mrn, ClearanceDecision? decision);
    void Finalisation(string mrn, Finalisation? finalisation);
    void ImportPreNotification(ImportPreNotification? importPreNotification);

    Task<IReadOnlyList<ReportClearanceDecision>> GetClearanceDecisions(
        DateTime day,
        CancellationToken cancellationToken
    );
}
