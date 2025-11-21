using AppCar.MinhDA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCar.MinhDA.DAL.Repositories
{
    public class BrandRepo
    {
        private AppCarManagementDBcontext? _ctx;

        public List<Brand> GetBrands()
        {
            _ctx = new();
            return _ctx.Brands.ToList();
        }
    }
}
