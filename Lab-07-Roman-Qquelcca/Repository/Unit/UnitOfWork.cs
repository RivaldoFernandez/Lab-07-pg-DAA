namespace Lab_07_Roman_Qquelcca.Repository.Unit;

﻿using System.Collections;
using Lab_07_Roman_Qquelcca.Models;


public class UnitOfWork : IUnitOfWork
{
    private readonly MiddlewareDbContext _context;
    private readonly Hashtable _repositories;

    public UnitOfWork(MiddlewareDbContext context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type))
            return (IGenericRepository<TEntity>)_repositories[type]!;

        var repoType = typeof(GenericRepository<>);
        var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);

        if (repoInstance == null)
            throw new Exception($"No se pudo crear el repositorio para el tipo {type}");

        _repositories.Add(type, repoInstance);
        return (IGenericRepository<TEntity>)repoInstance;
    }
}