

namespace Ejercicio.Persistencia.Interfaces
{
    using Ejercicio.Persistencia.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Data;
    public interface IRepositorioGenerico<T> : IDisposable where T : class
    {
        //Transacciones deshabilitadas
        //IDbTransaction BeginTransaction();
        int Count();
        bool Delete(object id, IDbTransaction transaction = null);
        bool DeleteByPK(IPrimaryKey primarykey, IDbTransaction transaction = null);
        bool DeleteByUid(Guid Uid, IDbTransaction transaction = null);
        bool SoftDeleteAllowed();
        bool SoftDelete(Guid Uid, IDbTransaction transaction = null);
        bool SoftDelete(int id, IDbTransaction transaction = null);        
        bool SoftDelete(IPrimaryKey primarykey, IDbTransaction transaction = null);
        bool UndoSoftDelete(Guid Uid, IDbTransaction transaction = null);
        IEnumerable<T> GetAll(int take, out int totalElementos, int skip = 0, string orderBy = "", string where = "", IDbTransaction transaction = null, bool mostrarBorrados = false);
        IEnumerable<E> GetAllCustom<E>(int take, out int totalElementos, int skip = 0, string propSelectSql = null, string joinSql = "", string whereSql = "", string orderBy = "", IDbTransaction transaction = null, bool mostrarBorrados = false);
        IEnumerable<E> Query<E>(string query, IDbTransaction transaction = null);
        List<List<E>> QueryMultiple<E>(List<string> queries);
        T GetById(object id, IDbTransaction transaction = null, bool mostrarBorrados = false);
        T GetByPK(IPrimaryKey id, IDbTransaction transaction = null, bool mostrarBorrados = false);
        T GetByUid(Guid id, IDbTransaction transaction = null, bool mostrarBorrados = false);
        List<T> GetByIds(List<object> ids, IDbTransaction transaction = null, bool mostrarBorrados = false);        
        bool Insert(T entidad, IDbTransaction transaction = null);
        bool Insert<E>(E entidad, IDbTransaction transaction = null);
        bool Update(T entidad, IDbTransaction transaction = null, bool mostrarBorrados = false);
        bool Update<E>(E entidad, IDbTransaction transaction = null, bool mostrarBorrados = false);
    }
}
