using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace BELTEXAM.Models
{
    public class Reservation
    {
        [Key]								
        public int ReservationId { get; set; }
            
	    [Required]	    
        public int UserId { get; set; }				

	    [Required]
	    public int MeetUpId { get; set; }

	    public User User { get; set; }      

        public MeetUp MeetUp { get; set; }
            									
        public DateTime CreatedAt { get;set; } = DateTime.Now;		
        public DateTime UpdatedAt { get;set; } = DateTime.Now;
    }
}