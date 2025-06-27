using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Core.Utils;
using ModularMonolith.Core.WebApi;
using ModularMonolith.Purchase.Application.UseCases;
using Swashbuckle.AspNetCore.Annotations;

namespace ModularMonolith.Purchase.Api;

[Authorize]
public class CartController : MainController
{
    [HttpGet]
    [SwaggerOperation(Summary = "Obtém o carrinho do usuário autenticado")]
    [SwaggerResponse(200, "Sucesso", typeof(GetCartByUserResponse))]
    [SwaggerResponse(404, "Carrinho não encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> GetMyCart(
        [FromServices] IGetCartByUserUseCase useCase)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(new ErrorRequest("Usuário não autenticado"));

        return ManageResponse(await useCase.Execute(userId.Value));
    }

    [HttpPost("add-item")]
    [SwaggerOperation(Summary = "Adiciona item ao carrinho")]
    [SwaggerResponse(200, "Sucesso", typeof(AddItemCartResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Add(
        [FromServices] IAddItemCartUseCase useCase,
        [FromBody] AddItemCartRequest request)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(new ErrorRequest("Usuário não autenticado"));

        return ManageResponse(await useCase.Execute(request, userId.Value));
    }

    [HttpDelete("remove-item")]
    [SwaggerOperation(Summary = "Remove item do carrinho")]
    [SwaggerResponse(200, "Sucesso", typeof(RemoveItemCartResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Remove(
        [FromServices] IRemoveItemCartUseCase useCase,
        [FromBody] RemoveItemCartRequest request)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(new ErrorRequest("Usuário não autenticado"));

        return ManageResponse(await useCase.Execute(request, userId.Value));
    }

    [HttpPut("update-item")]
    [SwaggerOperation(Summary = "Atualiza a quantidade de um item do carrinho")]
    [SwaggerResponse(200, "Sucesso", typeof(UpdateItemCartResponse))]
    [SwaggerResponse(400, "Parametros invalidos", typeof(ErrorRequest))]
    [SwaggerResponse(404, "Nao encontrado", typeof(ErrorRequest))]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateItemCartUseCase useCase,
        [FromBody] UpdateItemCartRequest request)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(new ErrorRequest("Usuário não autenticado"));

        return ManageResponse(await useCase.Execute(request, userId.Value));
    }


}