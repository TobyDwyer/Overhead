using System.Text.Json;
using Overhead.Api.Models;

namespace Overhead.Api.Services;

public class OpenSkyClient(HttpClient http)
{
    public async Task<OpenSkyState?> GetPlaneAsync(string icao24, CancellationToken ct = default)
    {
        var response = await http.GetFromJsonAsync<OpenSkyStatesResponse>($"states/all?icao24={icao24}", ct);

        var row = response?.States?.FirstOrDefault();

        return row is null ? null : MapResults(row);
    }


    private static OpenSkyState MapResults(List<JsonElement> s) => new()
    {
        Icao24 = s[0].GetString()!,
        Callsign = s[1].GetString()?.Trim(),
        OriginCountry = s[2].GetString()!,
        Longitude = GetDouble(s[5]),
        Latitude = GetDouble(s[6]),
        BaroAltitude = GetDouble(s[7]),
        OnGround = s[8].GetBoolean()
    };

    private static double? GetDouble(JsonElement e) =>
    e.ValueKind == JsonValueKind.Null ? null : e.GetDouble();

}