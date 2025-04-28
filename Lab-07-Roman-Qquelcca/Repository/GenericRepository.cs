using System.Linq.Expressions;
using Lab_07_Roman_Qquelcca.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Lab_07_Roman_Qquelcca.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly MiddlewareDbContext _context;

    public GenericRepository(MiddlewareDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (filter != null) query = query.Where(filter);
        if (include != null) query = include(query);
        return await query.ToListAsync();
    }


    public async Task<IEnumerable<T>> GetAll(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = _context.Set<T>();
        if (filter != null) query = query.Where(filter);
        if (include != null) query = include(query);
        return await query.ToListAsync();
    }

    public async Task<T?> GetById(int id) => await _context.Set<T>().FindAsync(id);

    public async Task<T?> GetByIdString(string id) => await _context.Set<T>().FindAsync(id);

    public async Task<List<T>> GetByIds(IEnumerable<int> ids,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var idProp = typeof(T).GetProperties().FirstOrDefault(p =>
            p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
            p.Name.StartsWith("Id", StringComparison.OrdinalIgnoreCase));

        if (idProp == null || idProp.PropertyType != typeof(int))
            throw new InvalidOperationException($"No se encontró una propiedad 'Id' válida en {typeof(T).Name}");

        var param = Expression.Parameter(typeof(T), "e");
        var propertyAccess = Expression.Property(param, idProp.Name);
        var contains = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(int));
        var idsConst = Expression.Constant(ids);
        var containsCall = Expression.Call(contains, idsConst, propertyAccess);
        var lambda = Expression.Lambda<Func<T, bool>>(containsCall, param);

        IQueryable<T> query = _context.Set<T>().Where(lambda);
        if (include != null) query = include(query);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetByStringProperty(string propertyName, string value,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(param, propertyName);
        if (property.Type != typeof(string))
            throw new ArgumentException($"La propiedad '{propertyName}' no es string.");

        var valueExp = Expression.Constant(value, typeof(string));
        var equalExp = Expression.Equal(property, valueExp);
        var lambda = Expression.Lambda<Func<T, bool>>(equalExp, param);

        IQueryable<T> query = _context.Set<T>().Where(lambda);
        if (include != null) query = include(query);
        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
