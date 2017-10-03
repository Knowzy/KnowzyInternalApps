using Microsoft.Knowzy.Domain;
using Microsoft.Knowzy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Knowzy.NET.DAL.KnowzyDataSet;

namespace Microsoft.Knowzy.NET.BLL
{
    public static class ProductInventoryMapper
    {
        public static Product ToProduct(this InventoryRow inventoryRow)
        {
            DevelopmentStatus status;
            bool succeeded = Enum.TryParse<DevelopmentStatus>(inventoryRow.Status, true, out status);

            if (!succeeded) status = DevelopmentStatus.Prototype;

            var product = new Product
            {
                Id = inventoryRow.Id,
                Engineer = inventoryRow.Engineer,
                Name = inventoryRow.Name,
                RawMaterial = inventoryRow.RawMaterial,
                Status = status,
                DevelopmentStartDate = inventoryRow.DevelopmentStartDate,
                ExpectedCompletionDate = inventoryRow.ExpectedCompletionDate,
                Notes = inventoryRow.Notes,
                ImageSource = inventoryRow.ImageSource
            };

            return product;
        }

    }
}
