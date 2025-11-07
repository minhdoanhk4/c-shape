using AirConditionerShop.DAL.Models;
using AirConditionerShop.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.BLL.Services
{
    public class AirConditionerService
    {
        // gui/ controller  --> service --> repo --> dbcontext --> table

        //service chứa các hàm cung cấp cho bên gui sài và nó cần repo trợ giúp, chứ ko bay thẳng xuống db context
        // service phải khai báo biến repo

        private AirConditionerRepo _repo = new(); //vì dbcontext đã đc repo  kiểm soát, new thoải mái


        //Hàm 1: GetAllAircons()
        public List<AirConditioner> GetAllAirCons()
        {
            return _repo.GetAll();
        }

        //Hàm 2: CreateAircons()

        public void CreateAirCon(AirConditioner obj)
        {
            _repo.Create(obj);
        }

        //Hàm 3: UpdateAircons()
        public void UpdateAirCon(AirConditioner obj)
        {
            _repo.Update(obj);
        }

        //Hàm 4: DeleteAircons()
        public void DeleteAirCon(AirConditioner obj)
        {
            _repo.Delete(obj);
        }


        //Hàm 5: SearchAircons()
    }
}
