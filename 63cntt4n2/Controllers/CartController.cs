using MyClass.DAO;
using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using _63cntt4n2.Models;
using System.Web.Services.Protocols;

namespace _63cntt4n2.Controllers
{
    public class CartController : Controller
    {
        ProductsDAO productDAO = new ProductsDAO();
        CategoriesDAO categoryDAO = new CategoriesDAO();
        public ActionResult Index()
        {
            var cart = (List<CartItem>)Session["Cart"];
            return View(cart);
        }
        // GET: Cart
        [HttpGet]
        public ActionResult AddAtDetail(string id, string qty)
        {
            if (Session["Cart"] != null)
            {
                var cart = (List<CartItem>)Session["Cart"];
                bool exist = false;
                //Check if the item already exist in cart 
                foreach (var item in cart)
                {
                    if (item.productId == int.Parse(id))
                    {
                        exist = true;
                        item.qty += int.Parse(qty);
                        break;
                    }
                }
                if (!exist) cart.Add(new CartItem(int.Parse(id), int.Parse(qty)));
                Session["Cart"] = cart;
            }
            return RedirectToAction("ProductDetails", "Site", new {id = id});
        }

        public ActionResult AddAtItem(string id)
        {
            Products model = productDAO.getRow(int.Parse(id));
            if (Session["Cart"] != null)
            {
                var cart = (List<CartItem>)Session["Cart"];
                bool exist = false;
                //Check if the item already exist in cart 
                foreach (var item in cart)
                {
                    if (item.productId == model.Id)
                    {
                        exist = true;
                        item.qty++;
                        break;
                    }
                }
                //If not
                if (!exist) cart.Add(new CartItem(model.Id,1));
                Session["Cart"] = cart;
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ContentResult Number()
        {
            var Cart = (List<CartItem>)Session["Cart"];
            string number = Cart.Count().ToString();
            return Content(number);
        }

        public ActionResult Clear() 
        {
            Session["Cart"] = new List<CartItem>();
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult CartItem(CartItem model)
        {
            var product = productDAO.getRow(model.productId);
            ViewBag.CatName = categoryDAO.getRow(product.CatID).Name;
            ViewBag.SellQty = model.qty;
            ViewBag.Qty = product.Qty;
            return View(product);
        }

        public ActionResult Test()
        {
            var Cart = (List<CartItem>)Session["Cart"];
            return View(Cart);
        }

        [HttpPost]
        public ActionResult CheckOutHandle(string[] quantity)
        {
            if (Session["UserID"].ToString() == "-1") return RedirectToAction("Login", "Site"); // check whether logged in or not
            var Cart = (List<CartItem>)Session["Cart"];
            for (int i = 0; i < Cart.Count(); i++)
            {
                Cart[i].qty = int.Parse(quantity[i]);
            }

            Session["Cart"] = Cart;
            return RedirectToAction("CheckOut");
        }

        public ActionResult CheckOut()
        {
            var Cart = (List<CartItem>)Session["Cart"];

            if (Cart.Count() == 0) return RedirectToAction("Index", "Cart");
            var NameList = new List<string>();
            var TotalList = new List<string>();
            int subTotal = 0;
            foreach (var item in Cart)
            {
                var product = productDAO.getRow(item.productId);
                NameList.Add(product.Name);
                int total = item.qty * (int)product.PriceSale;
                subTotal += total;
                TotalList.Add(total.ToString());
            }
            //Handle ship cost
            int shipCost = 0;


            ViewBag.NameList = NameList;
            ViewBag.TotalList = TotalList;
            ViewBag.SubTotal = subTotal;
            ViewBag.Ship = "";
            ViewBag.FinalTotal = subTotal + shipCost;
            return View(Cart);
        }
        OrdersDAO orderDAO = new OrdersDAO();
        OrderdetailsDAO orderdetailsDAO = new OrderdetailsDAO();

        [HttpPost]
        public ActionResult OrderConfirmation(FormCollection form)
        {
            string billinglastname = "";
            string billingfirstname = "";
            string shippinglastname = "";
            string shippingfirstname = "";
            string billingphone = "";
            string shippingphone = "";
            string billingaddress = "";
            string note = "";
            string shippingaddress = "";
            if (!string.IsNullOrEmpty(form["billinglastname"]))
            {
                billinglastname = form["billinglastname"];
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
            if (!string.IsNullOrEmpty(form["billingfirstname"]))
            {
                billingfirstname = form["billingfirstname"];
            }

            if (!string.IsNullOrEmpty(form["shippinglastname"]))
            {
                shippinglastname = form["shippinglastname"];
            }
            if (!string.IsNullOrEmpty(form["shippingfirstname"]))
            {
                shippingfirstname = form["shippingfirstname"];
            }
            if (!string.IsNullOrEmpty(form["shippingphone"]))
            {
                shippingphone = form["shippingphone"];
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
            if (!string.IsNullOrEmpty(form["billingphone"]))
            {
                billingphone = form["billingphone"];
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
            if (!string.IsNullOrEmpty(form["shippingaddress"]))
            {
                shippingaddress = form["shippingaddress"];
            }
            else
            {
                return RedirectToAction("CheckOut");
            }
            if (!string.IsNullOrEmpty(form["billingaddress"]))
            {
                billingaddress = form["billingaddress"];
            }

            if (!string.IsNullOrEmpty(form["note"]))
            {
                note = form["note"];
            }

            Orders order = new Orders();
            if (orderDAO.getList().Count() == 0)
            {
                order.Id = 1;
            }
            else
            {
                int largestId = orderDAO.getList().OrderByDescending(item => item.Id).FirstOrDefault().Id;
                order.Id = largestId + 1;
            }
            order.Note = note;
            order.ReceiverAddress = shippingaddress;
            order.ReceiverId = Session["UserID"].ToString();
            order.UserId = int.Parse(Session["UserID"].ToString());
            order.ReceiverPhone = shippingphone;
            order.CreatedAt = DateTime.Now;
            order.Status = 1;
            orderDAO.Insert(order);
            var Cart = (List<CartItem>)Session["Cart"];

            foreach (var item in Cart)
            {
                Orderdetails details = new Orderdetails();
                details.OrderId = order.Id;
                details.ProductId = item.productId;
                details.Qty = item.qty;
                details.Price = productDAO.getRow(item.productId).PriceSale;
                orderdetailsDAO.Insert(details);
            }

            ViewBag.BillingLastName = billinglastname;
            ViewBag.BillingFirstName = billingfirstname;
            ViewBag.ShippingLastName = shippinglastname;
            ViewBag.ShippingFirstName = shippingfirstname;
            ViewBag.BillingPhone =  billingphone;
            ViewBag.ShippingPhone =  shippingphone;
            ViewBag.BillingAddress =  billingaddress ;
            ViewBag.ShippingAddress =  shippingaddress ;


            var NameList = new List<string>();
            var TotalList = new List<string>();
            int subTotal = 0;
            foreach (var item in Cart)
            {
                var product = productDAO.getRow(item.productId);
                NameList.Add(product.Name);
                int total = item.qty * (int)product.PriceSale;
                subTotal += total;
                TotalList.Add(total.ToString());
            }
            //Handle ship cost
            int shipCost = 0;


            ViewBag.NameList = NameList;
            ViewBag.TotalList = TotalList;
            ViewBag.SubTotal = subTotal;
            ViewBag.Ship = "";
            ViewBag.FinalTotal = subTotal + shipCost;
            ViewBag.Date = DateTime.Now;
            ViewBag.OrderId = order.Id;
            return View(Cart);
        }
    }
}