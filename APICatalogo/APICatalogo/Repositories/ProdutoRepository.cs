using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

using X.PagedList;

namespace APICatalogo.Repositories;

public class ProdutoRepository(AppDbContext context) : Repository<Produto>(context), IProdutoRepository
{
    public async Task<IEnumerable<Produto>> GetPorCategoria(int id)
    {
        return (await GetAll()).Where(p => p.CategoriaId == id);
    }

    //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.Nome)
    //        .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
    //        .Take(produtosParameters.PageSize)
    //        .ToList();
    //}

    public async Task<IPagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters)
    {
        var produtos = (await GetAll())
            .OrderBy(p => p.Nome)
            .AsQueryable();

        var produtosPaginados = await produtos
            .ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);

        return produtosPaginados;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPreco(ProdutosFiltoPreco filtro)
    {
        var produtos = (await GetAll()).AsQueryable();

        if (filtro.Preco.HasValue && !string.IsNullOrWhiteSpace(filtro.PrecoCriterio))
        {
            if (filtro.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco > filtro.Preco.Value).OrderBy(p => p.Preco);
            else if (filtro.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco < filtro.Preco.Value).OrderBy(p => p.Preco);
            else if (filtro.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                produtos = produtos.Where(p => p.Preco == filtro.Preco.Value).OrderBy(p => p.Preco);
        }

        var produtosFiltratos = await produtos.ToPagedListAsync(filtro.PageNumber, filtro.PageSize);

        return produtosFiltratos;
    }
}
