﻿using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class OrdersDAO
    {
        private MyDBContext db = new MyDBContext();

        public List<Categories> getList()
        {
            return db.Categories.ToList();
        }

        public List<Categories> getList(string status = "All") //Status = 1,2: show; else hide
        {
            List<Categories> list = null;
            switch (status)
            {
                case "Index": //1, 2
                    list = db.Categories.Where(m => m.Status != 0).ToList();
                    break;
                case "Trash": // 0
                    list = db.Categories.Where(m => m.Status == 0).ToList();
                    break;
                default:
                    list = db.Categories.ToList();
                    break;
            }
            return list;
        }


        //Insert
        public int Insert(Categories row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }

        //Get row
        public Categories getRow(int? id)
        {
            if (id == null) return null;
            else return db.Categories.Find(id);
        }

        public int Update(Categories row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(Categories row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }
}