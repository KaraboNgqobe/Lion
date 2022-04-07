using System;
using System.ComponentModel.DataAnnotations;

namespace POC_Leave.Models
{
    public class Employee
    {
        public Employee()
        {
        }

        [Key]
        public int id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveReason { get; set; }
        public int LeaveDays { get; set; }
  
    }
}
