using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Overhead.Api.Models;

public record OpenSkyState
{
    public required string Icao24 { get; init; }
    public string? Callsign { get; init; }
    public required string OriginCountry { get; init; }
    public double? Longitude { get; init; }
    public double? Latitude { get; init; }
    public double? BaroAltitude { get; init; }
    public double? Heading { get; init; }
    public bool OnGround { get; init; }
}

public record OpenSkyStatesResponse
{
    public long Time { get; init; }
    public List<List<JsonElement>>? States { get; init; }
}

public record OpenSkyAreaRequest(
    double Area,
    [property: Range(-90, 90)] double Lat,
    [property: Range(-180, 180)] double Long
);

public record AreaBox(double MinLat, double MinLong, double MaxLat, double MaxLong);