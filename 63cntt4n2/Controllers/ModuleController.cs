using MyClass.DAO;
using MyClass.Model;
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
        public ActionResult MainMenu()
        {
            var ls = new List<Menus>();

            ls = menusDAO.getListByParentId(0);

            return View(ls);
        }
    }
}