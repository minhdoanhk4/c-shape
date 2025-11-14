using ManagementMusicPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.DAL.Reposistories
{
    public class AccountRepo
    {
        private ManagementMusicPlayerContext _ctx;

        public List<Account> GetAll()
        {
            _ctx = new();
            return _ctx.Accounts.ToList();
        }
        public void Create(Account obj)
        {
            _ctx = new();
            _ctx.Accounts.Add(obj);
            _ctx.SaveChanges();

        }

        //Hàm 3: Update aircon set cột x   = value mới where key =? -> Update
        public void Update(Account obj)
        {
            _ctx = new();
            _ctx.Accounts.Update(obj);
            _ctx.SaveChanges();

        }

        public void Delete(Account obj)
        {
            _ctx = new();
            _ctx.Accounts.Remove(obj);
            _ctx.SaveChanges();
        }

        //Hàm 5: select * from aircon where cột = like '%  %' -> search
    }
}
