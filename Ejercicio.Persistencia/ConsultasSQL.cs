
namespace Ejercicio.Persistence
{
    public class ConsultasSQL
    {
        public const string SQL_SECURIZAR_ID = "SELECT Uid, {0} as 'Nombre' FROM {1} WHERE [id] = {2}; ";
        public const string SQL_SECURIZAR_ID_NULL = "SELECT null as 'Uid', '' as 'Nombre'";

        public const string SQL_RESOLVER_Uid = "SELECT [id] FROM {0} WHERE [Uid] = '{1}'; ";
        public const string SQL_RESOLVER_Uid_NULL = "SELECT null as 'Id'";

        public const string SQL_SEPARADOR_PROP = ",";

        public const string SQL_GET_BY_ID = "SELECT * FROM {0} WHERE [{1}] = @ID";
        public const string SQL_GET_BY_KEY = "SELECT * FROM {0} WHERE {1}";

        public const string SQL_GET_BY_Uid = "SELECT * FROM {0} WHERE [{1}] = @Uid";
        public const string SQL_EXISTS_BY_Uid = "SELECT COUNT(*) FROM {0} WHERE [{1}] = @Uid";

        public const string SQL_GET_BY_IDS = "SELECT * FROM {0} WHERE [{1}] in @IDs";
        public const string SQL_GET_BY_PKS = "SELECT * FROM {0} WHERE ({1})";

        public const string SQL_DELETE_BY_ID = "DELETE FROM {0} WHERE [{1}] = @ID";
        public const string SQL_DELETE_BY_PK = "DELETE FROM {0} WHERE {1}";

        public const string SQL_DELETE_BY_Uid = "DELETE FROM {0} WHERE [{1}] = @Uid";

        public const string SQL_SOFT_DELETE_BY_Uid = "UPDATE {0} SET DeleteDate = getutcdate() WHERE [{1}] = @Uid AND (DeleteDate IS NULL OR DeleteDate > getutcdate())";
        public const string SQL_SOFT_DELETE_BY_PK = "UPDATE {0} SET DeleteDate = getutcdate() WHERE {1} AND (DeleteDate IS NULL OR DeleteDate > getutcdate())";

        public const string SQL_SOFT_DELETE_BY_Uid_AUDITORIA = "UPDATE {0} SET DeleteDate = getutcdate(), UpdateBy = '{1}', UpdatedDate = getutcdate() WHERE {2} = @Uid AND (DeleteDate IS NULL OR DeleteDate > getutcdate())";
        public const string SQL_SOFT_DELETE_BY_PK_AUDITORIA = "UPDATE {0} SET DeleteDate = getutcdate(), UpdateBy = '{1}', UpdatedDate = getutcdate() WHERE {2} AND (DeleteDate IS NULL OR DeleteDate > getutcdate())";

        public const string SQL_UNDO_SOFT_DELETE_BY_Uid = "UPDATE {0} SET DeleteDate = NULL WHERE [{1}] = @Uid";

        public const string SQL_UNDO_SOFT_DELETE_BY_Uid_AUDITORIA = "UPDATE {0} SET DeleteDate = NULL, UpdateBy = '{1}', UpdatedDate = getutcdate() WHERE [{2}] = @Uid";




        public const string SQL_COUNT = "SELECT COUNT(1) FROM {0}";

        public const string SQL_ALL_PROP = "{0}.*";
        public const string SQL_PROP = "{0}.[{1}]";

        public const string SQL_CUSTOM_PROP = "{0} AS '{1}'";

        public const string SQL_GET_ALL_CUSTOM = "SELECT {0} FROM {1} AS {2} {3} {4} ORDER BY {5} OFFSET {6} ROWS FETCH NEXT {7} ROWS ONLY";

        public const string SQL_GET_ALL_CUSTOM_COUNT = "SELECT COUNT(1) FROM {1} AS {2} {3} {4}";

        public const int DEFAULT_LENGTH_ALIAS = 3;

        public const string SQL_PARAMETRO = "@{0}";

        public const string SQL_SET = "[{0}] = @{0}";

        public const string SQL_INSERT_CHECK_Uid = @"
                declare @duplicado bit = 0

                select @duplicado = 1
                from {0} with(nolock)
                where {1} = '{2}'

                If( @duplicado = 0)
                begin
	                {3}
                    {4}
                end
                else
                begin
	                select 0 as 'Id'
                end
        ";
        public const string SQL_INSERT_INTO = "INSERT INTO {0}({1}) VALUES ({2});";

        public const string SQL_INSERT_INTO_GET_ID_BY_SCOPE = "select IIF(scope_identity() is null, -1, scope_identity()) as 'Id'";
        public const string SQL_INSERT_INTO_GET_ID_BY_Uid = "select {0} as 'Id' from {1} with(nolock) where [{2}] = '{3}'";


        public const string SQL_UPDATE = "UPDATE {0} SET {1} WHERE [{2}] = @{2}";
        public const string SQL_UPDATE_PK = "UPDATE {0} SET {1} WHERE {2}";

        public const string SQL_PROPIEDADES_AUDITORIA_INSERT = ", CreatedBy, CreatedDate, UpdateBy, UpdatedDate";

        public const string SQL_PROPIEDADES_AUDITORIA_INSERT_VALUE = ", '{0}', getutcdate(), '{0}', getutcdate()";

        public const string SQL_PROPIEDADES_AUDITORIA_UPDATE = ", UpdateBy = '{0}', UpdatedDate = getutcdate()";

        public const string SQL_FILTRO_FECHA_BORRADO = " AND (DeleteDate is null OR DeleteDate > getutcdate())";

        public const string SQL_FILTRO_WHERE_FECHA_BORRADO = " WHERE (DeleteDate is null OR DeleteDate > getutcdate())";

        public const string SQL_FILTRO_FECHA_BORRADO_ALIAS = " AND ({0}.DeleteDate is null OR {0}.DeleteDate > getutcdate())";

        public const string SQL_FILTRO_WHERE_FECHA_BORRADO_ALIAS = " WHERE ({0}.DeleteDate is null OR {0}.DeleteDate > getutcdate())";

    }
}
