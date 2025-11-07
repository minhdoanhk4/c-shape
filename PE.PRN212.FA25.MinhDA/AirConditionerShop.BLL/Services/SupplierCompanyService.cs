using AirConditionerShop.DAL.Models;
using AirConditionerShop.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.BLL.Services
{
    public class SupplierCompanyService
    {
        private SupplierCompanyRepo _repo = new();

        public List<SupplierCompany> GetAllSupplier()
        {
            return _repo.GetAll();
        }
    }
}
