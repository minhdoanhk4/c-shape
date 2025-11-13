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

        
    }
}
