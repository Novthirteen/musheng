using Castle.Services.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemMapMgr : ItemMapBaseMgr, IItemMapMgr
    {
    }
}

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ItemMapMgrE : com.Sconit.Service.MasterData.Impl.ItemMapMgr, IItemMapMgrE
    {
    }
}
