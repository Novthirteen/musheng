using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Persistence.Hql;
using NHibernate.Type;
using System.Collections;

namespace com.Sconit.Service.Hql.Impl
{
    public class HqlMgr : SessionBase, IHqlMgr
    {
        public IHqlDao hqlDao { get; set; }

        public void Delete(string hqlString)
        {
            this.hqlDao.Delete(hqlString);
        }

        public void Delete(string hqlString, object value, IType type)
        {
            this.hqlDao.Delete(hqlString, value, type);
        }

        public void Delete(string hqlString, object[] values, IType[] types)
        {
            this.hqlDao.Delete(hqlString, values, types);
        }

        public IList FindAll(string hqlString)
        {
            return this.hqlDao.FindAll(hqlString);
        }

        public IList FindAll(string hqlString, object value)
        {
            return this.hqlDao.FindAll(hqlString, value);
        }

        public IList FindAll(string hqlString, object value, IType type)
        {
            return this.hqlDao.FindAll(hqlString, value, type);
        }

        public IList FindAll(string hqlString, object[] values)
        {
            return this.hqlDao.FindAll(hqlString, values);
        }

        public IList FindAll(string hqlString, object[] values, IType[] types)
        {
            return this.hqlDao.FindAll(hqlString, values, types);
        }

        public IList FindAll(string hqlString, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll(hqlString, firstRow, maxRows);
        }

        public IList FindAll(string hqlString, object value, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll(hqlString, value, firstRow, maxRows);
        }

        public IList FindAll(string hqlString, object value, IType type, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll(hqlString, value, type, firstRow, maxRows);
        }

        public IList FindAll(string hqlString, object[] values, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll(hqlString, values, firstRow, maxRows);
        }

        public IList FindAll(string hqlString, object[] values, IType[] type, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll(hqlString, values, type, firstRow, maxRows);
        }

        public IList<T> FindAll<T>(string hqlString)
        {
            return this.hqlDao.FindAll<T>(hqlString);
        }

        public IList<T> FindAll<T>(string hqlString, object value)
        {
            return this.hqlDao.FindAll<T>(hqlString, value);
        }

        public IList<T> FindAll<T>(string hqlString, object value, IType type)
        {
            return this.hqlDao.FindAll<T>(hqlString, value, type);
        }

        public IList<T> FindAll<T>(string hqlString, object[] values)
        {
            return this.hqlDao.FindAll<T>(hqlString, values);
        }

        public IList<T> FindAll<T>(string hqlString, object[] values, IType[] types)
        {
            return this.hqlDao.FindAll<T>(hqlString, values, types);
        }

        public IList<T> FindAll<T>(string hqlString, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll<T>(hqlString, firstRow, maxRows);
        }

        public IList<T> FindAll<T>(string hqlString, object value, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll<T>(hqlString, value, firstRow, maxRows);
        }

        public IList<T> FindAll<T>(string hqlString, object value, IType type, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll<T>(hqlString, value, type, firstRow, maxRows);
        }

        public IList<T> FindAll<T>(string hqlString, object[] values, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll<T>(hqlString, values, firstRow, maxRows);
        }

        public IList<T> FindAll<T>(string hqlString, object[] values, IType[] type, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll<T>(hqlString, values, type, firstRow, maxRows);
        }

        public IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param)
        {
            return this.hqlDao.FindAll<T>(hqlString, param);
        }

        public IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param, IType[] types)
        {
            return this.hqlDao.FindAll<T>(hqlString, param, types);
        }

        public IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param, IType[] types, int firstRow, int maxRows)
        {
            return this.hqlDao.FindAll<T>(hqlString, param, types, firstRow, maxRows);
        }

        public IList FindAllWithNativeSql(string sql, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql(sql, columns);
        }

        public IList FindAllWithNativeSql(string sql, object value, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql(sql, value, columns);
        }

        public IList FindAllWithNativeSql(string sql, object value, IType type, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql(sql, value, type, columns);
        }

        public IList FindAllWithNativeSql(string sql, object[] values, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql(sql, values, columns);
        }

        public IList FindAllWithNativeSql(string sql, object[] values, IType[] types, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql(sql, values, types, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql<T>(sql, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object value, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql<T>(sql, value, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object value, IType type, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql<T>(sql, value, type, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object[] values, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql<T>(sql, values, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object[] values, IType[] types, IDictionary<String, IType> columns)
        {
            return hqlDao.FindAllWithNativeSql<T>(sql, values, types, columns);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql)
        {
            return hqlDao.FindEntityWithNativeSql<T>(sql);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object value)
        {
            return hqlDao.FindEntityWithNativeSql<T>(sql, value);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object value, IType type)
        {
            return hqlDao.FindEntityWithNativeSql<T>(sql, value, type);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object[] values)
        {
            return hqlDao.FindEntityWithNativeSql<T>(sql, values);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object[] values, IType[] types)
        {
            return hqlDao.FindEntityWithNativeSql<T>(sql, values, types);
        }
    }
}

#region Extend Class

namespace com.Sconit.Service.Ext.Hql.Impl
{
    public partial class HqlMgrE : com.Sconit.Service.Hql.Impl.HqlMgr, IHqlMgrE
    {
    }
}

#endregion
