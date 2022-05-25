using InRetailDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.IService
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> GetCustomerByContactNoAsync(string ContactNo, int BranchId);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerId(string CustomerName, string ContactNo, int BranchId);
    }
}
