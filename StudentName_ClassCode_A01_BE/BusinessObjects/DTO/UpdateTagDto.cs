using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class UpdateTagDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string TagName { get; set; }

        [StringLength(255)]
        public string? Note { get; set; }
    }
}
