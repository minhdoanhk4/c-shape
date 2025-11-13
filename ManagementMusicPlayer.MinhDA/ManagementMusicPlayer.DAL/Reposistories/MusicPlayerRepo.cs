using ManagementMusicPlayer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementMusicPlayer.DAL.Reposistories
{
    public class MusicPlayerRepo
    {
        private ManagementMusicPlayerContext _ctx;

        public List<MusicPlayer> GetAll()
        {
            _ctx = new();
            return _ctx.MusicPlayers.Include("Company").Include("Category").ToList();
            
        }


        public void Create(MusicPlayer obj)
        {
            _ctx = new();
            _ctx.MusicPlayers.Add(obj);
            _ctx.SaveChanges();

        }

        //Hàm 3: Update aircon set cột x   = value mới where key =? -> Update
        public void Update(MusicPlayer obj)
        {
            _ctx = new();
            _ctx.MusicPlayers.Update(obj);
            _ctx.SaveChanges();

        }

        //Hàm 4: Delete from aircon where  key =? -> Delete
        // các hàm trong repo  thường  đặt tên rất ngắn gọn, vì nó rất gần table, vì table có 4 lệnh basic:
        /* 
        - Insert into Aircon value(...)
        - Update Aircon  set cột  x  = new value, cột y = new value  where cột key = key muốn đổi
        - delete  from  Aircon  where cột key = key muốn xóa
        - select * from (get all)
        - select * from Aircon where key
         */
        // Tên hàm trong repo đăt ngắn gọn giống như lệnh SQL vì nó thao tác trên table
        // tên hàm trong service đặt chi tiết hơn, rõ ràng hơn do nó gần Gui - hướng về user
        //delete ORM chơi theo kiểu CSDL kiểu OOP thì 1. đưa key để xóa / 2. đưa 1 obj để xóa
        public void Delete(int key)
        {


        }

        public void Delete(MusicPlayer obj)
        {
            _ctx = new();
            _ctx.MusicPlayers.Remove(obj);
            _ctx.SaveChanges();
        }

        //Hàm 5: select * from aircon where cột = like '%  %' -> search
    }
}
