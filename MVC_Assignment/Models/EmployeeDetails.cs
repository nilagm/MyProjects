using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Assignment.Models
{
    public class EmployeeDetails
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public double Salary { get; set; }
        public string MaritalSTatus { get; set; }
        public string Location { get; set; }

    }
}