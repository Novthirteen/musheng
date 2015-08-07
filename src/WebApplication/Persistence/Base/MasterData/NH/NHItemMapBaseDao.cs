using Castle.Facilities.NHibernateIntegration;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Type;
using com.Sconit.Entity.MasterData;

namespace com.Sconit.Persistence.MasterData.NH
{
    public class NHItemMapBaseDao : NHDaoBase, IItemMapBaseDao
    {
         public NHItemMapBaseDao(ISessionManager sessionManager)
            : base(sessionManager)
        {
        }

         public virtual void DeleteItemMap(String id)
         {
             string hql = @"from ItemMap entity where entity.Item = ?";
             Delete(hql, new object[] { id }, new IType[] { NHibernateUtil.String });
         }

         public virtual ItemMap LoadItemMap(String item)
         {
             return FindById<ItemMap>(item);
         }

         public virtual void CreateItemMap(ItemMap itemMap)
         {
             Create(itemMap);
         }
    }
}
