using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _63cntt4n2.Models
{
    public enum SortMethod
    {
        AscName,
        AscPrice,
        DescName,
        DescPrice,
    }
    public class FilterFactor
    {
        string Category;
        SortMethod SortBy;
        string ShowNumber;
        string Brand;
        string PriceStart;
        string PriceEnd;
    }
}