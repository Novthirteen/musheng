using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;

namespace com.Sconit.Service.Business
{
    public interface IResolverMgr
    {
        Resolver Resolve(Resolver inputResolver);
    }
}




#region Extend Interface


namespace com.Sconit.Service.Ext.Business
{
    public partial interface IResolverMgrE : com.Sconit.Service.Business.IResolverMgr
    {
    }
}

#endregion
