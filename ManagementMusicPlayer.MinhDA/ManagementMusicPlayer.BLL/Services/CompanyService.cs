using ManagementMusicPlayer.DAL.Entities;
using ManagementMusicPlayer.DAL.Reposistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.BLL.Services
{
    public class CompanyService
    {
        private CompanyRepo _repo = new();

        public List<Company> GetAllCompany()
        {
            return _repo.GetAll();
        }

        public void CreateCompany(Company obj)
        {
            _repo.Create(obj);
        }

        //Hàm 3: UpdateAircons()
        public void UpdateCompany(Company obj)
        {
            _repo.Update(obj);
        }

        //Hàm 4: DeleteAircons()
        public void DeleteCompany(Company obj)
        {
            _repo.Delete(obj);
        }


        //Hàm 5: SearchAircons()
    }
}
