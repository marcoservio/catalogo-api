using APICatalogo.DTOs;
using APICatalogo.Enums;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using Newtonsoft.Json;

using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace APICatalogo.Controllers;

[EnableCors("OrigensComAcessorPermitido")]
//[EnableRateLimiting("fixedWindow")]
[Route("[controller]")]
[ApiController]
//[ApiExplorerSettings(IgnoreApi = true)]
[Produces("application/json")]
public class CategoriaController(ICategoriaRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    private readonly ICategoriaRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ServiceFilter(typeof(ApiLogginFilter))]
    [DisableRateLimiting]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _repository.GetAll();

        if (categorias?.Count() > 0)
        {
            var response = _mapper.Map<List<CategoriaDTO>>(categorias);
            return Ok(response);
        }

        return NoContent();
    }

    //[DisableCors]
    [HttpGet("{id:int}", Name = "ObterCategoria")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var categoria = await _repository.Get(c => c.Id == id);

        if (categoria is null)
            return NotFound();

        var response = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Post(CategoriaDTO request)
    {
        if (request is null)
            return BadRequest();

        var categoria = _mapper.Map<Categoria>(request);

        _repository.Add(categoria);
        await _unitOfWork.Commit();

        var response = _mapper.Map<CategoriaDTO>(categoria);

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Put(int id, CategoriaDTO request)
    {
        if (id != request.Id)
            return BadRequest();

        var categoria = _mapper.Map<Categoria>(request);

        _repository.Update(categoria);
        await _unitOfWork.Commit();

        var response = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = nameof(PolicyNames.AdminOnly))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await _repository.Get(c => c.Id == id);

        if (categoria is null)
            return BadRequest();

        _repository.Delete(categoria!);
        await _unitOfWork.Commit();

        return NoContent();
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] CategoriasParameters parameters)
    {
        var categorias = await _repository.GetCategorias(parameters);

        return ObterCategorias(categorias);
    }

    [HttpGet("filter/nome/pagination")]
    public async Task<IActionResult> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome filto)
    {
        var categorias = await _repository.GetCategoriasFiltroNome(filto);

        return ObterCategorias(categorias);
    }

    private IActionResult ObterCategorias(IPagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        if (categorias?.Count > 0)
        {
            var response = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(response);
        }

        return NoContent();
    }
}
