using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; } // Changed from CategoryID

        [Required]
        [StringLength(150)]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public int? ParentCategoryId { get; set; } // Nullable for top-level categories
        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; }

        public bool IsActive { get; set; } = true; //  Default to active

        // Navigation properties
        public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
