using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _63cntt4n2.Library;
using MyClass.DAO;
using MyClass.Model;
using UDW.Library;

namespace _63cntt4n2.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        
        // GET: Admin/Supplier
        public ActionResult Index()
        {
            return View(suppliersDAO.getList("Index"));
        }

        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name"); ;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //Xử lý tự động một số trường
                // Create at
                suppliers.CreatedAt = DateTime.Now;
                // Update at
                suppliers.UpdateAt = DateTime.Now;
                //Create By
                suppliers.CreatedBy = Convert.ToInt32(Session["UserID"]);
                //Update By
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                suppliersDAO.Insert(suppliers);
                TempData["message"] = new XMessage("success", "Tạo mới mẫu tin thành công!");
                return RedirectToAction("Index");
            }

            return View(suppliers);
        }

        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name"); ;
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name"); ;
            if (ModelState.IsValid)
            {
                //Xử lý tự động một số trường

                // Update at
                suppliers.UpdateAt = DateTime.Now;
                //Update By
                suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);
                //Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                suppliersDAO.Update(suppliers);
                TempData["message"] = new XMessage("success", "Thay đổi mẫu tin thành công!");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers = suppliersDAO.getRow(id);
            suppliersDAO.Delete(suppliers);
            TempData["message"] = new XMessage("success", "Xóa nhà cung cấp thành công!");
            return RedirectToAction("Index");
        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo thay đổi status thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }

            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Index");
            }
            //Cập nhật một số trường 
            //Update at
            suppliers.UpdateAt = DateTime.Now;
            //Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            suppliers.Status = 0;
            //Update database
            suppliersDAO.Update(suppliers);
            //Thông báo thay đổi status thành công
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công!");
            return RedirectToAction("Index");
        }

        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
        }

        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //Thông báo thay đổi status thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }

            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thông báo thay đổi thất bại
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin!");
                return RedirectToAction("Trash");
            }
            //Cập nhật một số trường 
            //Update at
            suppliers.UpdateAt = DateTime.Now;
            //Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            suppliers.Status = 2;
            //Update database
            suppliersDAO.Update(suppliers);
            //Thông báo thay đổi status thành công
            TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công!");
            return RedirectToAction("Index");
        }
    }
}
