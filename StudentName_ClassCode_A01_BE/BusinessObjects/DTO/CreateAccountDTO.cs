using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTO
{
    public class CreateAccountDTO
    {
        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Account name must be between 2 and 100 characters.")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string AccountPassword { get; set; }

        [Required(ErrorMessage = "Account role is required.")]
        [RegularExpression("^(Admin|Staff|Lecturer)$", ErrorMessage = "Role must be 'Admin', 'Staff', or 'Lecturer'.")]
        public string AccountRole { get; set; }
    }

}
