using APICatalogo.Controllers;
using APICatalogo.DTOs;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

namespace APICatalogoTest;

public class GetProdutoTest(ProdutosControllerTest controller) : IClassFixture<ProdutosControllerTest>
{
    private readonly ProdutosController _controller = new ProdutosController(controller.repository, controller.unitOfWork, controller.mapper);

    [Fact]
    public async Task GetProdutoById_Return_OKResult()
    {
        var prodId = 2;

        var data = await _controller.Get(prodId);

        data.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutoById_Return_NotFound()
    {
        var prodId = 999;

        var data = await _controller.Get(prodId);

        data.Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProdutoById_Return_BadReqeust()
    {
        var prodId = -1;

        var data = await _controller.Get(prodId);

        data.Should().BeOfType<BadRequestResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProdutoDto()
    {
        var data = await _controller.GetAll();

        data.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProdutos_Return_BadRequest()
    {
        var data = await _controller.GetAll(true);

        data.Should().BeOfType<BadRequestResult>()
            .Which.StatusCode.Should().Be(400);
    }
}
