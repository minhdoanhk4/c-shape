using AppCar.MinhDA.DAL.Models;
using AppCar.MinhDA.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCar.MinhDA.BLL.Services
{
    public class CarTypeService
    {
        private CarTypeRepo _repo = new();

        public List<CarType> GetAllType()
        {
            return _repo.GetCarTypes();
        }
    }
}
