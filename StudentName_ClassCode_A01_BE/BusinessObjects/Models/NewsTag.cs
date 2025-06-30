using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class NewsTag
    {
        // Composite Primary Key configured in DbContext using Fluent API
        [Required]
        public int NewsArticleId { get; set; }
        [ForeignKey("NewsArticleId")]
        public virtual NewsArticle NewsArticle { get; set; }

        [Required]
        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}
