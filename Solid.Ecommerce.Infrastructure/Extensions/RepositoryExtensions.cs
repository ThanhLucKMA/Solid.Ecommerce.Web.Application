﻿
using System.Runtime.CompilerServices;

namespace Solid.Ecommerce.Infrastructure.Extensions;

public static class RepositoryExtensions
{
    /// <summary>
    /// Filter data with the query
    /// </summary>
    public static IQueryable<T> Where<T>
        (
        this IRepository<T> repository, 
        Expression<Func<T, bool>> predicate
        ) where T : class
    {
        return repository.Entities.Where(predicate);
    }

    public static async Task<List<T>> ToListAsync<T> (this IRepository<T> repository) where T : class
    {
        return await repository.Entities.ToListAsync();
    }

    /// <summary>
    /// Get list of data for the tenant
    /// </summary>
    public static async Task<List<T>> ToListAsync<T>
        (
        this IRepository<T> repository, 
        Expression<Func<T,bool>> predicate
        ) where T : class
        => await repository.Entities.Where(predicate).ToListAsync();

    /// <summary>
    /// Order data for the tenant
    /// </summary>
    public static IOrderedQueryable<T> OrderBy<T, TKey>(this IRepository<T> repository, Expression<Func<T, TKey>> keySelector)
        where T : class
    {
        return repository.Entities.OrderBy(keySelector);
    }

    /// <summary>
    /// Get first record for the tenant
    /// </summary>
    public static async Task<T> FirstOrDefaultAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await repository.Entities.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Get first record for the tenant
    /// </summary>
    public static async Task<T> SingleOrDefaultAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await repository.Entities.SingleOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Get last record for the tenant
    /// </summary>
    public static async Task<T> LastOrDefaultAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await repository.Entities.LastOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Get first record for the tenant
    /// </summary>
    public static async Task<T> SingleAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await repository.Entities.SingleAsync(predicate);
    }

    /// <summary>
    /// Get first record for the tenant
    /// </summary>
    public static async Task<T> FirstAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await repository.Entities.FirstAsync(predicate);
    }

    /// <summary>
    /// Check data is exist in the tenant
    /// </summary>
    public static async Task<bool> AnyAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await repository.Entities.AnyAsync(predicate);
    }

    /// <summary>
    /// Count data in the tenant
    /// </summary>
    public static async Task<int> CountAsync<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await repository.Entities.CountAsync(predicate);
    }

    /// <summary>
    /// Filter data for the tenant and include the navigation property
    /// </summary>
    public static IIncludableQueryable<T, TProperty> Include<T, TProperty>(this IRepository<T> repository, Expression<Func<T, TProperty>> path)
        where T : class
    {
        return repository.Entities.Include(path);
    }

    public static IQueryable<TResult> Select<T, TResult>(this IRepository<T> repository, Expression<Func<T, TResult>> selector)
        where T : class
    {
        return repository.Entities.Select(selector);
    }

    public static DbCommand LoadStoredProc<T>(this IRepository<T> repository, string storedProcName)
        where T : class
    {

        return repository.ApplicationDBContext.DbContext.LoadStoredProc(storedProcName);
    }

    public static DbCommand LoadCommand<T>(this IRepository<T> repository, string storedProcName)
        where T : class
    {
        var conn = repository.ApplicationDBContext.DbContext.Database.GetDbConnection();
        var cmd = conn.CreateCommand();

        if (repository.ApplicationDBContext.DbContext.Database.CurrentTransaction != null)
        {
            cmd.Transaction = repository.ApplicationDBContext.DbContext.Database.CurrentTransaction.GetDbTransaction();
        }

        cmd.CommandText = storedProcName;
        cmd.CommandType = System.Data.CommandType.Text;
        return cmd;
    }

    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        where T : class
    {
        return await query.Where(predicate).ToListAsync();
    }

    public static async Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
    {
        return await query.Where(predicate).FirstOrDefaultAsync();
    }

    public static void Load<T, TProperty>(this IRepository<T> repository, T entity, Expression<Func<T, TProperty>> propertyExpression)
        where T : class where TProperty : class
    {
        repository.ApplicationDBContext.DbContext.Entry(entity).Reference(propertyExpression).Load();
    }

    private static void LoadEntity<T, T1, TProperty>(this IRepository<T> repository, T1 entity, Expression<Func<T1, TProperty>> propertyExpression)
        where T : class
        where TProperty : class 
        where T1 : class
    {
        repository.ApplicationDBContext.DbContext.Entry(entity).Reference(propertyExpression).Load();
    }

    public static void LoadEntities<T, T1, TProperty>(this IRepository<T> repository, IEnumerable<T1> entities, Expression<Func<T1, TProperty>> propertyExpression)
        where T : class where TProperty : class where T1 : class
    {
        foreach (var entity in entities)
        {
            repository.LoadEntity(entity, propertyExpression);
        }
    }
}
