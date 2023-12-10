using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderingApplication.Contracts.Persistence;
using OrderingDomain.Common;
using OrderingInfrastructure.Persistence;

namespace OrderingInfrastructure.Repositories;
public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly OrderContext _context;

    public RepositoryBase(OrderContext context)
    {
        _context = context;
    }
    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeString = null, bool disableTracking = true)
    {
        var query = _context.Set<T>().AsQueryable();
        if (disableTracking) query = query.AsNoTracking();
        if (!string.IsNullOrEmpty(includeString)) query = query.Include(includeString);
        if (predicate is not null) query = query.Where(predicate);
        if (orderby is not null) 
            return await orderby(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
    {
        var query = _context.Set<T>().AsQueryable();
        if (disableTracking) query = query.AsNoTracking();
        if (includes is not null)
            includes.Aggregate(query, (current, include) => current.Include(include));
        if (predicate is not null) query = query.Where(predicate);
        if (orderby is not null)
            await orderby(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
