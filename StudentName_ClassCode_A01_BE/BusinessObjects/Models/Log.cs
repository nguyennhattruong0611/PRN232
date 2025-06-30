using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public string Message { get; set; }

        public string? Level { get; set; } // Ví dụ: "Info", "Warning", "Error"

        public DateTime LoggedDate { get; set; }

        public string? Exception { get; set; } // Nếu muốn lưu thông tin exception

        public string? UserId { get; set; } // ID của người dùng gây ra log (nếu có)

        public string? Action { get; set; } // Hành động gây ra log (ví dụ: "CreateArticle")
    }
}
