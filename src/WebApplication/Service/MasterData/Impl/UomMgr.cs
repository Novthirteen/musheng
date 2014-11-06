using com.Sconit.Service.Ext.MasterData;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using NHibernate.Expression;
using com.Sconit.Entity.Exception;


//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class UomMgr : UomBaseMgr, IUomMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public IItemMgrE itemMgrE { get; set; }
       

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public Uom CheckAndLoadUom(string uomCode)
        {
            Uom uom = this.LoadUom(uomCode);
            if (uom == null)
            {
                throw new BusinessErrorException("Uom.Error.UomCodeNotExist", uomCode);
            }

            return uom;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList GetUom(string code, string name, string desc)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(Uom));
            if (code != string.Empty && code != null)
                criteria.Add(Expression.Like("Code", code, MatchMode.Anywhere));
            if (name != string.Empty && name != null)
                criteria.Add(Expression.Like("Name", name, MatchMode.Anywhere));
            if (desc != string.Empty && desc != null)
                criteria.Add(Expression.Like("Description", desc, MatchMode.Anywhere));
            return criteriaMgrE.FindAll(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<Uom> GetItemUom(string itemCode)
        {
            List<Uom> uomList = new List<Uom>();
            if (itemCode != null && itemCode != string.Empty)
            {
                IList<UomConversion> uomConversionList = uomConversionMgrE.GetUomConversion(itemCode);

                //添加基本单位
                uomList.Add(itemMgrE.LoadItem(itemCode).Uom);
                //添加单位转换中的单位
                foreach (UomConversion uomConversion in uomConversionList)
                {
                    if (!uomList.Contains(uomConversion.BaseUom))
                        uomList.Add(uomConversion.BaseUom);
                    if (!uomList.Contains(uomConversion.AlterUom))
                        uomList.Add(uomConversion.AlterUom);
                }
            }
            return uomList;
        }

        #endregion Customized Methods
    }
}


#region Extend Class


namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class UomMgrE : com.Sconit.Service.MasterData.Impl.UomMgr, IUomMgrE
    {
       
    }
}
#endregion
