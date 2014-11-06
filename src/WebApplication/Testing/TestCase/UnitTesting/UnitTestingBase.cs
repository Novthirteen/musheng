using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using com.Sconit.Service.Ext.MasterData;
using SconitTesting.Utility;
using com.Sconit.Service.Ext.Procurement;

namespace SconitTesting.TestCase.UnitTesting
{
    public class UnitTestingBase
    {
        #region 请按字母排序书写
        protected ICycleCountResultMgrE TheCycleCountResultMgr { get { return ServiceHelper.GetService<ICycleCountResultMgrE>("CycleCountResultMgr.service"); } }
        protected IFlowMgrE TheFlowMgr { get { return ServiceHelper.GetService<IFlowMgrE>("FlowMgr.service"); } }
        protected ILeanEngineMgrE TheLeanEngineMgr { get { return ServiceHelper.GetService<ILeanEngineMgrE>("LeanEngineMgr.service"); } }
        protected IOrderLocationTransactionMgrE TheOrderLocationTransactionMgr { get { return ServiceHelper.GetService<IOrderLocationTransactionMgrE>("OrderLocationTransactionMgr.service"); } }
        protected IOrderMgrE TheOrderMgr { get { return ServiceHelper.GetService<IOrderMgrE>("OrderMgr.service"); } }
        #endregion

        #region Fixture setup and teardown
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
        }

        [TestFixtureTearDown]
        public void TearDownTestFixture()
        {
        }
        #endregion

    }
}
