using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Assignment.Models;

namespace MVC_Assignment.Repository
{
    /// <summary>
    /// EmployeeRepository class
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        /// <summary>
        /// variable of employeeDbContext
        /// </summary>
        EmployeeDbEntities employeeDbContext;

        /// <summary>
        /// constructor of EmployeeRepository
        /// </summary>
        /// <param name="employeeDbEntities"></param>
        public EmployeeRepository(EmployeeDbEntities employeeDbEntities)
        {
            this.employeeDbContext = employeeDbEntities;
        }

        /// <summary>
        /// method to delete employee by ID
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteEmployee(int Id)
        {
            Employee user = employeeDbContext.Employees.Find(Id);

            employeeDbContext.Employees.Remove(user); 

            employeeDbContext.SaveChanges();
        }

        /// <summary>
        /// method to insert employee data in database
        /// </summary>
        /// <param name="employee"></param>
        public void InsertEmployee(Employee employee)
        {
            employeeDbContext.Employees.Add(employee);
            employeeDbContext.SaveChanges();
        }

        /// <summary>
        /// method to get all locations from database
        /// </summary>
        /// <returns>returns all locations</returns>
        public IEnumerable<SelectListItem> GetAllLocations()
        {
                IEnumerable<SelectListItem> items = employeeDbContext.Employees.Select(c => new SelectListItem
                {
                    Value = c.Location,
                    Text = c.Location,
                    Selected = true

                });

            return items;  
        }

        /// <summary>
        /// method to GetEmployeesDetails
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="search"></param>
        /// <returns>return employee data</returns>
        public IEnumerable<Employee> GetEmployeesDetails(string searchBy, string search)
        {
           
                IEnumerable<Employee> employees = new List<Employee>();
                if (searchBy != null && searchBy.ToUpper() == Constants.searchCategory.Salary.ToString().ToUpper())
                {
                    employees = employeeDbContext.Employees.Where(emp => emp.Salary.ToString() == search || search == null).ToList();
                }

                else if (searchBy != null && searchBy.ToUpper() == Constants.searchCategory.Age.ToString().ToUpper())
                {
                    employees = employeeDbContext.Employees.Where(emp => emp.Age.ToString().ToUpper() == search.ToUpper() || search == null).ToList();
                }
                else if (searchBy != null && searchBy.ToUpper() == Constants.searchCategory.Location.ToString().ToUpper())
                {
                    employees = employeeDbContext.Employees.Where(emp => emp.Location.ToString().ToUpper() == search.ToUpper() || search == null).ToList();

                }

                else
                {
                    employees = employeeDbContext.Employees.ToList();
                }

                return employees;
            

           
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    employeeDbContext.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



    }
}