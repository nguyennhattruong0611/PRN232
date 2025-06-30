using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }

        [Required]
        [StringLength(100)]
        public string TagName { get; set; }

        public string Note { get; set; }

        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
