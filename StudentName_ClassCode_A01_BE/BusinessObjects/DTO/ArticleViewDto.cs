using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{

    public class ArticleViewDto
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string? Headline { get; set; }
        public DateTime CreatedDate { get; set; }
        public string NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public bool NewsStatus { get; set; } 

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } 

        public int CreatedById { get; set; }
        public string CreatedByAccountName { get; set; } 

        public DateTime? ModifiedDate { get; set; }
        public int? UpdatedById { get; set; }
        public string? UpdatedByAccountName { get; set; } 

        public List<TagViewDto> Tags { get; set; } = new List<TagViewDto>(); 
    }
}
