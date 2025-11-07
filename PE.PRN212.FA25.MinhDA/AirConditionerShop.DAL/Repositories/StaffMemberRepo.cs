using AirConditionerShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.DAL.Repositories
{
    public class StaffMemberRepo
    {
        private AirConditionerShopDbContext _ctx;

        public StaffMember? FindByEmail(string email)
        {
            _ctx = new();
            //return _ctx.StaffMembers.ToList();
            return _ctx.StaffMembers.FirstOrDefault(nt => nt.EmailAddress == email);
        }

        public StaffMember? FindByEmailAndPassword(string email, string password)
        {
            return _ctx.StaffMembers.FirstOrDefault(nt => nt.EmailAddress == email && nt.Password == password);
        }
    }
    }

