using ManagementMusicPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.DAL.Reposistories
{
    public class CompanyRepo
    {
        private ManagementMusicPlayerContext _ctx;

        public List<Company> GetAll()
        {
            _ctx = new();
            return _ctx.Companies.ToList();
        }

        public void Create(Company obj)
        {
            _ctx = new();
            _ctx.Companies.Add(obj);
            _ctx.SaveChanges();

        }

        //Hàm 3: Update aircon set cột x   = value mới where key =? -> Update
        public void Update(Company obj)
        {
            _ctx = new();
            _ctx.Companies.Update(obj);
            _ctx.SaveChanges();

        }
       
        public void Delete(Company obj)
        {
            _ctx = new();
            _ctx.Companies.Remove(obj);
            _ctx.SaveChanges();
        }
    }
}
