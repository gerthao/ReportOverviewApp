﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        //[RegularExpression(@"[a-zA-Z0-9._%=-]+(@greatdentalplans\.com)$", ErrorMessage = "Email must be from greatdentalplans.com, example: \"user@greatdentalplans.com\"")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Login")]
        public bool RememberMe { get; set; }
    }
}
