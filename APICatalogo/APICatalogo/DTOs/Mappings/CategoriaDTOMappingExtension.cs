using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings;

public static class CategoriaDTOMappingExtension
{
    public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
    {
        if (categoria is null)
            return null;

        return new CategoriaDTO
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl,
        };
    }

    public static Categoria? ToCategoria(this CategoriaDTO dto)
    {
        if (dto is null)
            return null;

        return new Categoria
        {
            Id = dto.Id,
            Nome = dto.Nome,
            ImagemUrl = dto.ImagemUrl,
        };
    }

    public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias) { 
    
        if(categorias is null || categorias.Count() <= 0)
            return new List<CategoriaDTO>();

        return categorias.Select(categorias => new CategoriaDTO
        {
            Id = categorias.Id,
            Nome = categorias.Nome,
            ImagemUrl = categorias.ImagemUrl,
        }).ToList();
    }
}
