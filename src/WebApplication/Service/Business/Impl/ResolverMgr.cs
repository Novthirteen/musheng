using com.Sconit.Service.Ext.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.MasterData;
using com.Sconit.Entity;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using Castle.Services.Transaction;

namespace com.Sconit.Service.Business.Impl
{
    /// <summary>
    /// 解析条码,指定Service.Impl
    /// </summary>
    public class ResolverMgr : IResolverMgr
    {
        public IDictionary<string, IBusinessMgrE> dicService { get; set; }
        //private static log4net.ILog log = log4net.LogManager.GetLogger("Log.Resolver");

        #region Public Method
        //[Transaction(TransactionMode.Unspecified)]
        public Resolver Resolve(Resolver resolver)
        {
            //inputResolver.Result = null;
            resolver.Command = null;
            this.CheckValid(resolver);

            //Resolver resolver = CloneHelper.DeepClone<Resolver>(inputResolver);

            this.AnalyzeBarcode(resolver);

            IBusinessMgrE processer = dicService[resolver.ModuleType];
            processer.Process(resolver);

            resolver.Input = null;
            return resolver;
        }
        #endregion

        #region Private Method
        private void CheckValid(Resolver resolver)
        {
            if (resolver == null)
            {
                throw new TechnicalException("resolver is null");
                //log.Error("resolver is null");
            }

            if (resolver.UserCode == null || resolver.UserCode.Trim() == string.Empty)
            {
                throw new TechnicalException("resolver.UserCode is null");
                //log.Error("resolver.UserCode is null");
            }

            if (resolver.ModuleType == null || resolver.ModuleType.Trim() == string.Empty)
            {
                throw new TechnicalException("resolver.ModuleType is null");
                //log.Error("resolver.ModuleType is null");
            }
        }

        private void AnalyzeBarcode(Resolver resolver)
        {
            string barcode = resolver.Input;
            if (barcode == null || barcode.Trim() == string.Empty)
            {
                throw new TechnicalException("resolver.Input is null");
            }

            //Special,"$"
            if (barcode.StartsWith(BusinessConstants.BARCODE_SPECIAL_MARK))
            {
                if (barcode.Length < 2)
                {
                    throw new TechnicalException("resolver.Input.length < 2 when ");
                }
                resolver.BarcodeHead = barcode.Substring(1, 1);
                resolver.Input = barcode.Substring(2, barcode.Length - 2);
                string codePrefix = barcode.Substring(1, 1);
                if (codePrefix == BusinessConstants.BARCODE_HEAD_FLOW
                    || codePrefix == BusinessConstants.BARCODE_HEAD_FLOW_FACILITY)
                {
                    resolver.CodePrefix = codePrefix;
                }
            }
            else
            {
                //Order
                if (barcode.StartsWith(BusinessConstants.CODE_PREFIX_ORDER))
                {
                    if (resolver.CodePrefix == BusinessConstants.CODE_PREFIX_PICKLIST)
                    {
                        resolver.Transformers = null;
                    }
                    resolver.BarcodeHead = BusinessConstants.CODE_PREFIX_ORDER;
                    resolver.CodePrefix = BusinessConstants.CODE_PREFIX_ORDER;
                }
                //WO
                else if (barcode.StartsWith(BusinessConstants.CODE_PREFIX_WORK_ORDER))
                {
                    resolver.BarcodeHead = BusinessConstants.CODE_PREFIX_WORK_ORDER;
                    resolver.CodePrefix = BusinessConstants.CODE_PREFIX_WORK_ORDER;
                }
                //PickList
                else if (barcode.StartsWith(BusinessConstants.CODE_PREFIX_PICKLIST))
                {
                    resolver.BarcodeHead = BusinessConstants.CODE_PREFIX_PICKLIST;
                    resolver.CodePrefix = BusinessConstants.CODE_PREFIX_PICKLIST;
                }
                //ASN
                else if (barcode.StartsWith(BusinessConstants.CODE_PREFIX_ASN))
                {
                    resolver.BarcodeHead = BusinessConstants.CODE_PREFIX_ASN;
                    resolver.CodePrefix = BusinessConstants.CODE_PREFIX_ASN;
                }
                //Inspection
                else if (barcode.StartsWith(BusinessConstants.CODE_PREFIX_INSPECTION))
                {
                    resolver.BarcodeHead = BusinessConstants.CODE_PREFIX_INSPECTION;
                    resolver.CodePrefix = BusinessConstants.CODE_PREFIX_INSPECTION;
                }
                //StockTaking
                else if (barcode.StartsWith(BusinessConstants.CODE_PREFIX_CYCCNT))
                {
                    resolver.BarcodeHead = BusinessConstants.CODE_PREFIX_CYCCNT;
                    resolver.CodePrefix = BusinessConstants.CODE_PREFIX_CYCCNT;
                }
                else
                {
                    resolver.BarcodeHead = BusinessConstants.BARCODE_HEAD_DEFAULT;
                }
            }
        }
        #endregion
    }
}



#region Extend Class

namespace com.Sconit.Service.Ext.Business.Impl
{
    /// <summary>
    /// 解析条码,指定Service.Impl
    /// </summary>
    public partial class ResolverMgrE : com.Sconit.Service.Business.Impl.ResolverMgr, IResolverMgrE
    {

    }
}

#endregion
