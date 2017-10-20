using Microsoft.Knowzy.NET.DAL.KnowzyDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Knowzy.NET.DAL.KnowzyDataSet;

namespace Microsoft.Knowzy.NET.BLL
{
    public class InventoryBLL
    {
        private InventoryBLL() { }

        private static InventoryBLL current;

        public static InventoryBLL Current => current ?? (current = new InventoryBLL());

        private InventoryTableAdapter inventoryTableAdapter;

        protected InventoryTableAdapter InventoryTableAdapter
        {
            get { return inventoryTableAdapter ?? (inventoryTableAdapter = new InventoryTableAdapter()); }
        }

        public InventoryDataTable GetInventory()
        {
            return InventoryTableAdapter.GetAllInventory();
        }

        public int UpdateInventory(string inventoryId, 
                                    string engineer,
                                    string name,
                                    string rawMaterial,
                                    string status,
                                    DateTime? developmentStartDate,
                                    DateTime? expectedCompletionDate,
                                    string supplyManagementContact,
                                    string notes,
                                    string imageSource)
        {

            var inventoryDataTable = InventoryTableAdapter.GetInventoryByInventoryId(inventoryId);
            InventoryRow inventoryRow;

            bool isNewRecord = false;

            if (inventoryDataTable == null || inventoryDataTable?.Count == 0)
            {
                inventoryDataTable = new InventoryDataTable();
                inventoryRow = inventoryDataTable.NewInventoryRow();
                isNewRecord = true;
            }
            else
            {
                inventoryRow = inventoryDataTable.First();
            }
            
            inventoryRow.Id = inventoryId;
            inventoryRow.Engineer = engineer;
            inventoryRow.Name = name;
            inventoryRow.RawMaterial = rawMaterial;
            inventoryRow.Status = status;
            inventoryRow.SupplyManagementContact = supplyManagementContact;
            inventoryRow.Notes = notes;
            inventoryRow.ImageSource = imageSource;

            inventoryRow.DevelopmentStartDate = developmentStartDate.HasValue ? developmentStartDate.Value : DateTime.Today;
            inventoryRow.ExpectedCompletionDate = expectedCompletionDate.HasValue ? expectedCompletionDate.Value : DateTime.Today.AddMonths(1);

            if (isNewRecord) inventoryDataTable.AddInventoryRow(inventoryRow);

            return InventoryTableAdapter.Update(inventoryDataTable);
        }
    }
}
