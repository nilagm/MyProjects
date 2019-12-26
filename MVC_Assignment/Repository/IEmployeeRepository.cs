using MVC_Assignment.Models;
using System.Collections.Generic;

#region
using System.Web.Mvc;
#endregion

namespace MVC_Assignment.Repository
{
    /// <summary>
    /// interface of IEmployeeRepository
    /// </summary>
    interface IEmployeeRepository
    {

        /// <summary>
        /// method to GetEmployeesDetails
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="search"></param>
        /// <returns>return employee data</returns>
        IEnumerable<Employee> GetEmployeesDetails(string searchBy,string search);

        /// <summary>
        /// method to delete employee by ID
        /// </summary>
        /// <param name="Id"></param>
        void DeleteEmployee(int Id);

        /// <summary>
        /// method to get all locations from database
        /// </summary>
        /// <returns>returns all locations</returns>
        IEnumerable<SelectListItem> GetAllLocations();

        /// <summary>
        /// method to insert employee data in database
        /// </summary>
        /// <param name="employee"></param>
        void InsertEmployee(Employee employee);
    }
}
