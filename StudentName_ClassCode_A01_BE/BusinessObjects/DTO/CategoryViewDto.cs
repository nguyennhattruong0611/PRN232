using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class CategoryViewDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
