using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repositories;
using APICatalogo.Repositories.Interfaces;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace APICatalogoTest;

public class ProdutosControllerTest
{
    public IProdutoRepository repository;
    public IUnitOfWork unitOfWork;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString = "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=root";

    static ProdutosControllerTest()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    public ProdutosControllerTest()
    {
        var config = new MapperConfiguration(c =>
        {
            c.AddProfile(new AutoMapperProfile());
        });

        mapper = config.CreateMapper();

        var context = new AppDbContext(dbContextOptions);
        unitOfWork = new UnitOfWork(context);
        repository = new ProdutoRepository(context);
    }
}
