using Microsoft.SqlServer.Server;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using TallentexResult.Controllers;
using TallentexResult.Entities;
using TallentexResult.Interface;
using TallentexResult.Servies;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace TallentexResult.Models.StoredProcedure
{
    public interface ISetSMSLogs
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        int Action { get; set; }
        public string Templateid { get; set; }
        public string Fno { get; set; }
        public string Mobileno { get; set; }
        public string Messagetext { get; set; }
        public string Appname { get; set; }
        public string U_NAME { get; set; }
        public string SMSReferenceno { get; set; }
        public string F1 { get; set; }
    }

    class SetSMSLogs : ISetSMSLogs
    {
        public string spName => "usp_SetSMSLogs";   // Mandatory Declaration for stored procedure name

        public int Action { get; set; }
        public string Templateid { get; set; }
        public string Fno { get; set; }
        public string Mobileno { get; set; }
        public string Messagetext { get; set; }
        public string Appname { get; set; }
        public string U_NAME { get; set; }
        public string SMSReferenceno { get; set; }
        public string F1 { get; set; }
    }
}
