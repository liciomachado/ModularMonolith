using ModularMonolith.Core.Utils;

namespace ModularMonolith.ExternalServices.Car;

public interface ICarGateway
{
    Task<Result<CarByCodeResponse, Error>> ConsultTerritoryByCode(string carCode);
    Task<HttpResponseMessage> CallConsultTerritoryByCode(string carCode);
}