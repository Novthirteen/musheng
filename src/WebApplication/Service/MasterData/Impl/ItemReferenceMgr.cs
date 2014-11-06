using com.Sconit.Service.Ext.MasterData;


using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Service.Ext.Criteria;
using com.Sconit.Service.Ext.MasterData;
using NHibernate.Expression;

//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class ItemReferenceMgr : ItemReferenceBaseMgr, IItemReferenceMgr
    {
        public ICriteriaMgrE criteriaMgrE { get; set; }
        

        #region Customized Methods
        [Transaction(TransactionMode.Unspecified)]
        public override ItemReference LoadItemReference(string itemCode, string partyCode, string referenceCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemReference));
            criteria.Add(Expression.Eq("Item.Code", itemCode));
            if (partyCode != null && partyCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Party.Code", partyCode));
            }
            else
            {
                criteria.Add(Expression.IsNull("Party.Code"));
            }
            criteria.Add(Expression.Eq("ReferenceCode", referenceCode));
            IList<ItemReference> list = this.criteriaMgrE.FindAll<ItemReference>(criteria);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        [Transaction(TransactionMode.Unspecified)]
        public override ItemReference LoadItemReference(Item item, Party party, string referenceCode)
        {
            string partyCode = party == null ? string.Empty : party.Code;
            return this.LoadItemReference(item.Code, partyCode, referenceCode);
        }

        [Transaction(TransactionMode.Requires)]
        public override void DeleteItemReference(string itemCode, string partyCode, string referenceCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemReference));
            criteria.Add(Expression.Eq("Item.Code", itemCode));
            if (partyCode != null && partyCode.Trim() != string.Empty)
            {
                criteria.Add(Expression.Eq("Party.Code", partyCode));
            }
            else
            {
                criteria.Add(Expression.IsNull("Party.Code"));
            }
            criteria.Add(Expression.Eq("ReferenceCode", referenceCode));
            this.criteriaMgrE.Delete(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemReference> GetItemReference(string itemCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof(ItemReference));
            criteria.Add(Expression.Eq("Item.Code", itemCode));
            criteria.Add(Expression.Eq("IsActive", true));
            return this.criteriaMgrE.FindAll<ItemReference>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemReference> GetItemReference(Item item, Party party)
        {
            return GetItemReference(item.Code, party.Code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemReference> GetItemReference(string itemCode, string partyCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ItemReference>();

            criteria.Add(Expression.Eq("Item.Code", itemCode));
            criteria.Add(
                Expression.Or(
                Expression.Eq("Party.Code", partyCode),
                Expression.IsNull("Party.Code")
                )
                );
            criteria.Add(Expression.Eq("IsActive", true));

            return this.criteriaMgrE.FindAll<ItemReference>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemReference> GetItemReference(Item item, Party partyFrom, Party partyTo)
        {
            return GetItemReference(item.Code, partyFrom.Code, partyTo.Code);
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemReference> GetItemReference(string itemCode, string partyFromCode, string partyToCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ItemReference>();

            criteria.Add(Expression.Eq("Item.Code", itemCode));
            criteria.Add(
                Expression.Or(
                    Expression.Or(
                        Expression.Eq("Party.Code", partyFromCode),
                        Expression.Eq("Party.Code", partyToCode)
                    ),
                    Expression.IsNull("Party.Code")
                )
                );
            criteria.Add(Expression.Eq("IsActive", true));

            return this.criteriaMgrE.FindAll<ItemReference>(criteria);
        }

        [Transaction(TransactionMode.Unspecified)]
        public Item GetItemReferenceByRefItem(string refItemCode, string firstPartyCode, string secondPartyCode)
        {
            Item item = null;
            //��ѡfirstParty��
            if (firstPartyCode != string.Empty && firstPartyCode != null)
            {
                DetachedCriteria criteria = DetachedCriteria.For<ItemReference>();
                criteria.Add(Expression.Eq("ReferenceCode", refItemCode));
                criteria.Add(Expression.Eq("Party.Code", firstPartyCode));
                criteria.Add(Expression.Eq("IsActive", true));

                IList<ItemReference> itemRefList = criteriaMgrE.FindAll<ItemReference>(criteria);
                if (itemRefList != null && itemRefList.Count > 0)
                {
                    item = itemRefList[0].Item;
                }
            }

            //��ѡsecondParty��
            if (item == null && secondPartyCode != string.Empty && secondPartyCode != null)
            {
                DetachedCriteria partyCriteria = DetachedCriteria.For<ItemReference>();
                partyCriteria.Add(Expression.Eq("ReferenceCode", refItemCode));
                partyCriteria.Add(Expression.Eq("Party.Code", secondPartyCode));
                partyCriteria.Add(Expression.Eq("IsActive", true));

                IList<ItemReference> itemRefList = criteriaMgrE.FindAll<ItemReference>(partyCriteria);
                if (itemRefList != null && itemRefList.Count > 0)
                {
                    item = itemRefList[0].Item;
                }
            }
            if (item == null)
            {
                //��party��
                DetachedCriteria criteria = DetachedCriteria.For<ItemReference>();
                criteria.Add(Expression.Eq("ReferenceCode", refItemCode));
                criteria.Add(Expression.IsNull("Party"));
                criteria.Add(Expression.Eq("IsActive", true));
                IList<ItemReference> itemRefList = criteriaMgrE.FindAll<ItemReference>(criteria);
                if (itemRefList != null && itemRefList.Count > 0)
                {
                    item = itemRefList[0].Item;
                }
            }
            return item;
        }

        [Transaction(TransactionMode.Unspecified)]
        public string GetItemReferenceByItem(string itemCode, string firstPartyCode, string secondPartyCode)
        {
            string refItemCode = string.Empty;
            //��ѡfirstParty��
            if (firstPartyCode != string.Empty && firstPartyCode != null)
            {
                DetachedCriteria criteria = DetachedCriteria.For<ItemReference>();
                criteria.Add(Expression.Eq("Item.Code", itemCode));
                criteria.Add(Expression.Eq("Party.Code", firstPartyCode));
                criteria.Add(Expression.Eq("IsActive", true));

                IList<ItemReference> itemRefList = criteriaMgrE.FindAll<ItemReference>(criteria);
                if (itemRefList != null && itemRefList.Count == 1)
                {
                    refItemCode = itemRefList[0].ReferenceCode;
                }
                else if (itemRefList != null && itemRefList.Count > 1)
                {
                    return string.Empty;
                }
            }

            //��ѡsecondParty��
            if (refItemCode == string.Empty && secondPartyCode != string.Empty && secondPartyCode != null)
            {
                DetachedCriteria partyCriteria = DetachedCriteria.For<ItemReference>();
                partyCriteria.Add(Expression.Eq("Item.Code", itemCode));
                partyCriteria.Add(Expression.Eq("Party.Code", secondPartyCode));
                partyCriteria.Add(Expression.Eq("IsActive", true));

                IList<ItemReference> itemRefList = criteriaMgrE.FindAll<ItemReference>(partyCriteria);
                if (itemRefList != null && itemRefList.Count == 1)
                {
                    refItemCode = itemRefList[0].ReferenceCode;
                }
                else if (itemRefList != null && itemRefList.Count > 1)
                {
                    return string.Empty;
                }
            }
            if (refItemCode == string.Empty)
            {
                //��party��
                DetachedCriteria criteria = DetachedCriteria.For<ItemReference>();
                criteria.Add(Expression.Eq("Item.Code", itemCode));
                // criteria.Add(Expression.IsNull("Party"));
                criteria.Add(Expression.Eq("IsActive", true));
                IList<ItemReference> itemRefList = criteriaMgrE.FindAll<ItemReference>(criteria);
                if (itemRefList != null && itemRefList.Count >= 1)
                {
                    refItemCode = itemRefList[0].ReferenceCode;
                }
                else
                {
                    refItemCode = string.Empty;
                }
            }
            return refItemCode;
        }

        [Transaction(TransactionMode.Unspecified)]
        public IList<ItemReference> GetItemReferenceByParty(string partyFromCode, string partyToCode)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ItemReference>();

            criteria.Add(
                Expression.Or(
                    Expression.Or(
                        Expression.Eq("Party.Code", partyFromCode),
                        Expression.Eq("Party.Code", partyToCode)
                    ),
                    Expression.IsNull("Party.Code")
                )
                );
            criteria.Add(Expression.Eq("IsActive", true));

            return this.criteriaMgrE.FindAll<ItemReference>(criteria);
        }

        #endregion Customized Methods
    }
}



#region Extend Interface

namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class ItemReferenceMgrE : com.Sconit.Service.MasterData.Impl.ItemReferenceMgr, IItemReferenceMgrE
    {
        
    }
}
#endregion
