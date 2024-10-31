using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public readonly AppDbContext _context = context;

    public async Task Commit() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
