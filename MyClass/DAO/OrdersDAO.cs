using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class OrdersDAO
    {
        private MyDBContext db = new MyDBContext();

        public List<Orders> getList()
        {
            return db.Orders.ToList();
        }

        public List<Orders> getList(string status = "All") //Status = 1,2: show; else hide
        {
            List<Orders> list = null;
            switch (status)
            {
                case "Index": //1, 2
                    list = db.Orders.Where(m => m.Status != 0).ToList();
                    break;
                case "Trash": // 0
                    list = db.Orders.Where(m => m.Status == 0).ToList();
                    break;
                default:
                    list = db.Orders.ToList();
                    break;
            }
            return list;
        }


        //Insert
        public int Insert(Orders row)
        {
            db.Orders.Add(row);
            return db.SaveChanges();
        }

        //Get row
        public Orders getRow(int? id)
        {
            if (id == null) return null;
            else return db.Orders.Find(id);
        }

        public int Update(Orders row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(Orders row)
        {
            db.Orders.Remove(row);
            return db.SaveChanges();
        }
    }
}
