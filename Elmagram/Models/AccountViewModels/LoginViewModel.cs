using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Elmagram.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Display(Name = "ELMA Login")]
        public string ElmaLogin { get; set; }

        [Display(Name = "ELMA Password")]
        [DataType(DataType.Password)]
        public string ElmaPassword { get; set; }
    }
}
