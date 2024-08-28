using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using TallentexResult.Entities;
using TallentexResult.Interface;
using TallentexResult.Models;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _layer;   // injecting the dataservice
        private readonly ISMSService _smsService;   // injecting the sms service

        public HomeController(IDataService layer, ISMSService smsService)   // using interfaces in Dependency Injection
        {
            _layer = layer;
            _smsService = smsService;
        }

        //private readonly ILogger<HomeController> _logger;
        //private readonly ISMSService _smsService;

        //public HomeController(ILogger<HomeController> logger, ISMSService smsService)
        //{
        //    _logger = logger;
        //    _smsService = smsService;
        //}

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("fno"))) // if already login the nredirect to dashboard page
            {
                return RedirectToAction("dashboard", "result");
            }

            StudentMain model = new StudentMain();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(StudentMain model)
        {
            ViewData["MsgClass"] = "alert alert-danger";

            if (string.IsNullOrEmpty(model.Fno))
                ModelState.AddModelError("Fno", "Please type TALLENTEX Form No.");
            else if (model.Fno.Length != 8)
                ModelState.AddModelError("Fno", "TALLENTEX Form No. must contains 8 characters");
            else
            {
                // Assigning an object to the interface of its implemented class.
                IAuthenticateStudentResult _authenticateStudentResult = new AuthenticateStudentResult();

                _authenticateStudentResult.AppFormNo = model.Fno;
                _authenticateStudentResult.StudentName = "RES#987";
                _authenticateStudentResult.IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                DataSet ds = _layer.AuthenticateStudentResult(_authenticateStudentResult);  // Authenticate Student & fetch MObile to send OTP
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["status"].ToString() != "1")
                        {
                            ViewData["Message"] = dt.Rows[0]["message"].ToString();
                        }
                        else
                        {
                            HttpContext.Session.SetString("fno", model.Fno);
                            return RedirectToAction("dashboard", "result");
                            HttpContext.Session.SetString("OTP", "1234");
                            ViewBag.OTP = "1";

                            //HttpContext.Session.SetString("OTP", new Global().GetOTP());
                            //if (_smsService.Send(1, "9460937936", this.Request))
                            //    ViewBag.OTP = "1";
                            //else
                            //    ViewData["Message"] = "Some problem occured in SMS";

                        }
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// Validate the OTP
        /// </summary>
        /// <returns>redirect to dashboard if successfull validated</returns>
        [HttpPost]
        public IActionResult OTP(StudentMain model, IFormCollection frm)
        {
            ViewData["MsgClass"] = "alert alert-danger";

            if (frm == null)
                ViewData["Message"] = "Some error occured";
            else if (string.IsNullOrEmpty(model.Fno))
                ViewData["Message"] = "Invalid Form No.";
            else if (string.IsNullOrEmpty(frm["otp"]))
                ViewData["Message"] = "Please type otp";
            else if (frm["otp"] != HttpContext.Session.GetString("OTP"))
                ViewData["Message"] = "Please type correct otp";
            else
            {
                HttpContext.Session.SetString("fno", model.Fno);
                return RedirectToAction("dashboard", "result");
            }

            ViewBag.OTP = "1";
            return View("index", model);
        }

        //public IActionResult Privacy()
        //{
        //    ViewBag.Privacy = HttpContext.Session.GetString("OTP");

        //    var status = _smsService.Send(1, "9460937936", this.Request);

        //    HttpContext.Session.Clear();
        //    var a = HttpContext.Request.GetDisplayUrl();
        //    return View();
        //}        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var execeptionHandlerPathFeture = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ErrorViewModel model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = execeptionHandlerPathFeture.Error.Message,
                Source = execeptionHandlerPathFeture.Error.Source,
                ErrorPath = execeptionHandlerPathFeture.Path,
                StackTrace = execeptionHandlerPathFeture.Error.StackTrace,
                InnerException = Convert.ToString(execeptionHandlerPathFeture.Error.InnerException)
            };

            return View(model);
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class LogoutController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Removes all entries from the current session, if any.The session cookie is not removed.
            HttpContext.Session.Clear();
            //Delete the Session object by session ID.
            HttpContext.Session.Remove(HttpContext.Session.Id);

            return RedirectToAction("index", "home");
        }
    }
}
