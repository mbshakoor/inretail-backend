using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.RepositoryImp
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(InRetailContext inRetailContext) : base(inRetailContext)
        {
        }

        public Task<Customer> GetCustomerByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Customer> GetCustomerByContactNoAsync(string ContactNo, int BranchId)
        {
            return GetAll().FirstOrDefaultAsync(x => x.ContactNo == ContactNo && x.BranchId == BranchId);
        }
    }
}
