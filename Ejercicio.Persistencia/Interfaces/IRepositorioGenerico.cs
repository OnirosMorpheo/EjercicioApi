

namespace Ejercicio.Persistence.Interfaces
{
    using Ejercicio.Persistence.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public interface IRepositorioGenerico<T> : IDisposable where T : class
    {
        //Transacciones deshabilitadas
        //IDbTransaction BeginTransaction();
        int Count();
        bool Delete(object id, IDbTransaction transaction = null);
        bool DeleteByPK(IPrimaryKey primarykey, IDbTransaction transaction = null);
        bool DeleteByUid(Guid Uid, IDbTransaction transaction = null);
        Task<bool> DeleteByUidAsync(Guid Uid, IDbTransaction transaction = null);
        bool SoftDeleteAllowed();
        bool SoftDelete(Guid Uid, IDbTransaction transaction = null);
        Task<bool> SoftDeleteAsync(Guid Uid, IDbTransaction transaction = null);
        bool SoftDelete(int id, IDbTransaction transaction = null);        
        bool SoftDelete(IPrimaryKey primarykey, IDbTransaction transaction = null);
        bool UndoSoftDelete(Guid Uid, IDbTransaction transaction = null);
        IEnumerable<T> GetAll(int take, out int totalElementos, int skip = 0, string orderBy = "", string where = "", IDbTransaction transaction = null, bool mostrarBorrados = false);
        Task<IEnumerable<T>> GetAllAsync(int take, int skip = 0, string orderBy = "", string where = "", IDbTransaction transaction = null, bool mostrarBorrados = false);
        IEnumerable<E> GetAllCustom<E>(int take, out int totalElementos, int skip = 0, string propSelectSql = null, string joinSql = "", string whereSql = "", string orderBy = "", IDbTransaction transaction = null, bool mostrarBorrados = false);
        IEnumerable<E> Query<E>(string query, IDbTransaction transaction = null);
        List<List<E>> QueryMultiple<E>(List<string> queries);
        T GetById(object id, IDbTransaction transaction = null, bool mostrarBorrados = false);
        T GetByPK(IPrimaryKey id, IDbTransaction transaction = null, bool mostrarBorrados = false);
        T GetByUid(Guid id, IDbTransaction transaction = null, bool mostrarBorrados = false);
        Task<T> GetByUidAsync(Guid id, IDbTransaction transaction = null, bool mostrarBorrados = false);
        Task<bool> ExistsAsync(Guid id, bool mostrarBorrados = false);
        List<T> GetByIds(List<object> ids, IDbTransaction transaction = null, bool mostrarBorrados = false);        
        bool Insert(T entidad, IDbTransaction transaction = null);
        Task<bool> InsertAsync(T entidad, IDbTransaction transaction = null);
        bool Insert<E>(E entidad, IDbTransaction transaction = null);
        bool Update(T entidad, IDbTransaction transaction = null, bool mostrarBorrados = false);
        Task<bool> UpdateAsync(T entidad, IDbTransaction transaction = null, bool mostrarBorrados = false);
        bool Update<E>(E entidad, IDbTransaction transaction = null, bool mostrarBorrados = false);
    }
}
