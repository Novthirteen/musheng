using System;
using System.Collections.Generic;

using com.Sconit.Persistence;
using log4net;

namespace com.Sconit.Service
{
    /// <summary>
    /// The base class of all Session Class.
    /// </summary>
    public class SessionBase : ISession
    {

        public INHDao daoBase { get; set; }

        public void setDaoBase()
        {
            //this.daoBase = daoBase;
        }

        public IList<T> FindAll<T>()
        {
            return daoBase.FindAll<T>();
        }

        public IList<T> FindAll<T>(int firstRow, int maxRows)
        {
            return daoBase.FindAll<T>(firstRow, maxRows);
        }

        public T FindById<T>(object id)
        {
            return daoBase.FindById<T>(id);
        }

        public object Create(object instance)
        {
            return daoBase.Create(instance);
        }

        public void BatchCreate(IList<object> entities)
        {
            daoBase.BatchCreate(entities);
        }

        public void Update(object instance)
        {
            daoBase.Update(instance);
        }

        public void Delete(object instance)
        {
            daoBase.Delete(instance);
        }


        public void DeleteAll(Type type)
        {
            daoBase.DeleteAll(type);
        }

        public void Save(object instance)
        {
            daoBase.Save(instance);
        }

        public void FlushSession()
        {
            daoBase.FlushSession();
        }

        public void CleanSession()
        {
            daoBase.CleanSession();
        }
    }
}

#region 扩展


namespace com.Sconit.Service.Ext
{

    public partial class SessionBaseE : SessionBase
    {



    }
}

#endregion