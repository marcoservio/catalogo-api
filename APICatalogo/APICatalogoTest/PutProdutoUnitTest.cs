using APICatalogo.Controllers;
using APICatalogo.DTOs;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

namespace APICatalogoTest;

public class PutProdutoUnitTest(ProdutosControllerTest controller) : IClassFixture<ProdutosControllerTest>
{
    private readonly ProdutosController _controller = new ProdutosController(controller.repository, controller.unitOfWork, controller.mapper);

    [Fact]
    public async Task PutProduto_ReturnOkResult()
    {
        var prodId = 1;

        var updatedProduto = new ProdutoDTO
        {
            Id = prodId,
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 2
        };

        var result = await _controller.Put(prodId, updatedProduto);

        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PutProduto_Return_BadRequest()
    {
        var prodId = 10000;

        var updatedProduto = new ProdutoDTO
        {
            Id = 1,
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 2
        };

        var result = await _controller.Put(prodId, updatedProduto);

        result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
    }
}
