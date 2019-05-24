using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BELTEXAM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BELTEXAM.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;

        public HomeController(HomeContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(user => user.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Add(newUser);
                    dbContext.SaveChanges();
                    // Get UserID from newly added user and save to Session
                    User regUser = dbContext.Users.FirstOrDefault(user => user.Email == newUser.Email);
                    HttpContext.Session.SetInt32("UserId", regUser.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [Route("loginUser")]
        [HttpPost]
        public IActionResult LoginUser(LoginUser loginUser)
        {
            if(ModelState.IsValid)
            {
                // grab email and password fields 
                // search database for user object with that email                      
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == loginUser.Email);
                // if none found, send error message to ModelState
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }                  
                // compare hashed password               
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.Password);
                // if not matched, send error message to ModelState
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);                
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Login");
            }
        }

        [Route("dashboard")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            if( HttpContext.Session.GetInt32("UserId") == null )
            {
                return RedirectToAction("Logout"); 
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                // search database for user object with that userId  
                var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
                List<MeetUp> allMeetUps = dbContext.MeetUps.OrderBy(t => t.Date).Include(m => m.Creator).Include(act => act.Reservations).ThenInclude(res => res.User).ToList(); 
                ViewBag.User = dbContext.Users.Include( u => u.Reservations).ThenInclude( r=> r.MeetUp).FirstOrDefault( u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
                return View("Dashboard", allMeetUps);
            }
        }

        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            if( HttpContext.Session.GetInt32("UserId") == null )
            {
                return RedirectToAction("Index");                
            }
            else
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                return View("New");
            }
        }        

        [Route("create")]
        [HttpPost]
        public IActionResult Create(MeetUp newMeetUp)
        {
            if(HttpContext.Session.GetInt32("UserId") == null) 
            {
                return RedirectToAction("Logout");              
            }
            else
            {
                if(ModelState.IsValid)                         
                {
                    dbContext.Add(newMeetUp);              
                    dbContext.SaveChanges();
                    return Redirect("details/" + newMeetUp.MeetUpId);
                }
                else
                {
                    return View("New");                
                }
            }
                        
        }

        [Route("details/{meetUpId}")]
        [HttpGet]
        public IActionResult Details(int meetUpId)
        {
            if( HttpContext.Session.GetInt32("UserId") == null )
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UserId=HttpContext.Session.GetInt32("UserId");
                // search database for wedding object with that weddingId  
                // var wedding = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
                // Adrien's
                MeetUp meetUp = dbContext.MeetUps.Include(m => m.Creator).Include( w => w.Reservations).ThenInclude(r => r.User).FirstOrDefault( w => w.MeetUpId==meetUpId);      
                // ViewBag.User =dbContext.Users.FirstOrDefault( u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                return View("Details", meetUp);
            }
        }

        [HttpGet("destroy/{meetUpId}")]
        public IActionResult Destroy(int meetUpId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Logout");
            }
            else
            {
                MeetUp removeMeetUp= dbContext.MeetUps.FirstOrDefault(w => w.MeetUpId == meetUpId);
                if (removeMeetUp == null)
                {
                    return RedirectToAction("Dashboard");
                }
                dbContext.MeetUps.Remove(removeMeetUp);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
        }

        [Route("rsvp/{meetUpId}/{status}")]
        [HttpGet]
        public IActionResult RSVP(int meetUpId, string status)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if(status == "add")
                {
                    
                    Reservation newResponse = new Reservation();
                    newResponse.UserId = (int)HttpContext.Session.GetInt32("UserId");
                    newResponse.MeetUpId = meetUpId;
                    dbContext.Reservations.Add(newResponse);
                    dbContext.SaveChanges();
                    // return RedirectToAction("Dashboard");
                    // return View("Dashboard");
                }
                else if(status == "remove")
                {
                    Reservation removeResponse = dbContext.Reservations.FirstOrDefault( r => r.MeetUpId == meetUpId && r.UserId == HttpContext.Session.GetInt32("UserId"));
                    dbContext.Reservations.Remove(removeResponse);
                    dbContext.SaveChanges();                    
                }
                return RedirectToAction("Dashboard");
                // return View("Dashboard");
                
            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
        }
    }
}
