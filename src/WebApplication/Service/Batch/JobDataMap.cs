using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace com.Sconit.Service.Batch
{
    [Serializable]
    public class JobDataMap
    {
        public IDictionary<string, string> dataMap = new Dictionary<string, string>();

        public bool ContainKey(string key)
        {
            return dataMap.ContainsKey(key);
        }

        public void PutData(string key, string data)
        {
            dataMap[key] = data;
        }

        public string GetStringValue(string key)
        {
            string data = dataMap[key];
            return data;
        }

        public bool GetBooleanValue(string key)
        {
            string data = dataMap[key];
            return data.ToUpper(CultureInfo.InvariantCulture).Equals("TRUE");
        }

        public DateTime GetDateTime(string key)
        {
            string data = dataMap[key];

            try
            {
                return DateTime.Parse(data);
            }
            catch (System.Exception)
            {
                throw new InvalidCastException("Identified object is not a DateTime.");
            }
        }

        public double GetDoubleValue(string key)
        {
            string data = dataMap[key];
            return Double.Parse((string)data, CultureInfo.InvariantCulture);
        }

        public float GetFloatValue(string key)
        {
            string obj = dataMap[key];
            return Single.Parse(obj, CultureInfo.InvariantCulture);
        }

        public int GetIntValue(string key)
        {
            string obj = dataMap[key];
            return Int32.Parse(obj, CultureInfo.InvariantCulture);
        }

        public long GetLongValue(string key)
        {
            string obj = dataMap[key];
            return Int64.Parse(obj, CultureInfo.InvariantCulture);
        }
    }
}


#region Extend Class

namespace com.Sconit.Service.Ext.Batch
{
    [Serializable]
    public partial class JobDataMap : com.Sconit.Service.Batch.JobDataMap
    {

    }
}

#endregion

