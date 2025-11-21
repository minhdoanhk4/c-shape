using AppCar.MinhDA.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCar.MinhDA.DAL.Repositories
{
    public class CarTypeRepo
    {
        private AppCarManagementDBcontext? _ctx;

        public List<CarType> GetCarTypes()
        {
            _ctx = new();
            return _ctx.CarTypes.ToList();
        }
    }
}
