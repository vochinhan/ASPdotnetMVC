using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using _63cntt4n2.Library;
using MyClass.DAO;
using MyClass.Model;
using UDW.Library;

namespace _63cntt4n2.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        ProductsDAO productsDAO = new ProductsDAO();
        MenusDAO menusDAO = new MenusDAO();
        /// /////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Index
        public ActionResult Index()
        {
            ViewBag.CatList = categoriesDAO.getList("Index");
            ViewBag.SupList = suppliersDAO.getList("Index");
            ViewBag.ProList = productsDAO.getList("Index");
            return View(menusDAO.getList("Index"));
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //Them Loai san pham
            if (!string.IsNullOrEmpty(form["ThemCategory"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameCategory"]))
                {
                    var listitem = form["nameCategory"];
                    //chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Categories categories = categoriesDAO.getRow(id);
                        //ta ra menu
                        Menus menu = new Menus();
                        menu.Name = categories.Name;
                        menu.Link = categories.Id.ToString();
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Order = 0;
                        menu.CreatedAt = DateTime.Now;
                        menu.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục loại sản phẩm");
                }
            }

            //Them Nha cung cap
            if (!string.IsNullOrEmpty(form["ThemSupplier"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameSupplier"]))
                {
                    var listitem = form["nameSupplier"];
                    //chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Suppliers suppliers = suppliersDAO.getRow(id);
                        //ta ra menu
                        Menus menu = new Menus();
                        menu.Name = suppliers.Name;
                        menu.Link = suppliers.Id.ToString();
                        menu.TypeMenu = "supplier";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Order = 0;
                        menu.CreatedAt = DateTime.Now;
                        menu.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn nhà cung cấp");
                }
            }

            //Them San pham
            if (!string.IsNullOrEmpty(form["ThemProduct"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameProduct"]))
                {
                    var listitem = form["nameProduct"];
                    //chuyen danh sach thanh dang mang: 1,2,3,4...
                    var listarr = listitem.Split(',');//ngat mang thanh tung phan tu cach nhau boi dau ,
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);//ep kieu int
                        //lay 1 ban ghi
                        Products products = productsDAO.getRow(id);
                        //ta ra menu
                        Menus menu = new Menus();
                        menu.Name = products.Name;
                        menu.Link = (products.Id).ToString();
                        menu.TypeMenu = "product";
                        menu.Position = form["Position"];
                        menu.ParentId = 0;
                        menu.Order = 0;
                        menu.CreatedAt = DateTime.Now;
                        menu.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.UpdateAt = DateTime.Now;
                        menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                        menu.Status = 2; //tam thoi chua xuat ban
                        //Them vao DB
                        menusDAO.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn sản phẩm");
                }
            }

            //Them Custom
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                //kiem tra dau check cua muc con
                if (!string.IsNullOrEmpty(form["nameCustom"]) && !string.IsNullOrEmpty(form["linkCustom"]))
                {
                    //tao ra menu custom
                    Menus menu = new Menus();
                    menu.Name = form["nameCustom"];
                    menu.Link = form["linkCustom"];
                    menu.TypeMenu = "custom";
                    menu.Position = form["Position"];
                    menu.ParentId = 0;
                    menu.Order = 0;
                    menu.CreatedAt = DateTime.Now;
                    menu.CreatedBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.UpdateAt = DateTime.Now;
                    menu.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());
                    menu.Status = 2; //tam thoi chua xuat ban
                    //Them vao DB
                    menusDAO.Insert(menu);

                    TempData["message"] = new XMessage("success", "Thêm vào menu thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa đầy đủ thông tin cho menu");
                }
            }

            //tra ve trang Index
            return RedirectToAction("Index", "Menu");
        }

        ////////////////////////////////////////////////////////////////
        //GET: Admin/Menu/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //truy van dong co id = id yeu cau
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyen doi trang thai cua Satus tu 1<->2
                menus.Status = (menus.Status == 1) ? 2 : 1;

                //cap nhat gia tri UpdateAt
                menus.UpdateAt = DateTime.Now;

                //cap nhat lai DB
                menusDAO.Update(menus);

                //thong bao cap nhat trang thai thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");

                return RedirectToAction("Index");
            }
        }


        ////////////////////////////////////////////////////////////////
        // GET: Admin/Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

        // GET: Admin/Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TableID,TypeMenu,Position,Link,ParentID,Order,CreateAt,CreateBy,UpdateAt,UpdateBy,Status")] Menus menus)
        {
            if (ModelState.IsValid)
            {
                menusDAO.Insert(menus);
                return RedirectToAction("Index");
            }

            return View(menus);
        }

        // GET: Admin/Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.PositionList = new List<SelectListItem>
            {
                new SelectListItem {Value = "MainMenu", Text = "MainMenu"},
                new SelectListItem {Value = "Footer", Text = "Footer"}
            };

            ViewBag.TypeMenuList = new List<SelectListItem>
            {
                new SelectListItem {Value = "category", Text = "category"},
                new SelectListItem {Value = "product", Text = "product"},
                new SelectListItem {Value = "supplier", Text = "supplier"},
                new SelectListItem {Value = "custom", Text = "custom"}
            };

            ViewBag.ParentIDList = new SelectList(menusDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(menusDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                return HttpNotFound();
            }
            return View(menus);
        }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(Menus menus)
            {
                if (ModelState.IsValid)
                {
                    //Update tu dong 1 so truong 

                    if (menus.TypeMenu != "custom")
                    menus.Link = productsDAO.getRow(menus.Id).Id.ToString(); ///////////////////////////
                    menus.ParentId = 0;
                    menus.UpdateAt = DateTime.Now;
                    menus.UpdateBy = Convert.ToInt32(Session["UserID"].ToString());

                    menusDAO.Update(menus);
                    TempData["message"] = new XMessage("success", "Chỉnh sửa menu thành công");
                    return RedirectToAction("Index");
                }

                TempData["message"] = new XMessage("danger", "Chỉnh sửa menu thất bại");
                return View(menus);
            }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo xóa thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }

            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Thông báo xóa thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }

            return View(menus);
        }

        //POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menus menus = menusDAO.getRow(id);
            menusDAO.Delete(menus);
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            return RedirectToAction("Trash");
        }

        //Chuyển mẫu tin ở trạng thái status 1,2 thành 0 ( không hiển thị ở trang Index )
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thay đổi status thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }

            Menus menus= menusDAO.getRow(id);
            if (menus == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            //Cập nhật một số trường 
            //Update at
            menus.UpdateAt = DateTime.Now;
            //Update By
            menus.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            menus.Status = 0;
            //Update database
            menusDAO.Update(menus);
            //Thông báo thay đổi status thành công
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            return View(menusDAO.getList("Trash"));
        }

        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thay đổi status thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }

            Menus menus = menusDAO.getRow(id);
            if (menus == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }
            //Cập nhật một số trường 
            //Update at
            menus.UpdateAt = DateTime.Now;
            //Update By
            menus.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            menus.Status = 2;
            //Update database
            menusDAO.Update(menus);
            //Thông báo thay đổi status thành công
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công!");
            return RedirectToAction("Index");
        }
    }
}
