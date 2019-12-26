//EmployeeController class

#region
using MVC_Assignment.Models;
using MVC_Assignment.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
#endregion

namespace MVC_Assignment.Controllers
{
    /// <summary>
    /// emplpyee controller class
    /// </summary>
    public class EmployeeController: Controller
    {
        /// <summary>
        /// delcare repository interface variable
        /// </summary>
        private IEmployeeRepository Iemp;

        /// <summary>
        /// constructor of emplpyee controller
        /// </summary>
        public EmployeeController()
        {
            this.Iemp = new EmployeeRepository(new EmployeeDbEntities());
        }

        /// <summary>
        /// get method to insert employee data in database
        /// </summary>
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                //store error message value in tempdata
                TempData["errorMsg"] = ex.Message;

                // redirect to error action method in case of exception
                return RedirectToAction("Error");
            }

        }

        /// <summary>
        /// submit the form once form is filled with valid data 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns> return toindex method once data is saved in database</returns>

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //call insert method from employee repository class
                    Iemp.InsertEmployee(employee);

                    //return to index method once data is added in database
                    return RedirectToAction("Index");
                }

                return View(employee);
            }

            catch (Exception ex)
            {
                //store error message value in tempdata
                TempData["errorMsg"] = ex.Message;

                // redirect to error action method in case of exception
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// to  get all emplpyee data and search data by condition and get data according to that and sort the data
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="Sorting_Order"></param>
        /// <returns> return data on the basis of search criteria</returns>
        public ActionResult Index(string searchBy, string search,int? pageNumber,string Sorting_Order)
        {

            try
            {
                //get the all location from database 
                var dataList = Iemp.GetAllLocations();

                //store the location in viewbag
                ViewBag.Locations = dataList;

                IEnumerable<Employee> objemp = new List<Employee>();

                //do validation on search textbox
                DoValidation(searchBy, search,dataList);

                //check if modelstate is valid
                if (ModelState.IsValid)
                {
                    //call GetEmployeeDataBySearch method to get all employee daa=ta by search 
                    objemp = GetEmployeeDataBySearch(searchBy,search, pageNumber,Sorting_Order);
                   
                    //store fetched data count 
                    ViewBag.countRecord = objemp.Count();
                    return View(objemp);
                }

                else
                {
                    //store fetched data count 
                    ViewBag.countRecord = objemp.Count();

                    //if modelstate is not valid,return same view
                    return View(objemp.ToPagedList(pageNumber ?? 1, 10));

                }
            }

            catch (Exception ex)
            {
                //store error message value in tempdata
                TempData["errorMsg"] = ex.Message;

                // redirect to error action method in case of exception
                return RedirectToAction("Error");
            }
            

        }

        /// <summary>
        /// delete employee data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns> redirect to index method once data is deleted</returns>
        public ActionResult Delete(int id)
        {

            try
            {
                //call delete method EmployeeRepository
                Iemp.DeleteEmployee(id); 

                //redirect to index method
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //store error message value in tempdata
                TempData["errorMsg"] = ex.Message;

                // redirect to error action method in case of exception
                return RedirectToAction("Error");
            }

        }

        /// <summary>
        /// error action method
        /// </summary>
        /// <returns> return error view</returns>
        public ActionResult Error()
        {
            ViewBag.errorMsg = TempData["errorMsg"];
            return View();
        }


        #region private method

        /// <summary>
        /// do validation for search textbox 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="search"></param>
        /// <param name="dataList"></param>
        private void DoValidation(string searchBy, string search,IEnumerable<SelectListItem> dataList)
        {

            if (!string.IsNullOrEmpty(searchBy))
            {


                if (searchBy.ToUpper() == Constants.searchCategory.Age.ToString().ToUpper())
                {
                    int n = 0;
                    bool isInt = int.TryParse(search, out n);

                    if (!isInt || (int.Parse(search) == 0 || int.Parse(search) > 120 || int.Parse(search) < 0))
                    {

                        ModelState.AddModelError("AgeErrorMsg", "Please enter the age in between 1 to 120");
                    }
                }

                else if (searchBy.ToUpper() == Constants.searchCategory.Salary.ToString().ToUpper())
                {
                    Double n = 0;
                    bool isInt = Double.TryParse(search, out n);

                    if (!isInt)
                    {

                        ModelState.AddModelError("SalaryErrorMsg", "Please enter salary in valid format");
                    }
                }

                else if (searchBy.ToUpper() == Constants.searchCategory.Location.ToString().ToUpper())
                {
                    var containValue = dataList.Where(s => s.Value == search).ToList();
                    if (containValue.Count == 0 || string.IsNullOrEmpty(search))
                    {

                        ModelState.AddModelError("locationErrorMSg", "please select the location");
                    }

                }

            }
        }

        /// <summary>
        /// to get employee data by search
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="Sorting_Order"></param>
        /// <returns>return employee date by seach</returns>
        private IEnumerable<Employee> GetEmployeeDataBySearch(string searchBy,string search, int? pageNumber, string Sorting_Order)
        {
           var objemp = Iemp.GetEmployeesDetails(searchBy, search).ToPagedList(pageNumber ?? 1, 10);

            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";
            ViewBag.SortingAge = Sorting_Order == "Age" ? "Age_description" : "Age";
            ViewBag.SortingId = Sorting_Order == "Id" ? "Id_description" : "Id";
            ViewBag.SortingMStatus = Sorting_Order == "MaritalStatus" ? "MaritalStatus_description" : "MaritalStatus";
            switch (Sorting_Order)
            {
                case "Name_Description":
                    objemp = objemp.OrderByDescending(s => s.Name).ToPagedList(pageNumber ?? 1, 10);
                    break;
                case "Age":
                    objemp = objemp.OrderBy(s => s.Age).ToPagedList(pageNumber ?? 1, 10); ;
                    break;
                case "Age_description":
                    objemp = objemp.OrderByDescending(stu => stu.Age).ToPagedList(pageNumber ?? 1, 10); ;
                    break;
                case "Id_description":
                    objemp = objemp.OrderByDescending(stu => stu.Id).ToPagedList(pageNumber ?? 1, 10); ;
                    break;
                case "Id":
                    objemp = objemp.OrderBy(s => s.Id).ToPagedList(pageNumber ?? 1, 10); ;
                    break;
                case "MaritalStatus_description":
                    objemp = objemp.OrderByDescending(s => s.MaritalStatus).ToPagedList(pageNumber ?? 1, 10); ;
                    break;
                case "MaritalStatus":
                    objemp = objemp.OrderBy(s => s.MaritalStatus).ToPagedList(pageNumber ?? 1, 10); ;
                    break;
                default:
                    objemp = objemp.OrderBy(stu => stu.Name).ToPagedList(pageNumber ?? 1, 10); ;
                    break;
            }
            return objemp;

        }
        #endregion
    }
}