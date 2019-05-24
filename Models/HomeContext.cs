using Microsoft.EntityFrameworkCore;
using BELTEXAM.Models;

namespace BELTEXAM.Models
{
    	public class HomeContext : DbContext
    	{        		
        	public HomeContext(DbContextOptions options) : base(options) { }		

            public DbSet<User> Users { get; set; }

            public DbSet<MeetUp> MeetUps { get; set; }

            public DbSet<Reservation> Reservations { get; set; }
    	}
}