using MyClass.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace _63cntt4n2.Controllers
{
    public class ModuleController : Controller
    {
        MenusDAO menusDAO = new MenusDAO();
        // GET: Module
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult MainMenu()
        {
            return PartialView(menusDAO.getListByParentId(0));
        }
    }
}