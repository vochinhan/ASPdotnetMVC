using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class OrderdetailsDAO
    {
        private MyDBContext db = new MyDBContext();

        public List<Orderdetails> getList()
        {
            return db.Orderdetails.ToList();
        }
        //Insert
        public int Insert(Orderdetails row)
        {
            db.Orderdetails.Add(row);
            return db.SaveChanges();
        }

        //Get row
        public Orderdetails getRow(int? id)
        {
            if (id == null) return null;
            else return db.Orderdetails.Find(id);
        }

        public int Update(Orderdetails row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(Orderdetails row)
        {
            db.Orderdetails.Remove(row);
            return db.SaveChanges();
        }
    }
}
