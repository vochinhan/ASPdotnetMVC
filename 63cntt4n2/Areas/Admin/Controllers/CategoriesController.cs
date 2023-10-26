using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using UDW.Library;
using System.Diagnostics;
using _63cntt4n2.Library;

namespace _63cntt4n2.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        private MyDBContext db = new MyDBContext();

        // INDEX
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        //Admin/Categories/Status
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo thay đổi status thất bại
                TempData["message"] = new XMessage("danger", "Thay đổi status thất bại!");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Thay đổi status thất bại!");
                return RedirectToAction("Index");
            }
            //Cập nhật một số trường 
            //Update at
            categories.UpdateAt = DateTime.Now;
            //Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //Update database
            categoriesDAO.Update(categories);
            //Thông báo thay đổi status thành công
            TempData["message"] = new XMessage("success", "Thay đổi status thành công!");
            return RedirectToAction("Index");
        }


        // GET: Admin/Categories/Details/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }

            Categories categories = categoriesDAO.getRow(id);

            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            return View(categories);
        }



        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động một số trường
                // Create at
                categories.CreatedAt = DateTime.Now;
                // Update at
                categories.UpdateAt = DateTime.Now;
                //Create By
                categories.CreatedBy = Convert.ToInt32(Session["UserID"]);
                //Update By
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentId
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                { 
                    categories.Order += 1;
                }
                //Thêm mới dòng dữ liệu
                categoriesDAO.Insert(categories);

                //Hiển thị thông báo thêm dữ liệu thành công
                TempData["message"] = new XMessage("success","Thêm mới thành công!");


                return RedirectToAction("Index");
            }

            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");

            return View(categories);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật thất bại!");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);

            if (categories == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Cập nhật thất bại!");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Cập nhật một số trường
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentId
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //Update at
                categories.UpdateAt = DateTime.Now;
                //Update By
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);

                //Update database
                categoriesDAO.Update(categories);

                //Thông báo cập nhật thành công
                TempData["message"] = new XMessage("success", "Cập nhật thành công!");
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thông báo xóa thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo xóa thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }

            return View(categories);
        }

        //POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
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

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            //Cập nhật một số trường 
            //Update at
            categories.UpdateAt = DateTime.Now;
            //Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            categories.Status = 0;
            //Update database
            categoriesDAO.Update(categories);
            //Thông báo thay đổi status thành công
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }

        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thay đổi status thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }

            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }
            //Cập nhật một số trường 
            //Update at
            categories.UpdateAt = DateTime.Now;
            //Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            categories.Status = 2;
            //Update database
            categoriesDAO.Update(categories);
            //Thông báo thay đổi status thành công
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công!");
            return RedirectToAction("Index");
        }


    }
}
