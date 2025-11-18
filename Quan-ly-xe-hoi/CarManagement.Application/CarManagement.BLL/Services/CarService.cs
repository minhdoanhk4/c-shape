using CarManagement.DAL.Entities;
using CarManagement.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.BLL.Services
{
    public class CarService
    {
        private CarRepo _repo = new();

        //Hàm 1: GetAllCar()
        public List<Car> GetAllCar()
        {
            return _repo.GetAll();
        }

        //Hàm 2: CreateCar()

        public void CreateCar(Car obj)
        {
            _repo.Create(obj);
        }

        //Hàm 3: UpdateCar()
        public void UpdateCar(Car obj)
        {
            _repo.Update(obj);
        }

        //Hàm 4: DeleteCar()
        public void DeleteCar(Car obj)
        {
            _repo.Delete(obj);
        }


        //Hàm 5: SearchAircons()
        public List<Car> SearchCar(string keyword, string filterType)
        {
            return _repo.Search(keyword, filterType);
        }
    }
}
