using Microsoft.Knowzy.Xamarin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Knowzy.Xamarin.Services
{
    public class MockInventoryService
    {
        private List<InventoryModel> _inventoryList;

        private MockInventoryService()
        {
            _populateMockData();
        }

        private void _populateMockData()
        {
            _inventoryList = new List<InventoryModel>
            {
                new InventoryModel { InventoryId = "1", Engineer = "aaa", Name = "name1", RawMaterial = "" },
                new InventoryModel { InventoryId = "2", Engineer = "bbb", Name = "name2", RawMaterial = "" },
                new InventoryModel { InventoryId = "3", Engineer = "ccc", Name = "name3", RawMaterial = "" },
                new InventoryModel { InventoryId = "4", Engineer = "ddd", Name = "name4", RawMaterial = "" },
                new InventoryModel { InventoryId = "5", Engineer = "eee", Name = "name5", RawMaterial = "" }
            };
        }

        private static MockInventoryService current;

        public static MockInventoryService Current => current ?? (current = new MockInventoryService());

        public List<InventoryModel> GetInventory()
        {
            return _inventoryList;
        }

    }
}
