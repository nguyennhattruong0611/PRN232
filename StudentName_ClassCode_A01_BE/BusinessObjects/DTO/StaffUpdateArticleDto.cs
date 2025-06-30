using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class StaffUpdateArticleDto
    {
        [Required]
        [StringLength(255, MinimumLength = 10)]
        public string NewsTitle { get; set; }

        [StringLength(500)]
        public string? Headline { get; set; }

        [Required]
        public string NewsContent { get; set; }

        [StringLength(255)]
        public string? NewsSource { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<int>? TagIds { get; set; }

        public bool? NewsStatus { get; set; } 

    }
}
