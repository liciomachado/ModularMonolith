using Microsoft.Extensions.Configuration;
using ModularMonolith.Core.Utils;
using System.Net;
using System.Text.Json;

namespace ModularMonolith.ExternalServices.Car;

internal sealed class CarFunction(HttpClient httpClient, IConfiguration configuration) : ICarGateway
{
    public async Task<Result<CarByCodeResponse, Error>> ConsultTerritoryByCode(string carCode)
    {
        var iamKey = configuration["Iamkey"];
        var url = $"functions/car-v2/consult_territory_by_code/{carCode}/?IAMKEY={iamKey}";

        var response = await httpClient.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.BadRequest)
            return new BadRequestError("Erro ao obter resposta do car");

        response.EnsureSuccessStatusCode();
        var stringJson = await response.Content.ReadAsStringAsync();
        var output = JsonSerializer.Deserialize<AtpxDefaultResponse<CarByCodeResponse>>(stringJson);
        if (output?.ObjectResult is null)
            return new BadRequestError("Car não encontrado");

        return output!.ObjectResult;
    }

    public async Task<HttpResponseMessage> CallConsultTerritoryByCode(string carCode)
    {
        var iamKey = configuration["Iamkey"];
        var url = $"functions/car-v2/consult_territory_by_code/{carCode}/?IAMKEY={iamKey}";

        return await httpClient.GetAsync(url);
    }
}