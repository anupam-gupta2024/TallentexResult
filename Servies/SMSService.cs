using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using System.Data;
using TallentexResult.Entities;
using TallentexResult.Interface;
using TallentexResult.Models;
using TallentexResult.Models.StoredProcedure;
using TallentexResult.Repository;

namespace TallentexResult.Servies
{
    /// <summary>
    /// This class is used by the application to send SMS
    /// </summary>
    public class SMSService : ISMSService
    {
        private readonly SMSSetting _smsSetting;
        private readonly IRegistrationDb _registrationDb;
        private readonly ISetSMSLogs _setSMSLogs;

        /// <param name="smsSetting">Inject the dependency by passing SMSSetting from appsetting.json</param>
        /// <param name="options">Inject the dependency by passing Database Connection string from appsetting.json</param>
        public SMSService(IOptions<SMSSetting> smsSetting, IOptions<DataConnection> options)
        {
            _smsSetting = smsSetting.Value;
            _registrationDb = new RegistrationDb(options.Value.RegistrationConnection);

            // Assigning an SMS Log object to the interface of its implemented class.
            _setSMSLogs = new SetSMSLogs();
        }

        // Plug in your SMS service here to send a text message.
        public bool Send(int templateid, string to, HttpRequest request)
        {
            bool flag = false;

            if (!ValidateInputData.ValidateMobileNumber(to))
            {
                // "Mobile number is mandatory"
            }
            else
            {
                SMSLanguage(templateid);

                if (string.IsNullOrEmpty(_setSMSLogs.Messagetext))
                {
                    // "Message language is missing"
                }
                else
                {
                    string url_ = _smsSetting.generateUrl(_setSMSLogs.Messagetext, to); // generate SMS URL
                    string response = "";

                    try
                    {
                        // call Http request service by passing sms url
                        response = HttpService.requestAsync(url_).Result;
                        if (response.Contains("success")) flag = true;

                        //status_.StatusCode = response.Contains("success") ? 0 : int.Parse(response.Split('|')[1]);
                    }
                    catch (Exception ex) { response = ex.Message; }
                    finally
                    {
                        // create log after sending sms
                        SMSLog(to, response, request);
                    }
                }
            }

            return flag;
            // Fetch the SMS language from database by passing templateid
            void SMSLanguage(int templateid)
            {
                _setSMSLogs.Action = 2;
                _setSMSLogs.Templateid = templateid.ToString();

                DataSet dstemplate = _registrationDb.SetSMSLogs(_setSMSLogs);

                if (dstemplate != null && dstemplate.Tables.Count > 0)
                {
                    if (dstemplate.Tables[0] != null && dstemplate.Tables[0].Rows.Count > 0)
                    {
                        if (_setSMSLogs.Templateid == "1")  // In case of OTP Langauage set the necessary parameter (i.e. OTP Code)
                        {
                            _setSMSLogs.Appname = "OTP";
                            string[] parameters = { "", request.HttpContext.Session.GetString("OTP") ?? "", "TALLENTEX Online Registration Portal" };
                            _setSMSLogs.Messagetext = (dstemplate.Tables[0].Rows[0]["templatetext"].ToString() ?? "").FormatEx(parameters);
                        }
                    }
                }
            }
        }

        // Plug in your SMS service here to send a text message.
        public bool Send(string to, string message, string appname, HttpRequest request)
        {
            bool flag = false;

            if (!ValidateInputData.ValidateMobileNumber(to))
            {
                // "Mobile number is mandatory"
            }
            else
            {
                _setSMSLogs.Messagetext = message;
                _setSMSLogs.Appname = appname;

                if (string.IsNullOrEmpty(_setSMSLogs.Messagetext))
                {
                    // "Message language is missing"
                }
                else
                {
                    string url_ = _smsSetting.generateUrl(_setSMSLogs.Messagetext, to); // generate SMS URL
                    string response = "";

                    try
                    {
                        // call Http request service by passing sms url
                        response = HttpService.requestAsync(url_).Result;
                        if (response.Contains("success")) flag = true;

                        //status_.StatusCode = response.Contains("success") ? 0 : int.Parse(response.Split('|')[1]);
                    }
                    catch (Exception ex) { response = ex.Message; }
                    finally
                    {
                        // create log after sending sms
                        SMSLog(to, response, request);
                    }
                }
            }

            return flag;
        }

        private void SMSLog(string to, string response, HttpRequest request)
        {
            _setSMSLogs.Action = 1;
            _setSMSLogs.Fno = request.HttpContext.Session.GetString("fno") ?? "0";
            _setSMSLogs.Mobileno = to;
            _setSMSLogs.U_NAME = to;
            _setSMSLogs.SMSReferenceno = response;
            _setSMSLogs.F1 = request.GetDisplayUrl();

            _registrationDb.SetSMSLogs(_setSMSLogs);
        }
    }

    //Extension methods must be defined in a static class
    public static class StringExtensions
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static string FormatEx(this string s, params string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                s = s.Replace("{" + i.ToString() + "}", parameters[i].ToString());
            }

            return s;
        }
    }
}
