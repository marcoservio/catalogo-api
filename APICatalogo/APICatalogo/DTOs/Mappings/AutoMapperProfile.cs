using APICatalogo.Models;

using AutoMapper;

namespace APICatalogo.DTOs.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Produto, ProdutoDTO>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateRequest>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();

        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
    }
}
