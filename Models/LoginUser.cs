using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;


namespace BELTEXAM.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email Address")]
        public string Email { get; set; }

        [Required]
        // [MinLength(8, ErrorMessage="Password must be at least 8 characters long.")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contain at least one upper case english letter, at least one lower case english letter, at least one digit, and at least one special character and be a minimum of 8 in length")]
        [DataType(DataType.Password)]
        public string Password { get; set; }        
    }
}