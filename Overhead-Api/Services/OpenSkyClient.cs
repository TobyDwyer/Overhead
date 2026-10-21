using System.Globalization;
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

    public async Task<List<OpenSkyState>> GetPlanesInArea(OpenSkyAreaRequest request, CancellationToken ct = default)
    {
        var box = GetAreaBox(request.Lat, request.Long, request.Area);
        // Used to standardise to a . decimal
        var inv = CultureInfo.InvariantCulture;

        // F4 is used to format to 4 decimal points.
        var url = "states/all" + $"?lamin={box.MinLat.ToString("F4", inv)}" + $"&lomin={box.MinLong.ToString("F4", inv)}" + $"&lamax={box.MaxLat.ToString("F4", inv)}" + $"&lomax={box.MaxLong.ToString("F4", inv)}";

        var response = await http.GetFromJsonAsync<OpenSkyStatesResponse>(url, ct);

        return response?.States?.Select(MapResults).ToList() ?? [];
    }


    private static OpenSkyState MapResults(List<JsonElement> s) => new()
    {
        Icao24 = s[0].GetString()!,
        Callsign = s[1].GetString()?.Trim(),
        OriginCountry = s[2].GetString()!,
        Longitude = GetDouble(s[5]),
        Latitude = GetDouble(s[6]),
        BaroAltitude = GetDouble(s[7]),
        Heading = GetDouble(s[10]),
        OnGround = s[8].GetBoolean()
    };

    private static double? GetDouble(JsonElement e) =>
    e.ValueKind == JsonValueKind.Null ? null : e.GetDouble();

    private static AreaBox GetAreaBox(double lat, double lon, double radiusKm)
    {
        // Distance to angle , then radians to degrees
        // Radius / EarthRadius
        var latDelta = radiusKm / 6371.0 * (180.0 / Math.PI);

        var lonDelta = latDelta / Math.Cos(lat * Math.PI / 180.0);

        return new AreaBox(
            MinLat: Math.Max(lat - latDelta, -90),
            MinLong: Math.Max(lon - lonDelta, -180),
            MaxLat: Math.Min(lat + latDelta, 90),
            MaxLong: Math.Min(lon + lonDelta, 180)
        );
    }
}