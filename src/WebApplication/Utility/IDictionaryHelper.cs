using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Utility
{
    public static class IDictionaryHelper
    {
        public static void AddRange<T1, T2>(IDictionary<T1, T2> sourceDic, IDictionary<T1, T2> addedDic)
        {
            if (sourceDic == null)
            {
                sourceDic = new Dictionary<T1, T2>();
            }

            if (addedDic != null && addedDic.Count > 0)
            {
                foreach (T1 t in addedDic.Keys)
                {                    
                    sourceDic.Add(t, addedDic[t]);
                }
            }
        }
    }
}
