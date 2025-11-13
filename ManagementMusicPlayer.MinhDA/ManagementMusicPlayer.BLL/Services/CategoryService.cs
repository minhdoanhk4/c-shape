using ManagementMusicPlayer.DAL.Entities;
using ManagementMusicPlayer.DAL.Reposistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.BLL.Services
{
    public class CategoryService
    {
        public CategoryRepo _repo = new();

        public List<Category> GetAllCategory()
        {
            return _repo.GetAll();
        }

    }
}
