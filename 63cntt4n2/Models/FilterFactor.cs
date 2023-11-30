using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _63cntt4n2.Models
{
    public class FilterFactor
    {
        public string Value { get; set; }
        public string Name { get; set; }

        public FilterFactor(string Value, string Name)
        {
            this.Value = Value;
            this.Name = Name;
        }
    }
}