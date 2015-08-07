using com.Sconit.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Service.MasterData
{
    public interface IItemMapBaseMgr
    {
        void DeleteItemMap(String id);

        ItemMap LoadItemMap(String item);

        void UpdateItemMap(ItemMap itemMap);

        void CreateItemMap(ItemMap itemMap);
    }
}
