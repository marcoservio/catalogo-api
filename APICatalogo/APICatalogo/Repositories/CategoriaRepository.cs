using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
    public async Task<IPagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters)
    {
        var categoria = (await GetAll())
            .OrderBy(p => p.Id)
            .AsQueryable();

        var categoriaPaginados = await categoria
            .ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

        return categoriaPaginados;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNome(CategoriasFiltroNome filtro)
    {
        var categorias = (await GetAll()).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro.Nome))
            categorias = categorias.Where(c => c.Nome.Contains(filtro.Nome, StringComparison.OrdinalIgnoreCase));

        //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, filtro.PageNumber, filtro.PageSize);

        var categoriasFiltradas = await categorias
            .ToPagedListAsync(filtro.PageNumber, filtro.PageSize);

        return categoriasFiltradas;
    }
}
