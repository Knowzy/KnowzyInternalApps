using Microsoft.Knowzy.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.NET.BLL
{
    public class SqlDataProvider : IDataProvider
    {
        public Product[] GetData()
        {
            var inventoryRowArray = InventoryBLL.Current.GetInventory().ToArray();

            List<Product> products = new List<Product>();

            foreach(var inventoryRow in inventoryRowArray)
            {
                var product = inventoryRow.ToProduct();
                products.Add(product);
            }

            return products.ToArray();
        }

        public void SetData(Product[] products)
        {
            foreach(var product in products)
            {
                InventoryBLL.Current.UpdateInventory(
                    product.Id, 
                    product.Engineer, 
                    product.Name, 
                    product.RawMaterial, 
                    product.Status.ToString(), 
                    product.DevelopmentStartDate, 
                    product.ExpectedCompletionDate, 
                    product.SupplyManagementContact, 
                    product.Notes, 
                    product.ImageSource);
            }
        }
    }
}
