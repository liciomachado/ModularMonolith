using System.Globalization;
using System.Text.Json.Serialization;

namespace ModularMonolith.ExternalServices.Car;

public record CarByCodeResponse(
    [property: JsonPropertyName("codigoMunicipio")] string CodigoMunicipio,
    [property: JsonPropertyName("codImovel")] string CodImovel,
    [property: JsonPropertyName("numArea")] double NumArea,
    //[property: JsonPropertyName("areaIntersect")] double AreaIntersect,
    [property: JsonPropertyName("codEstado")] string CodEstado,
    [property: JsonPropertyName("nomeMunicipio")] string NomeMunicipio,
    //[property: JsonPropertyName("numeModulo")] double NumeModulo,
    //[property: JsonPropertyName("tipoImovel")] string TipoImovel,
    [property: JsonPropertyName("situacao")] string Situacao,
    [property: JsonPropertyName("condicao")] string Condicao,
    [property: JsonPropertyName("centroidLongitude")] double CentroidLongitude,
    [property: JsonPropertyName("centroidLatitude")] double CentroidLatitude,
    [property: JsonPropertyName("geom")] string Geom,
    [property: JsonPropertyName("dadosExtratoPdf")] DadosExtratoResponse? DadosExtrato,
    [property: JsonPropertyName("reservaLegal")] IEnumerable<ReservaLegalResponse> ReservaLegal

);

public record ReservaLegalResponse(
    [property: JsonPropertyName("nomeTema")] string NomeTema,
    [property: JsonPropertyName("numArea")] double NumArea,
    [property: JsonPropertyName("geom")] string Geom
);

public record DadosExtratoResponse(
    [property: JsonPropertyName("centroideX")] double CentroidLongitude,
    [property: JsonPropertyName("centroideY")] double CentroidLatitude,
    [property: JsonPropertyName("estado")] string Estado,
    [property: JsonPropertyName("codigo")] string Codigo,
    [property: JsonPropertyName("area")] double Area,
    [property: JsonPropertyName("municipio")] string NomeMunicipio,
    [property: JsonPropertyName("dataRegistro")] string DateRegistro,
    [property: JsonPropertyName("modulosFiscais")] double ModulosFiscais
)
{
    public DateTime GetRegisterDate()
    {
        var dateFormat = new[] { "dd/MM/yyyy" };
        var date = DateTime.ParseExact(DateRegistro, dateFormat, CultureInfo.InvariantCulture);
        return date;
    }
};