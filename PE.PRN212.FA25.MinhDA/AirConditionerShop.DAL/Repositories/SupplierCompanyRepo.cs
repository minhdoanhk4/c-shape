using AirConditionerShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.DAL.Repositories
{
    public class SupplierCompanyRepo
    {
        private AirConditionerShopDbContext _ctx;

        public List<SupplierCompany> GetAll()
        {
            _ctx = new();
            return _ctx.SupplierCompanies.ToList();
        }
    }
    
}
