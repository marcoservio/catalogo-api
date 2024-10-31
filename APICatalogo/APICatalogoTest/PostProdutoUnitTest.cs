using APICatalogo.Controllers;
using APICatalogo.DTOs;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

namespace APICatalogoTest;

public class PostProdutoUnitTest(ProdutosControllerTest controller) : IClassFixture<ProdutosControllerTest>
{
    private readonly ProdutosController _controller = new ProdutosController(controller.repository, controller.unitOfWork, controller.mapper);

    [Fact]
    public async Task PostProduto_Return_CreatedStatusCode()
    {
        var novoProduto = new ProdutoDTO
        {
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 2
        };

        var data = await _controller.Post(novoProduto);

        var result = data.Should().BeOfType<CreatedAtRouteResult>();
        result.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduto_Return_BadRequest()
    {
        ProdutoDTO novoProduto = null!;

        var data = await _controller.Post(novoProduto);

        var result = data.Should().BeOfType<BadRequestResult>();
        result.Subject.StatusCode.Should().Be(400);
    }
}
