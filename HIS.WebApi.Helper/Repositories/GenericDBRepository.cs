using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Base.Interfaces;
using HIS.WebApi.Auth.Base.Interfaces.SingleId;

namespace HIS.WebApi.Auth.Base.Repositories
{
  internal abstract class GenericDbRepository<T, TIdProperty> : IRepositoryAddAndDelete<T, TIdProperty>, IRepositoryFindAll<T>, IRepositoryFindSingle<T, TIdProperty>, IRepositoryUpdate<T, TIdProperty>, ICountAsync<T> where T : class, IEntity<TIdProperty>
  {
    #region Field

    #endregion

    #region Ctor

    protected GenericDbRepository(DbContext ctx)
    {
      if (ctx == null){throw new ArgumentNullException(nameof(ctx));}

      DBContext = ctx;
    }
    #endregion

    #region Method
    public async Task<bool> AddAsync(T model)
    {
      var result = false;
      if (model == null)
      {
        throw new ArgumentNullException(nameof(model));
      }

      try
      {
        var existingModel = await this.FindAsync(model.Id);
        if (existingModel != null)
        {
          await RemoveAsync(existingModel);
        }
        this.DBContext.Set<T>().Add(model);
        //this.DBContext.Set(typeof(T)).Add(model);
        result = await DBContext.SaveChangesAsync() > 0;
      }
      catch (Exception e)
      {
        throw new DbUpdateException($"Unable to add Entry of type {typeof(T).Namespace}", e);
      }
      return result;
    }

    public async Task<bool> RemoveAsync(TIdProperty id)
    {
      var model = await this.FindAsync(id);
      return await RemoveAsync(model);
    }

    public async Task<bool> RemoveAsync(T model)
    {
      if (model == null)
      {
        throw new ArgumentNullException("model");
      }
      this.DBContext.Set<T>().Remove(model);
      return await this.DBContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(TIdProperty id)
    {
      var all = await this.GetAllAsync();
      return all.Any(e => e.Id.Equals(id));
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
      var result = new List<T>();
      try
      {
        var dbSet = this.DBContext.Set<T>();

        if (dbSet != null)
        {
          result = await dbSet.ToListAsync();
        }
      }
      catch (Exception e)
      {
        throw new EntityCommandExecutionException($"Error on selecting all Data of type {typeof(T).Name}", e);
      }
      return result.AsQueryable();
    }

    public async Task<T> FindAsync(TIdProperty id)
    {
      T result = default(T);
      var dbSet = this.DBContext.Set<T>();
      if (dbSet != null)
      {
        result = await dbSet.FindAsync(id);
      }
      return result;

    }

    public async Task<bool> UpdateAsync(T model)
    {
      if (model == null)
      {
        throw new ArgumentNullException("model");
      }
      this.DBContext.Entry(model).State = EntityState.Modified;
      return await this.DBContext.SaveChangesAsync() > 0;
    }

    public async Task<int> CountAsync<T>() where T : class
    {
      DbSet<T> dbset = GetTypedDBSet<T>();
      var count = await dbset.CountAsync();
      return count;
    }


    protected DbSet<T> GetTypedDBSet<T>() where T : class
    {
      return (DbSet<T>)this.DBContext
                           .GetType()
                           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .First(x => x.PropertyType.GenericTypeArguments.Contains(typeof(T)))
                           .GetValue(this.DBContext);
    }

    #endregion

    #region Property

    protected DbContext DBContext { get; private set; }
    #endregion
  }
}
