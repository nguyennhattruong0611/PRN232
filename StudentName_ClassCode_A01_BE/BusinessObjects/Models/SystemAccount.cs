using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class SystemAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; } // Changed from AccountID to follow C# conventions

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string AccountEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountRole { get; set; } // e.g., "Admin", "Staff", "Lecturer" 

        [Required]
        public string AccountPassword { get; set; } // Remember to hash passwords in a real application!

        // Navigation properties
        [InverseProperty("CreatedBy")]
        public virtual ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();

        [InverseProperty("UpdatedBy")]
        public virtual ICollection<NewsArticle> UpdatedNewsArticles { get; set; } = new List<NewsArticle>();
    }
}
