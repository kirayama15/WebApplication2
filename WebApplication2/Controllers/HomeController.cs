using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HomeModels model = new HomeModels();

        public List<NewEmployee> employee = new List<NewEmployee>();
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        /// <summary>
        /// Method created in adding of New Employee
        /// Employee Details is being added on a list then being stored in Cache Memory since there is no Database connected
        /// </summary>
        /// <param name="employeeName"></param>
        /// <param name="birthdate"></param>
        /// <param name="tin"></param>
        /// <param name="empType"></param>
        /// <returns></returns>
        public string AddEmployee(string employeeName, string birthdate, string tin, string empType)
        {
            string EmployeeName = employeeName;
            try
            {
                ///Creation of New Cache
                ObjectCache _cache = MemoryCache.Default;
                string newEmpCache = string.Format("Emp", "Employees");
                List<NewEmployee> empList = new List<NewEmployee>();
                empList = _cache[newEmpCache] as List<NewEmployee>;
                //Checking if there is already an existing Cache for Employee List
                if (!_cache.Contains(newEmpCache) || empList == null)
                {
                    //If no Employee List Cache found, it will populate a new list then it would be stored on the Cache
                    NewEmployee empDetails = new NewEmployee()
                    {
                        EmployeeName = employeeName,
                        Birthdate = birthdate,
                        TIN = tin,
                        EmployeeType = empType
                    };
                    //Setting of Cache Expiration into 1 hour
                    CacheItemPolicy cachePolicy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                    };
                    List<NewEmployee> newEmpList = new List<NewEmployee>();
                    newEmpList.Add(empDetails);
                    _cache.Set(string.Format("Emp", "Employees"), newEmpList, cachePolicy);
                }
                else
                {
                    //If Employee List Cache was found, It will add the new employee details into the existing Employee List then stored back again to Cache
                    NewEmployee empDetails = new NewEmployee()
                    {
                        EmployeeName = employeeName,
                        Birthdate = birthdate,
                        TIN = tin,
                        EmployeeType = empType
                    };
                    //Same Cache Expiration setting of 1 hour
                    CacheItemPolicy cachePolicy = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                    };
                    empList.Add(empDetails);
                    _cache.Set(string.Format("Emp", "Employees"), empList, cachePolicy);
                }
            }
            catch (Exception e1)
            {

                employeeName = e1.Message;
            }
            
            
            return EmployeeName;
        }
        public IActionResult EmployeeList()
        {
            
            return View();
        }


        /// <summary>
        /// Method Created in Getting the Employee List from Cache
        /// </summary>
        /// <returns></returns>
        public IActionResult EmployeeLists()
        {
            try
            {
                ObjectCache _cache = MemoryCache.Default;
                string newEmpCache = string.Format("Emp", "Employees");
                employee = _cache[newEmpCache] as List<NewEmployee>;
                return Ok(employee);
            }
            catch (Exception e1)
            {

                return Error();
            }
           
        }

        /// <summary>
        /// Method created for showing Employee Details based on the Value Selected from the Employee Dropdown Filter
        /// </summary>
        /// <param name="empSelected"></param>
        /// <returns></returns>
        public IActionResult EmployeeFilter(string empSelected)
        {
            try
            {
                ObjectCache _cache = MemoryCache.Default;
                string newEmpCache = string.Format("Emp", "Employees");
                employee = _cache[newEmpCache] as List<NewEmployee>;
                employee = employee.Where(x => x.EmployeeName == empSelected).ToList();
                return Ok(employee);
            }
            catch (Exception e1)
            {

                return Error();
            }
            
        }

        /// <summary>
        /// Method Created for Computing of an Employee Salary
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="absences"></param>
        /// <param name="empType"></param>
        /// <returns></returns>
        public string CalculateSalary(string rate, string absences, string empType)
        {
            string netPay = string.Empty;
            try
            {
                if (empType == "Regular Employee")
                {
                    //If Employee Type is Regular Employee below are the variables that holds all the salary deductions and computation
                    double daysAbsence = Convert.ToDouble(absences);
                    double salaryDeduction = 0;
                    double taxDeduction = (Convert.ToDouble(rate) * 0.12);
                    if (daysAbsence > 0)
                    {
                        salaryDeduction = (Convert.ToDouble(rate) / 22);

                    }
                    double netRegSalary = Convert.ToDouble(rate) - salaryDeduction;
                    netRegSalary = netRegSalary - taxDeduction;
                    netRegSalary = Math.Round(netRegSalary, 2);
                    netPay = netRegSalary.ToString("0.00");
                }
                else
                {
                    //If Employee type is Contractual Employee below is the computation
                    double daysworked = Convert.ToDouble(absences);
                    double netSalary = (Convert.ToDouble(rate) * daysworked);
                    netSalary = Math.Round(netSalary, 2);
                    netPay = netSalary.ToString("0.00");
                }

            }
            catch (Exception e1)
            {

                netPay = e1.Message;
            }
            
            
            return netPay;
        }
    }
}
