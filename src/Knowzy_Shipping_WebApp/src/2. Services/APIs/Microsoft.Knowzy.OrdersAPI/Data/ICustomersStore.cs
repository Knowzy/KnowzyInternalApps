using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Knowzy.Domain;

namespace Microsoft.Knowzy.CustomersAPI.Data
{
    public interface ICustomersStore : IDisposable
    {
        Task<bool> Connected();
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomer(string customerId);
        Task<Customer> UpsertCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string customerId);
    }
}