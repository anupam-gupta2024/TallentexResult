using Microsoft.AspNetCore.Mvc;
using System.Data;
using TallentexResult.Entities;
using TallentexResult.Interface;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Controllers
{
    // Explicitly call the baseController ctor and have each ChildController ctor inject the BaseController's dependencies
    public class IFAController : baseController
    {
        /// <summary>
        /// injecting the service (IDropdown) by applying Interface Segregation Principle (ISP)
        /// </summary>
        private readonly IDropdown _dropdown;

        public IFAController(IDataService layer,
            IAmazonUploader amazonUploader,
            IDropdown dropdown) : base(layer, amazonUploader)
        {
            _dropdown = dropdown;
        }

        public IActionResult Index()
        {
            StudentMain model = new StudentMain();
            model.ds1 = getResult();        // Fetch basic detail of student

            if (model.ds1 != null && model.ds1.Tables.Count > 0)
            {
                DataTable dt = model.ds1.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["show_result"].ToString() == "0")     // Check if result (analysis) is displayed or not.
                    {
                        return RedirectToAction("dashboard", "result");
                    }

                    model.Class = dt.Rows[0]["Class"].ToString() ?? "";
                    model.Fno = dt.Rows[0]["appformno"].ToString() ?? "";
                }
            }

            ViewBag.Mode = _dropdown.getMode(); // Fetch the mode of study in ALLEN (ONLINE | OFFLINE)
            ViewBag.Center = _dropdown.getCenter(); // Fetch the ALLEN Center City in cas e of OFFLINE mode

            return View(model);
        }

        /// <summary>
        /// Print Scholarsip page
        /// </summary>
        /// <param name="center">ALLEn Center</param>
        public IActionResult Scholarship(int center)
        {
            StudentMain model = new StudentMain();

            model.ds1 = getResult();    // Fetch basic detail of student
            if (model.ds1 != null && model.ds1.Tables.Count > 0)
            {
                DataTable dt = model.ds1.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    model.Fno = dt.Rows[0]["appformno"].ToString() ?? "";

                    ViewBag.Fno = model.Fno;
                    ViewBag.Center = center;

                    model.ds2 = getPerformanceCard();   // call Performance method to fetch result
                }
            }

            return View(model);
        }
    }

    /// <summary>
    /// for AJAX method call in Jquery
    /// </summary>
    [SessionExpire]
    public class IFAAjaxController : Controller
    {
        /// <summary>
        /// injecting the service (IAdmissionInfo) by applying Interface Segregation Principle (ISP)
        /// </summary>
        private readonly IAdmissionInfo _admissionInfo;

        public IFAAjaxController(IAdmissionInfo admissionInfo)
        {
            _admissionInfo = admissionInfo;
        }

        #region Ajax Call
        /// <summary>
        /// Async Method to Display Scholarship using Ajax call
        /// </summary>
        /// <param name="fno">Student Roll No.</param>
        /// <param name="Center">ALLEN Center</param>
        /// <returns>JSON array that contains scholarship</returns>
        [HttpPost]
        public async Task<ActionResult> getScholarship(int fno, int Center)
        {
            IGetScholarship _getScholarship = new GetScholarship();
            _getScholarship.AppFormNo = fno;
            _getScholarship.CenterID = Center;
            _getScholarship.AcademicSession = LNResources.AcademicSession;  // get current AcademicSession value
            DataSet ds = await _admissionInfo.GetScholarship(_getScholarship);  // Fetch the scholarship from database

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Create a list that hold scholarship
                    var scholarship = await Task.FromResult(
                        dt.AsEnumerable()
                        .Select(r => r.Field<string>("Message"))
                        .FirstOrDefault());

                    return new JsonResult(new { scholarship = scholarship });
                }
            }

            // return empty JSON in case of empty record
            return new JsonResult(new { scholarship = string.Empty });
        }

        /// <summary>
        /// Async Method to Display Courses using Ajax call
        /// </summary>
        /// <param name="fno">Student Roll No.</param>
        /// <param name="Center">ALLEN Center</param>
        /// <returns>JSON array that contains courses</returns>
        [HttpPost]
        public async Task<ActionResult> getCourse(int fno, int Center)
        {
            IGetCourse _getCourse = new GetCourse();
            _getCourse.AppFormNo = fno;
            _getCourse.CenterID = Center;
            DataSet ds = await _admissionInfo.GetCourse(_getCourse);    // Fetch the course from database

            if (ds != null && ds.Tables.Count > 0)
            {
                // Create a list that contain courses
                var courselist = await Task.FromResult(
                        Newtonsoft.Json.JsonConvert.SerializeObject(ds)
                        );

                return new JsonResult(new { courselist = courselist });
            }

            // return empty JSON in case of empty record
            return new JsonResult(new { courselist = string.Empty });
        }

        /// <summary>
        /// Async Method to Display Course Fees using Ajax call
        /// </summary>
        /// <param name="fno">Student Roll No.</param>
        /// <param name="Center">ALLEN Center</param>
        /// <param name="Course">Course Code</param>
        /// <returns>JSON array that contains course fees</returns>
        [HttpPost]
        public async Task<ActionResult> getCourseFee(int fno, int Center, int Course)
        {
            IGetCourseFee _getCourseFee = new GetCourseFee();
            _getCourseFee.AppFormNo = fno;
            _getCourseFee.CenterID = Center;
            _getCourseFee.CCID = Course;
            _getCourseFee.AcademicSession = LNResources.AcademicSession;    // get current AcademicSession value
            DataSet ds = await _admissionInfo.GetCourseFee(_getCourseFee);   // Fetch the course fee from database

            if (ds != null && ds.Tables.Count > 0)
            {
                // Create a list that contain fee of passed course
                var coursefeelist = await Task.FromResult(
                        Newtonsoft.Json.JsonConvert.SerializeObject(ds)
                        );

                return new JsonResult(new { coursefeelist = coursefeelist });
            }

            // return empty JSON in case of empty record
            return new JsonResult(new { coursefeelist = string.Empty });
        }
        #endregion
    }
}
