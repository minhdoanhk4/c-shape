using ManagementMusicPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.DAL.Reposistories
{
    public class CategoryRepo
    {
        private ManagementMusicPlayerContext _ctx;

        public List<Category> GetAll()
        {
            _ctx = new();
            return _ctx.Categories.ToList();
        }
    }
}
