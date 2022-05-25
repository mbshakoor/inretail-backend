using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.ServiceImp
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetCustomerByIdAsync(id);
        }

        public async Task<Customer> GetCustomerByContactNoAsync(string ContactNo, int BranchId)
        {
            return await _customerRepository.GetCustomerByContactNoAsync(ContactNo, BranchId);
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            return await _customerRepository.AddAsync(customer);
        }

        public async Task<Customer> GetCustomerId(string CustomerName, string ContactNo, int BranchId)
        {
            Customer customer = null;
            if (string.IsNullOrEmpty(CustomerName) && string.IsNullOrEmpty(ContactNo))
            {
                customer = await GetDummyCustomerId(BranchId);
            }
            else
            {
                customer = await _customerRepository.GetCustomerByContactNoAsync(ContactNo, BranchId);
                if (customer == null)
                {
                    customer = new Customer();
                    customer.CustomerName = CustomerName;
                    customer.ContactNo = ContactNo;
                    customer.BranchId = BranchId;
                    customer.CreatedOn = DateTime.Now;
                    customer.UpdatedOn = DateTime.Now;

                    customer = await _customerRepository.AddAsync(customer);
                }
            }
            return customer;
        }

        async Task<Customer> GetDummyCustomerId(int BranchId)
        {
            var dummyCustomer = await _customerRepository.GetCustomerByContactNoAsync(ConstHelper.DummyContactNo, BranchId);

            if (dummyCustomer == null)
            {
                Customer customer = new Customer();
                customer.CustomerName = ConstHelper.DummyCustomerName;
                customer.ContactNo = ConstHelper.DummyContactNo;
                customer.BranchId = BranchId;
                customer.CreatedOn = DateTime.Now;
                customer.UpdatedOn = DateTime.Now;

                dummyCustomer = await _customerRepository.AddAsync(customer);
            }

            return dummyCustomer;
        }
    }
}
