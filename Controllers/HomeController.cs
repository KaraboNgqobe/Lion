using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using POC_Leave.Models;


namespace POC_Leave.Controllers
{
    public class HomeController : Controller
    {


        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Quickstart";



        private readonly ILogger<HomeController> _logger;
       
    public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Employee> employee = new List<Employee>();

            using ( MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=LeaveManagement;port=3306;password=Joburg.5947"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Employee", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    Employee obj = new Employee();
                    obj.id = Convert.ToInt32(reader["id"]);
                    obj.FirstName = reader["FirstName"].ToString();
                    obj.LastName = reader["LastName"].ToString();
                    obj.startDate = Convert.ToDateTime(reader["StartDate"]);
                    obj.endDate = Convert.ToDateTime(reader["EndDate"]);
                    obj.LeaveType = reader["LeaveType"].ToString();
                    obj.LeaveReason = reader["LeaveReason"].ToString();
                    obj.LeaveDays = Convert.ToInt32(reader["LeaveDays"]);
                  
                   

                    employee.Add(obj);



                }

                reader.Close();
            }
            return View(employee);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Employee()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


  
        [HttpPost]
        public IActionResult Event( DateTime start, DateTime end)
        {

            //API GET STRING
          
            Event newEvent = new Event()

            
            {
                Summary = "Google I/O 2015",
                Location = "Lions Development",
                Description = "Create an Event for Leave Request",
                Start = new EventDateTime()
                {
                    DateTime = start,
                    TimeZone = "America/Los_Angeles",
                },
                End = new EventDateTime()
                {
                    DateTime = end,
                    TimeZone = "America/Los_Angeles",
                },
                Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=2" },
                Attendees = new EventAttendee[] {
                new EventAttendee() { Email = "u16278055@tuks.co.za" },
               
                 },

                Reminders = new Event.RemindersData()
                {
                    UseDefault = false,
                    Overrides = new EventReminder[] {
                    new EventReminder() { Method = "email", Minutes = 24 * 60 },
                    new EventReminder() { Method = "sms", Minutes = 10 },
                     }
                }
            };


           // String calendarId = "uExample@tuks.co.za";
            //EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);

            //Event createdEvent = request.Execute();
           // Console.WriteLine("Event created: {0}");

           
            return Redirect("Index");




        }


        // Adding a new Leave Request
        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            

            using (MySqlConnection cn = new MySqlConnection("server=localhost;user=root;database=LeaveManagement;port=3306;password=Joburg.5947"))
            {
                
                
                    
                    cn.Open();

                    string Query = "INSERT INTO LeaveManagement.Employee (FirstName, LastName, StartDate, EndDate, LeaveType, LeaveReason, LeaveDays) VALUES (@FirstName,@LastName,@StartDate, @EndDate,@LeaveType,@LeaveReason,@LeaveDays); ";
                   
                    using (MySqlCommand cmd = new MySqlCommand(Query, cn))
                    {
                         Console.WriteLine(employee.FirstName);
                        cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                        cmd.Parameters.AddWithValue("@StartDate", employee.startDate);
                        cmd.Parameters.AddWithValue("@EndDate", employee.endDate);
                        cmd.Parameters.AddWithValue("@LeaveType", employee.LeaveType);
                        cmd.Parameters.AddWithValue("@LeaveReason", employee.LeaveReason);
                        cmd.Parameters.AddWithValue("@LeaveDays", employee.LeaveDays);

                         Console.WriteLine(Query);
                        cmd.ExecuteNonQuery();

                        cn.Close();
                    }

                Event(employee.startDate, employee.endDate); // pass the startDate and endDate to Event function

            }

            // Event(StartDate, EndDate)

            return Redirect("Index");

            
        }

        
    }
}
