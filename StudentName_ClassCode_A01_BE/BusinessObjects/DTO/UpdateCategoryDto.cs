using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Category name must be between 3 and 150 characters.")]
        public string CategoryName { get; set; }

        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
        public string? CategoryDescription { get; set; }

        public int? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool IsActive { get; set; }
    }
}
