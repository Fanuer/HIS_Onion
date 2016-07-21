﻿using System.Threading.Tasks;

namespace HIS.WebApi.Helper.Interfaces.MultiId
{
    public interface IRepositoryAddAndDeleteMultiId<T, in TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<bool> AddAsync(T model);
        Task<bool> RemoveAsync(TIdProperty[] id);
        Task<bool> RemoveAsync(T model);
        Task<bool> ExistsAsync(TIdProperty[] id);
    }
}