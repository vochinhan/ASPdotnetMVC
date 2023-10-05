using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Categories")]

    public class Categories
    {
        [Key]
        [Display(Name="Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên loại sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Tên rút gọn")]
        public string Slug { get; set; }

        [Display(Name = "Cấp cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Required]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Required]
        [Display(Name = "")]
        public string MetaKey { get; set; }

        [Display(Name = "Tạo mới bởi")]
        public int CreatedBy { get; set; }

        [Display(Name = "Thời gian tạo mới")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Cập nhật bởi")]
        public int UpdateBy { get; set; }

        [Display(Name = "Thời gian cập nhật")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        public int Status { get; set; }

    }
}
