using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _63cntt4n2.Models
{
    public class CartItem
    {
        public int productId { get; set; }
        public int qty { get; set; }

        public CartItem(int id, int qty)
        {
            this.productId = id;
            this.qty = qty;
        }

        public CartItem()
        { }
    }
}