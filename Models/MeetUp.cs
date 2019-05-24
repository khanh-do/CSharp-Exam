using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace BELTEXAM.Models
{
    public class MeetUp
    {
        [Key]
        public int MeetUpId { get; set; }

        [Required]
	    [MinLength(2, ErrorMessage="Title name must be at least 2 characters long.")]
        [MaxLength(45, ErrorMessage="Tile name must contain less than 45 characters.")]
	    [Display(Name="Title")]	
        public string Title { get; set; }      

        [Required]
        [Display(Name="Date and Time")]  
        //[DataType(DataType.Date)]   //To get the time entry to show up also, leave this line out
        [FutureDate]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name="Duration (in Hours)")]
        [Range(0, 12)]
        public int Duration { get; set; }

        [Required]
	    [MinLength(2, ErrorMessage="Description must be at least 2 characters long.")]
        [MaxLength(245, ErrorMessage="Description must contain less than 245 characters.")]
	    [Display(Name="Description")]	
        public string Description { get; set; }

        public List<Reservation> Reservations { get; set; }  

        [Required]
	    public int UserId { get; set; }

	    public User Creator { get; set; }      
    
        public DateTime CreatedAt { get;set; } = DateTime.Now;
        public DateTime UpdatedAt { get;set; } = DateTime.Now;
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, 
        ValidationContext validationContext)
        {
            DateTime date;
            if(value is DateTime)
            {
                date = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid datetime");
            }
            if(date < DateTime.Now)
            {
                return new ValidationResult("Date must be in the future");
            }
            else
            {
               return ValidationResult.Success;
            }
        }
    }
}