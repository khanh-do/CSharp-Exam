using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace BELTEXAM.Models
{
    public class User
    {
        [Key]								
        public int UserId { get; set; }
            
	    [Required]
	    [MinLength(2, ErrorMessage="Your first name must be at least 2 characters long.")]
        [MaxLength(45, ErrorMessage="Your first name must contain less than 45 characters.")]
	    [Display(Name="First Name")]	
        public string FirstName { get; set; }				

	    [Required]
	    [MinLength(2, ErrorMessage="Your first name must be at least 2 characters long.")]
        [MaxLength(45, ErrorMessage="Your last name must contain less than 45 characters.")]
	    [Display(Name="Last Name")]
        public string LastName { get; set; }

	    [Required]
	    [EmailAddress]
        [Display(Name="Email Address")]
        public string Email { get; set; }

	    [Required]
	    // [MinLength(8, ErrorMessage="Password must be at least 8 characters long.")]
	    [DataType(DataType.Password)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contain at least one upper case english letter, at least one lower case english letter, at least one digit, and at least one special character and be a minimum of 8 in length")]
        public string Password { get; set; }

	    [NotMapped]								
	    [Compare("Password")]
	    [DataType(DataType.Password)]
	    [Display(Name="Confirm Password")]
	    public string ConfirmPassword { get; set; }

        public List<MeetUp> CreatedMeetUps { get; set; }  

        public List<Reservation> Reservations { get; set; } 
            									
        public DateTime CreatedAt { get;set; } = DateTime.Now;		
        public DateTime UpdatedAt { get;set; } = DateTime.Now;
    }
}