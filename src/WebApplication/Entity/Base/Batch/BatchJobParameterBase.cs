using System;
using System.Collections;
using System.Collections.Generic;

//TODO: Add other using statements here

namespace com.Sconit.Entity.Batch
{
    [Serializable]
    public abstract class BatchJobParameterBase : EntityBase
    {
        #region O/R Mapping Properties
		
		private Int32 _id;
		public Int32 Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}
		private com.Sconit.Entity.Batch.BatchJobDetail _batchJobDetail;
		public com.Sconit.Entity.Batch.BatchJobDetail BatchJobDetail
		{
			get
			{
				return _batchJobDetail;
			}
			set
			{
				_batchJobDetail = value;
			}
		}
		private string _parameterName;
		public string ParameterName
		{
			get
			{
				return _parameterName;
			}
			set
			{
				_parameterName = value;
			}
		}
		private string _parameterValue;
		public string ParameterValue
		{
			get
			{
				return _parameterValue;
			}
			set
			{
				_parameterValue = value;
			}
		}

        #endregion

		public override int GetHashCode()
        {
			if (Id != 0)
            {
                return Id.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            BatchJobParameterBase another = obj as BatchJobParameterBase;

            if (another == null)
            {
                return false;
            }
            else
            {
            	return (this.Id == another.Id);
            }
        } 
    }
	
}
