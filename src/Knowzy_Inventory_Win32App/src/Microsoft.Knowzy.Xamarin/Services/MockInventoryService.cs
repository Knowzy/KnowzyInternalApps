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
                new InventoryModel { InventoryId = "12358132", Engineer = "Ryan Drescher", Name = "Purple nose", RawMaterial = "Purple foam" },
                new InventoryModel { InventoryId = "12358134", Engineer = "Russel Peters", Name = "Patch Adams", RawMaterial = "Red foam" },
                new InventoryModel { InventoryId = "PN3476", Engineer = "Steve Jacobs", Name = "Fabric printed nose", RawMaterial = "Fabric with prints" },
                new InventoryModel { InventoryId = "RN3454", Engineer = "Russel Peters", Name = "Black Nose", RawMaterial = "Black foam" },
                new InventoryModel { InventoryId = "RN3456", Engineer = "Jane Smith", Name = "Deluxe Nose", RawMaterial = "Johan Harris" }
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
