using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.CustomsDeclarations;

public static class EndpointRouteBuilderExtensions
{
    public static void MapCustomsDeclarationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("customs-declarations/{mrn}/", Get)
            .WithName("CustomsDeclarationByMrn")
            .WithTags("CustomsDeclarations")
            .WithSummary("Get CustomsDeclaration")
            .WithDescription("Get a Customs Declaration by MRN")
            .Produces<CustomsDeclarationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPut("customs-declarations/{mrn}/", Put)
            .WithName("PutCustomsDeclaration")
            .WithTags("CustomsDeclarations")
            .WithSummary("Put CustomsDeclaration")
            .WithDescription("Put a Customs Declaration")
            .Produces<CustomsDeclarationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    /// <param name="mrn">MRN</param>
    /// <param name="context"></param>
    /// <param name="customsDeclarationService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string mrn,
        HttpContext context,
        [FromServices] ICustomsDeclarationService customsDeclarationService,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarationEntity = await customsDeclarationService.GetCustomsDeclaration(mrn, cancellationToken);
        if (customsDeclarationEntity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(customsDeclarationEntity.ETag);

        return Results.Ok(ToResponse(customsDeclarationEntity));
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string mrn,
        HttpContext context,
        [FromBody] Domain.CustomsDeclaration.CustomsDeclaration data,
        [FromHeader(Name = "If-Match")] string? etag,
        [FromServices] ICustomsDeclarationService customsDeclarationService,
        CancellationToken cancellationToken
    )
    {
        var customsDeclarationEntity = new CustomsDeclarationEntity { Id = mrn, Data = data };

        try
        {
            customsDeclarationEntity = string.IsNullOrEmpty(etag)
                ? await customsDeclarationService.Insert(customsDeclarationEntity, cancellationToken)
                : await customsDeclarationService.Update(customsDeclarationEntity, etag, cancellationToken);

            context.SetResponseEtag(customsDeclarationEntity.ETag);

            return Results.Ok(ToResponse(customsDeclarationEntity));
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
    }

    private static CustomsDeclarationResponse ToResponse(CustomsDeclarationEntity customsDeclarationEntity)
    {
        return new CustomsDeclarationResponse(
            customsDeclarationEntity.Data,
            customsDeclarationEntity.Created,
            customsDeclarationEntity.Updated
        );
    }
}
