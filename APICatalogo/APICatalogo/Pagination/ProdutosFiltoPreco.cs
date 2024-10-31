namespace APICatalogo.Pagination;

public class ProdutosFiltoPreco : QueryStringParameters
{
    public decimal? Preco { get; set; }
    public string? PrecoCriterio { get; set; }
}
