using ManagementMusicPlayer.DAL.Entities;
using ManagementMusicPlayer.DAL.Reposistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.BLL.Services
{
    public class MusicPlayerService
    {
        private MusicPlayerRepo _repo = new();

        public List<MusicPlayer> GetAllPlayer()
        {
            return _repo.GetAll();
        }

        public void CreatePlayer(MusicPlayer obj)
        {
            _repo.Create(obj);
        }

        //Hàm 3: UpdateAircons()
        public void UpdatePlayer(MusicPlayer obj)
        {
            _repo.Update(obj);
        }

        //Hàm 4: DeleteAircons()
        public void DeletePlayer(MusicPlayer obj)
        {
            _repo.Delete(obj);
        }


        //Hàm 5: SearchAircons()
    }
}
