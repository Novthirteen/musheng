﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NHibernate.Type;

namespace com.Sconit.Persistence.Hql
{
    public interface IHqlDao
    {
        void Delete(string hqlString);

        void Delete(string hqlString, object value, IType type);

        void Delete(string hqlString, object[] values, IType[] types);

        IList FindAll(string hqlString);

        IList FindAll(string hqlString, object value);

        IList FindAll(string hqlString, object value, IType type);

        IList FindAll(string hqlString, object[] values);

        IList FindAll(string hqlString, object[] values, IType[] types);

        IList FindAll(string hqlString, int firstRow, int maxRows);

        IList FindAll(string hqlString, object value, int firstRow, int maxRows);

        IList FindAll(string hqlString, object value, IType type, int firstRow, int maxRows);

        IList FindAll(string hqlString, object[] values, int firstRow, int maxRows);

        IList FindAll(string hqlString, object[] values, IType[] type, int firstRow, int maxRows);

        IList<T> FindAll<T>(string hqlString);

        IList<T> FindAll<T>(string hqlString, object value);

        IList<T> FindAll<T>(string hqlString, object value, IType type);

        IList<T> FindAll<T>(string hqlString, object[] values);

        IList<T> FindAll<T>(string hqlString, object[] values, IType[] types);

        IList<T> FindAll<T>(string hqlString, int firstRow, int maxRows);

        IList<T> FindAll<T>(string hqlString, object value, int firstRow, int maxRows);

        IList<T> FindAll<T>(string hqlString, object value, IType type, int firstRow, int maxRows);

        IList<T> FindAll<T>(string hqlString, object[] values, int firstRow, int maxRows);

        IList<T> FindAll<T>(string hqlString, object[] values, IType[] type, int firstRow, int maxRows);

        IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param);

        IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param, IType[] type);

        IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param, IType[] type, int firstRow, int maxRows);


        IList FindAllWithNativeSql(string sql, IDictionary<String, IType> columns);

        IList FindAllWithNativeSql(string sql, object value, IDictionary<String, IType> columns);

        IList FindAllWithNativeSql(string sql, object value, IType type, IDictionary<String, IType> columns);

        IList FindAllWithNativeSql(string sql, object[] values, IDictionary<String, IType> columns);

        IList FindAllWithNativeSql(string sql, object[] values, IType[] types, IDictionary<String, IType> columns);

        IList<T> FindAllWithNativeSql<T>(string sql, IDictionary<String, IType> columns);

        IList<T> FindAllWithNativeSql<T>(string sql, object value, IDictionary<String, IType> columns);

        IList<T> FindAllWithNativeSql<T>(string sql, object value, IType type, IDictionary<String, IType> columns);

        IList<T> FindAllWithNativeSql<T>(string sql, object[] values, IDictionary<String, IType> columns);

        IList<T> FindAllWithNativeSql<T>(string sql, object[] values, IType[] types, IDictionary<String, IType> columns);

        IList<T> FindEntityWithNativeSql<T>(string sql);

        IList<T> FindEntityWithNativeSql<T>(string sql, object value);

        IList<T> FindEntityWithNativeSql<T>(string sql, object value, IType type);

        IList<T> FindEntityWithNativeSql<T>(string sql, object[] values);

        IList<T> FindEntityWithNativeSql<T>(string sql, object[] values, IType[] types);
    }
}
