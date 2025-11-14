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

        public List<Account> GetAllAccount()
        {
            return _repo.GetAll();
        }

        public void CreateAccount(Account obj)
        {
            _repo.Create(obj);
        }


        public void UpdateAccount(Account obj)
        {
            _repo.Update(obj);
        }


        public void DeleteAccount(Account obj)
        {
            _repo.Delete(obj);
        }
    }
}
