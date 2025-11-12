using ManagementMusicPlayer.DAL.Entities;
using ManagementMusicPlayer.DAL.Reposistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.BLL.Services
{
    public class AccountService
    {
        private AccountRepo _repo = new();

        public List<Account> GetAllAccounts()
        {
            return _repo.GetAll();
        }

        public void CreateAcc(Account obj)
        {
            _repo.Create(obj);
        }

        //Hàm 3: UpdateAircons()
        public void UpdateAcc(Account obj)
        {
            _repo.Update(obj);
        }

        //Hàm 4: DeleteAircons()
        public void DeleteAcc(Account obj)
        {
            _repo.Delete(obj);
        }


        //Hàm 5: SearchAircons()
    }
}
