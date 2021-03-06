﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Type;
using System.Collections;
using Castle.Facilities.NHibernateIntegration;
using NHibernate;

namespace com.Sconit.Persistence.Hql.NH
{
    public class NHHqlDao : NHDaoBase, IHqlDao
    {
        public NHHqlDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

        public virtual IList FindAll(string hqlString)
        {
            return FindAllWithCustomQuery(hqlString);
        }

        public virtual IList FindAll(string hqlString, object value)
        {
            return FindAllWithCustomQuery(hqlString, value);
        }

        public virtual IList FindAll(string hqlString, object value, IType type)
        {
            return FindAllWithCustomQuery(hqlString, value, type);
        }

        public virtual IList FindAll(string hqlString, object[] values)
        {
            return FindAllWithCustomQuery(hqlString, values);
        }

        public virtual IList FindAll(string hqlString, object[] values, IType[] types)
        {
            return FindAllWithCustomQuery(hqlString, values, types);
        }

        public virtual IList FindAll(string hqlString, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery(hqlString, firstRow, maxRows);
        }

        public virtual IList FindAll(string hqlString, object value, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery(hqlString, value, firstRow, maxRows);
        }

        public virtual IList FindAll(string hqlString, object value, IType type, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery(hqlString, value, type, firstRow, maxRows);
        }

        public virtual IList FindAll(string hqlString, object[] values, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery(hqlString, values, firstRow, maxRows);
        }

        public virtual IList FindAll(string hqlString, object[] values, IType[] types, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery(hqlString, values, types, firstRow, maxRows);
        }

        public virtual IList<T> FindAll<T>(string hqlString)
        {
            return FindAllWithCustomQuery<T>(hqlString);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object value)
        {
            return FindAllWithCustomQuery<T>(hqlString, value);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object value, IType type)
        {
            return FindAllWithCustomQuery<T>(hqlString, value, type);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object[] values)
        {
            return FindAllWithCustomQuery<T>(hqlString, values);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object[] values, IType[] types)
        {
            return FindAllWithCustomQuery<T>(hqlString, values, types);
        }

        public virtual IList<T> FindAll<T>(string hqlString, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery<T>(hqlString, firstRow, maxRows);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object value, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery<T>(hqlString, value, firstRow, maxRows);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object value, IType type, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery<T>(hqlString, value, type, firstRow, maxRows);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object[] values, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery<T>(hqlString, values, firstRow, maxRows);
        }

        public virtual IList<T> FindAll<T>(string hqlString, object[] values, IType[] types, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery<T>(hqlString, values, types, firstRow, maxRows);
        }

        public virtual IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param)
        {
            return FindAllWithCustomQuery<T>(hqlString, param);
        }

        public virtual IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param, IType[] types)
        {
            return FindAllWithCustomQuery<T>(hqlString, param, types);
        }

        public virtual IList<T> FindAll<T>(string hqlString, IDictionary<string, object> param, IType[] types, int firstRow, int maxRows)
        {
            return FindAllWithCustomQuery<T>(hqlString, param, types, firstRow, maxRows);
        }

        public IList FindAllWithNativeSql(string sql, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql(sql, (object[])null, (IType[])null, columns);
        }

        public IList FindAllWithNativeSql(string sql, object value, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql(sql, new object[] { value }, (IType[])null, columns);
        }

        public IList FindAllWithNativeSql(string sql, object value, IType type, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql(sql, new object[] { value }, new IType[] { type }, columns);

        }

        public IList FindAllWithNativeSql(string sql, object[] values, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql(sql, values, (IType[])null, columns);
        }

        public IList FindAllWithNativeSql(string sql, object[] values, IType[] types, IDictionary<String, IType> columns)
        {
            if (sql == null || sql.Length == 0) throw new ArgumentNullException("queryString");
            if (values != null && types != null && types.Length != values.Length) throw new ArgumentException("Length of values array must match length of types array");

            using (ISession session = GetSession())
            {
                try
                {
                    ISQLQuery query = session.CreateSQLQuery(sql);

                    if (columns != null)
                    {
                        foreach (string column in columns.Keys)
                        {
                            query.AddScalar(column, columns[column]);
                        }
                    }

                    if (values != null)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (types != null && types[i] != null)
                            {
                                query.SetParameter(i, values[i], types[i]);
                            }
                            else
                            {
                                query.SetParameter(i, values[i]);
                            }
                        }
                    }

                    IList result = query.List();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new DataException("Could not perform Find for custom query : " + sql, ex);
                }
            }
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql<T>(sql, (object[])null, (IType[])null, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object value, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql<T>(sql, new object[] { value }, (IType[])null, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object value, IType type, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql<T>(sql, new object[] { value }, new IType[] { type }, columns);

        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object[] values, IDictionary<String, IType> columns)
        {
            return FindAllWithNativeSql<T>(sql, values, (IType[])null, columns);
        }

        public IList<T> FindAllWithNativeSql<T>(string sql, object[] values, IType[] types, IDictionary<String, IType> columns)
        {
            if (sql == null || sql.Length == 0) throw new ArgumentNullException("queryString");
            if (values != null && types != null && types.Length != values.Length) throw new ArgumentException("Length of values array must match length of types array");

            using (ISession session = GetSession())
            {
                try
                {
                    ISQLQuery query = session.CreateSQLQuery(sql);

                    if (columns != null)
                    {
                        foreach (string column in columns.Keys)
                        {
                            query.AddScalar(column, columns[column]);
                        }
                    }

                    if (values != null)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (types != null && types[i] != null)
                            {
                                query.SetParameter(i, values[i], types[i]);
                            }
                            else
                            {
                                query.SetParameter(i, values[i]);
                            }
                        }
                    }

                    IList<T> result = query.List<T>();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new DataException("Could not perform Find for custom query : " + sql, ex);
                }
            }
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql)
        {
            return FindEntityWithNativeSql<T>(sql, (object[])null, (IType[])null);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object value)
        {
            return FindEntityWithNativeSql<T>(sql, new object[] { value }, (IType[])null);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object value, IType type)
        {
            return FindEntityWithNativeSql<T>(sql, new object[] { value }, new IType[] { type });

        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object[] values)
        {
            return FindEntityWithNativeSql<T>(sql, values, (IType[])null);
        }

        public IList<T> FindEntityWithNativeSql<T>(string sql, object[] values, IType[] types)
        {
            if (sql == null || sql.Length == 0) throw new ArgumentNullException("queryString");
            if (values != null && types != null && types.Length != values.Length) throw new ArgumentException("Length of values array must match length of types array");

            using (ISession session = GetSession())
            {
                try
                {
                    ISQLQuery query = session.CreateSQLQuery(sql).AddEntity(typeof(T));
                    if (values != null)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (types != null && types[i] != null)
                            {
                                query.SetParameter(i, values[i], types[i]);
                            }
                            else
                            {
                                query.SetParameter(i, values[i]);
                            }
                        }
                    }

                    IList<T> result = query.List<T>();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new DataException("Could not perform Find for custom query : " + sql, ex);
                }
            }
        }
    }
}
