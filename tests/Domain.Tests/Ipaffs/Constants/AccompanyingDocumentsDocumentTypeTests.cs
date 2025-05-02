using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;
using FluentAssertions;

namespace Defra.TradeImportsDataApi.Domain.Tests.Ipaffs.Constants;

public class AccompanyingDocumentsDocumentTypeTests
{
    [Theory]
    [InlineData("airWaybill", true)]
    [InlineData("airwaybill", true)]
    [InlineData(null, false)]
    public void WhenAirWaybill_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsAirWaybill(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("billOfLading", true)]
    [InlineData("billoflading", true)]
    [InlineData(null, false)]
    public void WhenBillOfLading_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsBillOfLading(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("cargoManifest", true)]
    [InlineData("cargomanifest", true)]
    [InlineData(null, false)]
    public void WhenCargoManifest_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsCargoManifest(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("catchCertificate", true)]
    [InlineData("catchcertificate", true)]
    [InlineData(null, false)]
    public void WhenCatchCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsCatchCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("commercialDocument", true)]
    [InlineData("commercialdocument", true)]
    [InlineData(null, false)]
    public void WhenCommercialDocument_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsCommercialDocument(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("commercialInvoice", true)]
    [InlineData("commercialinvoice", true)]
    [InlineData(null, false)]
    public void WhenCommercialInvoice_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsCommercialInvoice(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("conformityCertificate", true)]
    [InlineData("conformitycertificate", true)]
    [InlineData(null, false)]
    public void WhenConformityCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsConformityCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("containerManifest", true)]
    [InlineData("containermanifest", true)]
    [InlineData(null, false)]
    public void WhenContainerManifest_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsContainerManifest(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("customsDeclaration", true)]
    [InlineData("customsdeclaration", true)]
    [InlineData(null, false)]
    public void WhenCustomsDeclaration_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsCustomsDeclaration(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("docom", true)]
    [InlineData("DOCOM", true)]
    [InlineData(null, false)]
    public void WhenDocom_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsDocom(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("healthCertificate", true)]
    [InlineData("healthcertificate", true)]
    [InlineData(null, false)]
    public void WhenHealthCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsHealthCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("heatTreatmentCertificate", true)]
    [InlineData("heattreatmentcertificate", true)]
    [InlineData(null, false)]
    public void WhenHeatTreatmentCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsHeatTreatmentCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("importPermit", true)]
    [InlineData("importpermit", true)]
    [InlineData(null, false)]
    public void WhenImportPermit_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsImportPermit(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("inspectionCertificate", true)]
    [InlineData("inspectioncertificate", true)]
    [InlineData(null, false)]
    public void WhenInspectionCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsInspectionCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("itahc", true)]
    [InlineData("ITAHC", true)]
    [InlineData(null, false)]
    public void WhenItahc_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsItahc(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("journeyLog", true)]
    [InlineData("journeylog", true)]
    [InlineData(null, false)]
    public void WhenJourneyLog_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsJourneyLog(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("laboratorySamplingResultsForAflatoxin", true)]
    [InlineData("laboratorysamplingresultsforaflatoxin", true)]
    [InlineData(null, false)]
    public void WhenLaboratorySamplingResultsForAflatoxin_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsLaboratorySamplingResultsForAflatoxin(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("latestVeterinaryHealthCertificate", true)]
    [InlineData("latestveterinaryhealthcertificate", true)]
    [InlineData(null, false)]
    public void WhenLatestVeterinaryHealthCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsLatestVeterinaryHealthCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("letterOfAuthority", true)]
    [InlineData("letterofauthority", true)]
    [InlineData(null, false)]
    public void WhenLetterOfAuthority_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsLetterOfAuthority(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("licenseOrAuthorisation", true)]
    [InlineData("licenseorauthorisation", true)]
    [InlineData(null, false)]
    public void WhenLicenseOrAuthorisation_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsLicenseOrAuthorisation(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("mycotoxinCertification", true)]
    [InlineData("mycotoxincertification", true)]
    [InlineData(null, false)]
    public void WhenMycotoxinCertification_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsMycotoxinCertification(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("originCertificate", true)]
    [InlineData("origincertificate", true)]
    [InlineData(null, false)]
    public void WhenOriginCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsOriginCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("other", true)]
    [InlineData("OTHER", true)]
    [InlineData(null, false)]
    public void WhenOther_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsOther(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("phytosanitaryCertificate", true)]
    [InlineData("phytosanitarycertificate", true)]
    [InlineData(null, false)]
    public void WhenPhytosanitaryCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsPhytosanitaryCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("processingStatement", true)]
    [InlineData("processingstatement", true)]
    [InlineData(null, false)]
    public void WhenProcessingStatement_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsProcessingStatement(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("proofOfStorage", true)]
    [InlineData("proofofstorage", true)]
    [InlineData(null, false)]
    public void WhenProofOfStorage_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsProofOfStorage(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("railwayBill", true)]
    [InlineData("railwaybill", true)]
    [InlineData(null, false)]
    public void WhenRailwayBill_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsRailwayBill(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("seaWaybill", true)]
    [InlineData("seawaybill", true)]
    [InlineData(null, false)]
    public void WhenSeaWaybill_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsSeaWaybill(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("veterinaryHealthCertificate", true)]
    [InlineData("veterinaryhealthcertificate", true)]
    [InlineData(null, false)]
    public void WhenVeterinaryHealthCertificate_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsVeterinaryHealthCertificate(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("listOfIngredients", true)]
    [InlineData("listofingredients", true)]
    [InlineData(null, false)]
    public void WhenListOfIngredients_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsListOfIngredients(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("packingList", true)]
    [InlineData("packinglist", true)]
    [InlineData(null, false)]
    public void WhenPackingList_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsPackingList(status).Should().Be(expected);
    }

    [Theory]
    [InlineData("roadConsignmentNote", true)]
    [InlineData("roadconsignmentnote", true)]
    [InlineData(null, false)]
    public void WhenRoadConsignmentNote_ThenMatch(string? status, bool expected)
    {
        AccompanyingDocumentsDocumentType.IsRoadConsignmentNote(status).Should().Be(expected);
    }
}
