using com.Sconit.Service.Ext.MasterData;


using System;
using Castle.Services.Transaction;
using com.Sconit.Entity;
using com.Sconit.Entity.MasterData;
using com.Sconit.Persistence.MasterData;
using com.Sconit.Entity.Distribution;
using com.Sconit.Entity.Exception;
using com.Sconit.Utility;
using com.Sconit.Persistence;
using System.Data;
using System.Data.SqlClient;


//TODO: Add other using statements here.

namespace com.Sconit.Service.MasterData.Impl
{
    [Transactional]
    public class NumberControlMgr : NumberControlBaseMgr, INumberControlMgr
    {
        public IEntityPreferenceMgrE entityPreferenceMgrE { get; set; }
        public IUomConversionMgrE uomConversionMgrE { get; set; }
        public ISqlHelperDao sqlHelperDao { get; set; }

        #region Customized Methods

        [Transaction(TransactionMode.Requires)]
        public string GenerateNumber(string code)
        {
            string numberSuffix = GetNextSequence(code);

            EntityPreference entityPreference = entityPreferenceMgrE.LoadEntityPreference(BusinessConstants.ENTITY_PREFERENCE_CODE_ORDER_LENGTH);
            int orderLength = int.Parse(entityPreference.Value);
            numberSuffix = numberSuffix.PadLeft(orderLength - code.Length, '0');


            return (code + numberSuffix);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateNumber(string code, int numberSuffixLength)
        {
            string numberSuffix = GetNextSequence(code);

            if (numberSuffix.Length > numberSuffixLength)
            {
                throw new TechnicalException("numberSuffix.length > numberSuffixLength");
            }

            numberSuffix = numberSuffix.PadLeft(numberSuffixLength, '0');

            return (code + numberSuffix);
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateHuId(int itemId, string sortAndColor)
        {
            return GenerateNumber(itemId.ToString(), 6) + sortAndColor;
        }

        [Transaction(TransactionMode.Requires)]
        public string GenerateOrderNo(string orderType)
        {
            if (BusinessConstants.CODE_MASTER_ORDER_TYPE_VALUE_PRODUCTION == orderType)
            {
                return GenerateNumber(BusinessConstants.CODE_PREFIX_WORK_ORDER);
            }
            else
            {
                return GenerateNumber(BusinessConstants.CODE_PREFIX_ORDER);
            }
        }
        #endregion Customized Methods

        #region Private Methods
        private String GetNextSequence(string code)
        {
            SqlParameter[] parm = new SqlParameter[2];

            parm[0] = new SqlParameter("@CodePrefix", SqlDbType.VarChar, 50);
            parm[0].Value = code;

            parm[1] = new SqlParameter("@NextSequence", SqlDbType.Int, 50);
            parm[1].Direction = ParameterDirection.InputOutput;

            sqlHelperDao.ExecuteStoredProcedure("GetNextSequence", parm);

            return parm[1].Value.ToString();
        }

        private void ReverseUpdateSequence(string code, int seq)
        {
            SqlParameter[] parm = new SqlParameter[2];

            parm[0] = new SqlParameter("@CodePrefix", SqlDbType.VarChar, 50);
            parm[0].Value = code;

            parm[1] = new SqlParameter("@NextSequence", SqlDbType.Int, 50);
            parm[1].Value = seq;

            sqlHelperDao.ExecuteStoredProcedure("GetNextSequence", parm);
        }
        #endregion
    }
}


#region Extend Class




namespace com.Sconit.Service.Ext.MasterData.Impl
{
    [Transactional]
    public partial class NumberControlMgrE : com.Sconit.Service.MasterData.Impl.NumberControlMgr, INumberControlMgrE
    {
        
    }
}
#endregion
