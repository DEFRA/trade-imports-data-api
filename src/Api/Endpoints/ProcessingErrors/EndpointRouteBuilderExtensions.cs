using Defra.TradeImportsDataApi.Api.Authentication;
using Defra.TradeImportsDataApi.Api.Exceptions;
using Defra.TradeImportsDataApi.Api.Extensions;
using Defra.TradeImportsDataApi.Api.Services;
using Defra.TradeImportsDataApi.Api.Utils;
using Defra.TradeImportsDataApi.Data;
using Defra.TradeImportsDataApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Defra.TradeImportsDataApi.Api.Endpoints.ProcessingErrors;

public static class EndpointRouteBuilderExtensions
{
    public static void MapProcessingErrorEndpoints(this IEndpointRouteBuilder app)
    {
        const string groupName = "ProcessingErrors";

        app.MapGet("processing-errors/{mrn}/", Get)
            .WithName("ProcessingErrorByMrn")
            .WithTags(groupName)
            .WithSummary("Get ProcessingError")
            .WithDescription("Get a Processing Error by MRN")
            .Produces<ProcessingErrorResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Read);

        app.MapPut("processing-errors/{mrn}/", Put)
            .WithName("PutProcessingError")
            .WithTags(groupName)
            .WithSummary("Put ProcessingError")
            .WithDescription("Put a Processing Error")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(PolicyNames.Write);
    }

    /// <param name="mrn">MRN</param>
    /// <param name="context"></param>
    /// <param name="processingErrorService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    private static async Task<IResult> Get(
        [FromRoute] string mrn,
        HttpContext context,
        [FromServices] IProcessingErrorService processingErrorService,
        CancellationToken cancellationToken
    )
    {
        var processingErrorEntity = await processingErrorService.GetProcessingError(mrn, cancellationToken);
        if (processingErrorEntity is null)
        {
            return Results.NotFound();
        }

        context.SetResponseEtag(processingErrorEntity.ETag);

        return Results.Ok(
            new ProcessingErrorResponse(
                processingErrorEntity.Id,
                processingErrorEntity.ProcessingErrors,
                processingErrorEntity.Created,
                processingErrorEntity.Updated
            )
        );
    }

    [HttpPut]
    private static async Task<IResult> Put(
        [FromRoute] string mrn,
        HttpContext context,
        [FromBody] Domain.Errors.ProcessingError[] data,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        [FromServices] IProcessingErrorService processingErrorService,
        CancellationToken cancellationToken
    )
    {
        var processingErrorEntity = new ProcessingErrorEntity { Id = mrn, ProcessingErrors = data };

        var etag = ETags.ValidateAndParseFirst(ifMatch);

        try
        {
            if (string.IsNullOrEmpty(etag))
            {
                await processingErrorService.Insert(processingErrorEntity, cancellationToken);

                return Results.Created();
            }

            await processingErrorService.Update(processingErrorEntity, etag, cancellationToken);

            return Results.NoContent();
        }
        catch (ConcurrencyException)
        {
            return Results.Conflict();
        }
        catch (EntityNotFoundException)
        {
            return Results.NotFound();
        }
    }
}
