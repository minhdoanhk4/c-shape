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

    }
}
