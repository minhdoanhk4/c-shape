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

        public void Create(Category obj)
        {
            _ctx = new();
            _ctx.Categories.Add(obj);
            _ctx.SaveChanges();

        }

        //Hàm 3: Update aircon set cột x   = value mới where key =? -> Update
        public void Update(Category obj)
        {
            _ctx = new();
            _ctx.Categories.Update(obj);
            _ctx.SaveChanges();

        }

        public void Delete(Category obj)
        {
            _ctx = new();
            _ctx.Categories.Remove(obj);
            _ctx.SaveChanges();
        }
    }
}
