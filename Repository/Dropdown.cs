using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Data;
using TallentexResult.Interface;
using TallentexResult.Models;
using TallentexResult.Models.StoredProcedure;
using TallentexResult.Servies;

namespace TallentexResult.Repository
{
    public class Dropdown : IDropdown
    {
        private readonly IResultDb _resultDb;
        private readonly IGetCenter _getCenter;

        public Dropdown(IOptions<DataConnection> options)
        {
            _resultDb = new ResultDb(options.Value.ResultConnection);
            _getCenter = new GetCenter();
        }

        public List<SelectListItem> getMode()
        {
            _getCenter.Action = 6;
            DataSet ds = _resultDb.GetCenter(_getCenter);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<SelectListItem> lst = new List<SelectListItem>();
                    lst.Add(new SelectListItem { Text = "Please Select", Value = "" });
                    foreach (DataRow row in dt.Rows)
                    {
                        lst.Add(new SelectListItem { Text = row["coursetype"].ToString(), Value = row["coursetypeid"].ToString() });
                    }
                    return lst;
                }
            }

            return new List<SelectListItem>();
        }

        public List<SelectListItem> getCenter()
        {
            _getCenter.Action = 4;
            DataSet ds = _resultDb.GetCenter(_getCenter);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<SelectListItem> lst = new List<SelectListItem>();
                    lst.Add(new SelectListItem { Text = "Please Select", Value = "" });
                    foreach (DataRow row in dt.Rows)
                    {
                        lst.Add(new SelectListItem { Text = new Global().Capitalize(row["CenterName"].ToString() ?? ""), Value = row["CenterID"].ToString() });
                    }
                    return lst;
                }
            }

            return new List<SelectListItem>();
        }
    }
}
