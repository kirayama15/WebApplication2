using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class HomeModels
    {
    }
    public class NewEmployee
    {
        public NewEmployee()
        {
            EmployeeName = string.Empty;
            Birthdate = string.Empty;
            TIN = string.Empty;
            EmployeeType = string.Empty;
        }
        public string EmployeeName { get; set; }
        public string Birthdate { get; set; }
        public string TIN { get; set; }
        public string EmployeeType { get; set; }
    }
}
