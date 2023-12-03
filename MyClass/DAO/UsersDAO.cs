using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class UsersDAO
    {
        private MyDBContext db = new MyDBContext();

        // INDEX = SELECT * FROM

        public List<Users> getList()
        {
            return db.Users.ToList();
        }

        public List<Users> getList(string status = "All") //Status = 1,2: show; else hide
        {
            List<Users> list = null;
            switch (status)
            {
                case "Index": //1, 2
                    list = db.Users.Where(m => m.Status != 0).ToList();
                    break;
                case "Trash": // 0
                    list = db.Users.Where(m => m.Status == 0).ToList();
                    break;
                case "Avail":
                    {
                        list = db.Users
                            .Where(m => m.Status == 1)
                            .ToList();
                        break;
                    }
                default:
                    list = db.Users.ToList();
                    break;
            }
            return list;
        }


        //Insert
        public int Insert(Users row)
        {
            db.Users.Add(row);
            return db.SaveChanges();
        }

        //Get row
        public Users getRow(int? id)
        {
            if (id == null) return null;
            else return db.Users.Find(id);
        }

        public int Update(Users row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(Users row)
        {
            db.Users.Remove(row);
            return db.SaveChanges();
        }


    }
}
