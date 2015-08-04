using com.Sconit.Entity.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Sconit.Persistence.MasterData
{
    public interface IItemMapBaseDao
    {
        void DeleteItemMap(String id);

        ItemMap LoadItemMap(String item);

        void CreateItemMap(ItemMap itemMap);
    }
}
