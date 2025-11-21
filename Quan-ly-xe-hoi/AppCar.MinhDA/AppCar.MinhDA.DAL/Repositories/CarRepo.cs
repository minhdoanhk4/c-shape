using AppCar.MinhDA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCar.MinhDA.DAL.Repositories
{
    public class CarRepo
    {
        private AppCarManagementDBcontext _ctx;

        //      <<<<  Hàm GET_ALL => show lên grid   >>>>
        public List<Car> GetAll()
        {
            _ctx = new();
            return _ctx.Cars.Include("Brand").Include("CarType").ToList();
        }

        //      <<<<  Hàm CREATE => lưu vào DB   >>>>
        public void Create(Car obj)
        {
            _ctx = new();
            _ctx.Cars.Add(obj);
            _ctx.SaveChanges();

        }

        //      <<<<  Hàm UPDATE => lưu vào DB   >>>>
        public void Update(Car obj)
        {
            _ctx = new();
            _ctx.Cars.Update(obj);
            _ctx.SaveChanges();

        }

        //      <<<<  Hàm REMOVE => lưu vào DB   >>>>
        public void Delete(Car obj)
        {
            _ctx = new();
            _ctx.Cars.Remove(obj);
            _ctx.SaveChanges();

        }
    }
}
