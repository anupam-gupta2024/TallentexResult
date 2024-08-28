using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TallentexResult.Interface;
using TallentexResult.Models.StoredProcedure;
using TallentexResult.Servies;

namespace TallentexResult.Controllers
{
    /// <summary>
    /// Base Controller to avoid redundant DI code
    /// </summary>
    [SessionExpire]
    public class baseController : Controller
    {
        protected readonly IDataService _layer;     // injecting the dataservice
        protected readonly IAmazonUploader _amazonUploader;     // injecting the aws service

        protected readonly IGetResult _getResult;

        public baseController(IDataService layer, IAmazonUploader amazonUploader)   // using interfaces in Dependency Injection
        {
            _layer = layer;
            _amazonUploader = amazonUploader;

            // Assigning an object to the interface of its implemented class.
            _getResult = new GetResult();
        }

        /// <summary>
        /// Fetch the basic detail of student
        /// </summary>
        /// <returns>Dataset contain all necessary detail of student</returns>
        protected DataSet? getResult()
        {
            _getResult.AppFormNo = HttpContext.Session.GetString("fno") ?? "";  // get current Student Roll no. from session variable
            DataSet ds = _layer.GetResult(_getResult);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region Fetch the student photo from AWS cloud
                    string srcphoto = _amazonUploader.srcphoto;
                    bool isExists = _amazonUploader.S3Exists(string.Format(srcphoto, dt.Rows[0]["photopath"])).Result;
                    if (isExists)
                        ViewBag.Photo = Url.Content(_amazonUploader.GeneratePreSignedURL(String.Format(srcphoto, dt.Rows[0]["photopath"])).Result);
                    else
                        ViewBag.Photo = LNResources.NoImage;
                    #endregion

                    ViewBag.Fno = dt.Rows[0]["appformno"];
                    ViewBag.Name = new Global().Capitalize(dt.Rows[0]["StudentName"].ToString() ?? "");
                    ViewBag.ShowResult = dt.Rows[0]["show_result"].ToString();  // get the show_result{i.e. 1-display, 0->hide} flag and pass it in ViewBag
                }
            }

            return ds;
        }

        /// <summary>
        /// Fetch Marks, CSI & Difficulty Level from Database
        /// </summary>
        /// <returns>Dataset contain all Marks, CSI & Difficulty Level</returns>
        protected DataSet getPerformanceCard()
        {
            IGetPerformanceCard _getPerformanceCard = new GetPerformanceCard();  // Assigning an object to the interface of its implemented class.

            _getPerformanceCard.AppFormNo = HttpContext.Session.GetString("fno") ?? "";  // get current Student Roll no. from session variable

            return _layer.GetPerformanceCard(_getPerformanceCard);
        }
    }
}
