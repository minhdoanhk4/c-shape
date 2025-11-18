using CarManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManagement.DAL.Repositories
{
    public class CarRepo
    {
        private ManagementCarDbContext? _ctx;

        //      <<<<  Hàm GET_ALL => show lên grid   >>>>
        public List<Car> GetAll()
        {
            _ctx = new();
            return _ctx.Cars.Include("Brand").Include("CarType").ToList();
        }

        //      <<<<  Hàm CREATE => lưu vào DB   >>>>
        public void Create(Car obj)
        {
            _ctx = new();
            _ctx.Cars.Add(obj);
            _ctx.SaveChanges();

        }

        //      <<<<  Hàm UPDATE => lưu vào DB   >>>>
        public void Update(Car obj)
        {
            _ctx = new();
            _ctx.Cars.Update(obj);
            _ctx.SaveChanges();

        }

        //      <<<<  Hàm REMOVE => lưu vào DB   >>>>
        public void Delete(Car obj)
        {
            _ctx = new();
            _ctx.Cars.Remove(obj);
            _ctx.SaveChanges();

        }

        //      <<<<  Hàm SEARCH => lưu vào DB   >>>>
        public List<Car> Search(string keyword, string filterType = "All")
        {
            _ctx = new();

            // 1. Nếu từ khóa rỗng, trả về tất cả
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAll();
            }

            string key = keyword.ToLower();

            // 2. Chuẩn bị câu truy vấn cơ bản (chưa chạy xuống DB)
            var query = _ctx.Cars
                            .Include("Brand")
                            .Include("CarType");

            // 3. Kiểm tra ComboBox đang chọn cái gì để filter tương ứng
            switch (filterType)
            {
                case "Car Model": // Tìm theo tên xe
                    return query.Where(c => c.CarModel.ToLower().Contains(key)).ToList();

                case "Brand": // Tìm theo tên Hãng
                              // Dùng Brand.BrandName dựa trên file Brand.cs
                    return query.Where(c => c.Brand != null && c.Brand.BrandName.ToLower().Contains(key)).ToList();

                case "Car Type": // Tìm theo tên Loại xe
                                 // Dùng Type.TypeName dựa trên file CarType.cs
                    return query.Where(c => c.CarType != null && c.CarType.TypeName.ToLower().Contains(key)).ToList();

                case "Car Engine": // Tìm theo Động cơ
                    return query.Where(c => c.CarEngine != null && c.CarEngine.ToLower().Contains(key)).ToList();

                default: // "All" hoặc null -> Tìm trên TẤT CẢ các trường
                    return query.Where(c =>
                        (c.CarModel != null && c.CarModel.ToLower().Contains(key)) ||
                        (c.CarEngine != null && c.CarEngine.ToLower().Contains(key)) ||
                        (c.Brand != null && c.Brand.BrandName.ToLower().Contains(key)) ||
                        (c.CarType != null && c.CarType.TypeName.ToLower().Contains(key))
                    ).ToList();
            }
        }

    }
}
