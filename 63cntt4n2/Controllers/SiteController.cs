using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using _63cntt4n2.Areas.Admin.Controllers;
using _63cntt4n2.Library;
using _63cntt4n2.Models;
using MyClass.DAO;
using MyClass.Model;

namespace _63cntt4n2.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        ProductsDAO productDAO = new ProductsDAO();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ProductsDAO productDAO = new ProductsDAO();
            Products product = new Products();
            product = productDAO.getRow(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }
            //Get category name
            CategoriesDAO categoryDAO = new CategoriesDAO();
            ViewBag.CatName = categoryDAO.getRow(product.CatID).Name;

            //Product availability base on quantity
            if (product.Qty > 0)
            {
                ViewBag.Avail = "Còn hàng";
            }
            else
            {
                ViewBag.Avail = "Hết hàng";
            } 
                
            return View(product);
        }

        public ActionResult ShopCategory()
        {
            ProductsDAO productDAO = new ProductsDAO();
            var ls = productDAO.getList("Index");
            return View(ls);
        }

        [HttpGet]
        public ActionResult ShopCategory(FormCollection form)
        {
            var ls = productDAO.getList("Avail");
            return View(ls);
        }

        public ActionResult ProductItem(Products model)
        {
            return View(model);
        }

        public ActionResult TopProducts()
        {
            var ls = productDAO.getList("Avail");
            return View(ls);
        }

        public ActionResult SmallProductItem(Products model)
        {
            return View(model);
        }

        public ActionResult RelatedProducts(int id, string cate)
        {
            var ls = productDAO.getList("Avail");
            ls = ls.Where(item => item.Supplier == cate && item.Id != id).ToList();
            return View("TopProducts", ls);
        }
    }
}