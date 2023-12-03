using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;
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
        CategoriesDAO categoryDAO = new CategoriesDAO();
        SuppliersDAO supplierDAO = new SuppliersDAO();
        OrderdetailsDAO orderdetailDAO = new OrderdetailsDAO();

        public ActionResult Index()
        {
            ViewBag.Trending = productDAO.getList("Avail");
            ViewBag.Top = productDAO.getList("Avail");
            return View();
        }

        [HttpGet]
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Products product = new Products();
            product = productDAO.getRow(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }
            //Get category name
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

        [HttpGet]
        public ActionResult ShopCategory()
        {
            ViewBag.CatList = categoryDAO.getList("Avail");
            ViewBag.SupplierList = supplierDAO.getList("Avail");

            ViewBag.SortType = new List<FilterFactor>
            {
                new FilterFactor("NameASC","Tên tăng dần"),
                new FilterFactor("NameDESC","Tên giảm dần"),
                new FilterFactor("PriceASC","Giá tăng dần"),
                new FilterFactor("PriceDESC","Giá giảm dần")
            };

            ViewBag.RecommendType = new List<FilterFactor>
            {
                new FilterFactor("BestSeller","Bán chạy"),
                new FilterFactor("Recommended","Đề xuất"),
                new FilterFactor("New","Sản phẩm mới"),
            };

            //Handle category filter
            string cateOption = "";
            string suppOption = "";
            var ls = productDAO.getList("Avail");
            if (!string.IsNullOrEmpty(Request.QueryString["category"]) && Request.QueryString["category"] != "-1")
            {
                cateOption = Request.QueryString["category"];
                ViewBag.CategoryOption = cateOption;
                ls = ls.Where(item => item.CatID == int.Parse(cateOption)).ToList();
            }
            else ViewBag.CategoryOption = "-1";

            //Handle supplier filter
            if (!string.IsNullOrEmpty(Request.QueryString["supplier"]) && Request.QueryString["supplier"] != "-1")
            {
                suppOption = Request.QueryString["supplier"];
                ViewBag.SupplierOption = suppOption;
                ls = ls.Where(item => item.Supplier == suppOption).ToList();
            }
            else ViewBag.SupplierOption = "-1";

            //Handle sort type
            if (!string.IsNullOrEmpty(Request.QueryString["sort"]))
            {
                ViewBag.sortOption = Request.QueryString["sort"];
                switch (Request.QueryString["sort"])
                {
                    case "NameASC":
                        ls = ls.OrderBy(item => item.Name).ToList();
                        break;
                    case "NameDESC":
                        ls = ls.OrderByDescending(item => item.Name).ToList();
                        break;
                    case "PriceASC":
                        ls = ls.OrderBy(item => item.PriceSale).ToList();
                        break;
                    case "PriceDESC":
                        ls = ls.OrderByDescending(item => item.PriceSale).ToList();
                        break;
                }
            }
            else ViewBag.sortOption = "NameASC";

            //Handle price range filter
            int lowerprice = 0, upperprice = 100000;
            if (!string.IsNullOrEmpty(Request.QueryString["lowerprice"]) && !string.IsNullOrEmpty(Request.QueryString["upperprice"]))
            {
                lowerprice = int.Parse(Request.QueryString["lowerprice"]);
                upperprice = int.Parse(Request.QueryString["upperprice"]);
                ls = ls.Where(item => item.PriceSale >= lowerprice && item.PriceSale <= upperprice).ToList();
            }
            ViewBag.lowerprice = lowerprice;
            ViewBag.upperprice = upperprice;

            //Handle Recommend
            ViewBag.recommendOption = "Recommended";

            //Handle Search
            if (!string.IsNullOrEmpty(Request.QueryString["search"]))
            {
                string search = Request.QueryString["search"].ToLower();
                ViewBag.search = search;

                foreach (var item in ls)
                {
                    var catName = categoryDAO.getRow(item.CatID).Name;
                    var suppName = supplierDAO.getRow(int.Parse(item.Supplier)).Name;
                    string ItemStr = item.Id + item.Name + catName + suppName + item.Slug.Replace("-", "") + item.MetaKey;
                    ItemStr = ItemStr.Replace(" ", "").ToLower();
                    ItemStr = VietnameseAccentRemove.VietnameseConvert(ItemStr);
                    search = VietnameseAccentRemove.VietnameseConvert(search);
                    search = search.Replace(" ", "");
                    if (ItemStr.IndexOf(search) == -1)
                    {
                        item.Status = 0;
                    }
                }

                ls.RemoveAll(item => item.Status == 0);
            }
            return View(ls);
        }

        public ActionResult ProductItem(Products model)
        {
            ViewBag.CatName = categoryDAO.getRow(model.CatID).Name;
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

        public ActionResult RelatedProducts(int id, int cate, string supplier)
        {
            var ls = productDAO.getList("Avail");
            ls = ls.Where(item => (item.Supplier == supplier || item.CatID == cate) && item.Id != id).ToList();
            return View("TopProducts", ls);
        }

        public ContentResult CategoryQty(string id)
        {
            string number;
            if (id == "all") number = productDAO.getList("Avail").Count().ToString();
            else number = productDAO.getList("Avail").Where(item => item.CatID == int.Parse(id)).ToList().Count().ToString();
            return Content(number);
        }

        public ContentResult SupplierQty(string id)
        {
            string number;
            if (id == "all") number = productDAO.getList("Avail").Count().ToString();
            else number = productDAO.getList("Avail").Where(item => item.Supplier == id).ToList().Count().ToString();
            return Content(number);
        }

        public ActionResult Login()
        {
            ViewBag.Message = "You have to login to check out";
            return View();
        }

        UsersDAO userDAO = new UsersDAO();
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var UserList = userDAO.getList("ALl");
            foreach (var user in UserList)
            {
                if (user.Username == username && user.Password == password)
                {
                    Session["UserID"] = user.Id;
                    if (user.Role == "admin")
                        return RedirectToAction("Index", "Dashboard", new {area = "Admin"}); //admin
                    return RedirectToAction("Index", "Cart"); //user
                }
            }
            ViewBag.Message = "Username or password is invalid";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            string fullname = "";
            string password = "";
            string username = "";
            string phone = "";
            string email = "";
            string confirmPassword = "";

            if (!string.IsNullOrEmpty(form["fullname"]))
            {
                fullname = form["fullname"];
            }
            else return View();

            if (!string.IsNullOrEmpty(form["username"]))
            {
                username = form["username"];
            }
            else return View();

            if (!string.IsNullOrEmpty(form["password"]))
            {
                password = form["password"];
            }
            else return View();

            if (!string.IsNullOrEmpty(form["confirmPassword"]))
            {
                confirmPassword = form["confirmPassword"];
            }
            else return View();

            if (!string.IsNullOrEmpty(form["email"]))
            {
                email = form["email"];
            }
            else return View();

            if (!string.IsNullOrEmpty(form["phone"]))
            {
                phone = form["phone"];
            }
            else return View();

            if (password != confirmPassword)
            {
                return View();
            }

            Users user = new Users();

            if (userDAO.getList().Count() == 0)
            {
                user.Id = 1;
            }
            else
            {
                int largestId = userDAO.getList().OrderByDescending(item => item.Id).FirstOrDefault().Id;
                user.Id = largestId + 1;
            }

            user.Username = username;
            user.Password = password;
            user.Phone = phone;
            user.Email = email;
            user.FullName = fullname;
            user.CreatedAt = DateTime.Now;
            user.CreatedBy = user.Id;
            user.Status = 1;
            user.Role = "user";
            userDAO.Insert(user);

            return View("Login");
        }

        public ActionResult Logout()
        {
            Session["UserID"] = "-1";
            return RedirectToAction("Index", "Site");
        }
    }
}