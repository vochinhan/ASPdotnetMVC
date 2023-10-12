﻿using System;
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

        [Required(ErrorMessage = "Tên loại sản phẩm không được để trống")]
        [Display(Name = "Tên loại sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Tên rút gọn")]
        public string Slug { get; set; }

        [Display(Name = "Cấp cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Required(ErrorMessage = "Từ khóa")]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        [Display(Name = "Tạo mới bởi")]
        public int CreatedBy { get; set; }

        [Required(ErrorMessage = "Ngày cập nhật không được để trống")]
        [Display(Name = "Thời gian tạo mới")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Người cập nhật không được để trống")]
        [Display(Name = "Cập nhật bởi")]
        public int UpdateBy { get; set; }

        [Required(ErrorMessage = "Thời gian cập nhật không được để trống")]
        [Display(Name = "Thời gian cập nhật")]
        public DateTime UpdateAt { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }

    }
}
