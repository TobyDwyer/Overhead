using Overhead.Api.Models;
using Overhead.Api.Services;

namespace Overhead.Api.Endpoints;

public static class OpenSkyEndpoints
{
    public static void MapOpenSkyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/planes");

        group.MapGet("/{icao24}", async (string icao24, OpenSkyClient client, CancellationToken ct) =>
        {
            var plane = await client.GetPlaneAsync(icao24, ct);
            return plane;
        });

        group.MapGet("/", async ([AsParameters] OpenSkyAreaRequest request, OpenSkyClient client, CancellationToken ct) =>
        {
            var planes = await client.GetPlanesInArea(request, ct);
            return TypedResults.Ok(planes);
        });
    }
}