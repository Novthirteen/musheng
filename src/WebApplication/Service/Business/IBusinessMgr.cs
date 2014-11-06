using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;

namespace com.Sconit.Service.Business
{
    public interface IBusinessMgr
    {
        void Process(Resolver resolver);
    }
}


﻿
#region Extend Interface

namespace com.Sconit.Service.Ext.Business
{
    public partial interface IBusinessMgrE : com.Sconit.Service.Business.IBusinessMgr
    {
        
    }
}

#endregion
