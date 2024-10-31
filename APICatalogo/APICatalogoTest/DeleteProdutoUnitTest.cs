using APICatalogo.Controllers;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

namespace APICatalogoTest;

public class DeleteProdutoUnitTest(ProdutosControllerTest controller) : IClassFixture<ProdutosControllerTest>
{
    private readonly ProdutosController _controller = new ProdutosController(controller.repository, controller.unitOfWork, controller.mapper);

    [Fact]
    public async Task DeleteProdutoById_Return_OkResult()
    {
        var prodId = 3;

        var result = await _controller.Delete(prodId);

        result.Should().NotBeNull();    
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteProdutoById_Return_BadRequest()
    {
        var prodId = 999;

        var result = await _controller.Delete(prodId);

        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestResult>();
    }
}
