using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace APICatalogo.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> Get(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public T Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<T>().Update(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<T>().Remove(entity);
        return entity;
    }
}
