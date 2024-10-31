using APICatalogo.DTOs;
using APICatalogo.Enums;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
//[ApiExplorerSettings(IgnoreApi = true)]
[ApiConventionType(typeof(DefaultApiConventions))]
public class ProdutosController(IProdutoRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly IProdutoRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    
    [HttpGet("bycategoria/{id:int}")]
    public async Task<IActionResult> GetProdutosCategoria(int id)
    {
        var produtos = await _repository.GetPorCategoria(id);

        if (produtos?.Count() > 0)
        {
            var response = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(response);
        }

        return NotFound();
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] ProdutosParameters parameters)
    {
        var produtos = await _repository.GetProdutos(parameters);

        return ObterProdutos(produtos);
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.UserOnly))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAll(bool ehTest = false)
    {
        try
        {
            var produtos = await _repository.GetAll();

            if (ehTest)
                throw new Exception();

            if (produtos?.Count() > 0)
            {
                var response = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
                return Ok(response);
            }

            return NotFound();
        }
        catch (Exception)
        {
            return BadRequest();
        }        
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public async Task<IActionResult> Get(int id)
    {
        if (id <= 0)
            return BadRequest();

        var produto = await _repository.Get(p => p.Id == id);

        if (produto is null)
            return NotFound();

        var response = _mapper.Map<ProdutoDTO>(produto);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProdutoDTO request)
    {
        if (request is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(request);

        _repository.Add(produto);
        await _unitOfWork.Commit();

        var response = _mapper.Map<ProdutoDTO>(produto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = response.Id }, response);
    }

    [HttpPatch("{id:int}/UpdatePartial")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> request)
    {
        if (request is null || id <= 0)
            return BadRequest();

        var produto = await _repository.Get(p => p.Id == id);

        if (produto is null)
            return NotFound();

        var dto = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        request.ApplyTo(dto, ModelState);

        if (!ModelState.IsValid || TryValidateModel(dto))
            return BadRequest(ModelState);

        _mapper.Map(dto, produto);

        _repository.Update(produto);
        await _unitOfWork.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(int id, ProdutoDTO request)
    {
        if (id != request.Id)
            return BadRequest();

        var produto = _mapper.Map<Produto>(request);

        _repository.Update(produto);
        await _unitOfWork.Commit();

        var response = _mapper.Map<ProdutoDTO>(produto);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        var produto = await _repository.Get(p => p.Id == id);

        if (produto is null)
            return BadRequest();

        _repository.Delete(produto);
        await _unitOfWork.Commit();

        return NoContent();
    }

    [HttpGet("filter/preco/pagination")]
    public async Task<IActionResult> GetProdutosFilterPreco([FromQuery] ProdutosFiltoPreco filtro)
    {
        var produtos = await _repository.GetProdutosFiltroPreco(filtro);
        return ObterProdutos(produtos);
    }

    private IActionResult ObterProdutos(IPagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.Count,
            produtos.PageSize,
            produtos.PageCount,
            produtos.TotalItemCount,
            produtos.HasNextPage,
            produtos.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        if (produtos?.Count > 0)
        {
            var response = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(response);
        }

        return NoContent();
    }
}
