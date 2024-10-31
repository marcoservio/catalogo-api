using APICatalogo.Models;
using APICatalogo.Pagination;

using X.PagedList;

namespace APICatalogo.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IPagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
    Task<IPagedList<Produto>> GetProdutosFiltroPreco(ProdutosFiltoPreco filtro);
    Task<IEnumerable<Produto>> GetPorCategoria(int id);
}
