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
    public bool OnGround { get; init; }
}

public record OpenSkyStatesResponse
{
    public long Time { get; init; }
    public List<List<JsonElement>>? States { get; init; }
}