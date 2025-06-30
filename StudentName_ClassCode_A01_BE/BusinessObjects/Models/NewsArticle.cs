using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class NewsArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsArticleId { get; set; } // Changed from NewsArticleID

        [Required]
        [StringLength(255)]
        public string NewsTitle { get; set; }

        public string? Headline { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string NewsContent { get; set; }

        [StringLength(255)]
        public string? NewsSource { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public bool NewsStatus { get; set; } 

        [Required]
        public int CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual SystemAccount CreatedBy { get; set; }

        public int? UpdatedById { get; set; } 
        [ForeignKey("UpdatedById")]
        public virtual SystemAccount UpdatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } 

        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
