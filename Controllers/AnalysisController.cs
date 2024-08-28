using Microsoft.AspNetCore.Mvc;
using System.Data;
using TallentexResult.Entities;
using TallentexResult.Interface;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Controllers
{
    // Explicitly call the baseController ctor and have each ChildController ctor inject the BaseController's dependencies
    public class AnalysisController : baseController
    {        
        private readonly IGetTopicWiseAnalysis _getTopicWiseAnalysis;

        public AnalysisController(IDataService layer, IAmazonUploader amazonUploader) : base(layer, amazonUploader)
        {
            // Assigning an object to the interface of its implemented class.
            _getTopicWiseAnalysis = new GetTopicWiseAnalysis();
        }

        public IActionResult Index()
        {
            StudentMain model = new StudentMain();

            model.ds1 = getResult();    // Fetch basic detail of student

            if (model.ds1 != null && model.ds1.Tables.Count > 0)
            {
                DataTable dt = model.ds1.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["show_result"].ToString() == "0")    // Check if result (analysis) is displayed or not.
                    {
                        return RedirectToAction("dashboard", "result");
                    }

                    model.Fno = dt.Rows[0]["appformno"].ToString() ?? "";

                    model.ds2 = getPerformanceCard();   // call Performance method to fetch result

                    if (model.ds2 != null && model.ds2.Tables.Count > 0)
                    {
                        // Serializes the datatable object to a JSON string to set TempData value
                        var serializeData = Newtonsoft.Json.JsonConvert.SerializeObject(model.ds2.Tables[0]);
                        TempData[StudentMain.Key.marks.ToString()] = serializeData;
                        TempData[StudentMain.Key.percentages.ToString()] = serializeData;
                        TempData[StudentMain.Key.score.ToString()] = serializeData;
                    }

                    if (model.ds2 != null && model.ds2.Tables.Count > 2)
                    {
                        // Serializes the datatable object to a JSON string to set TempData value
                        var serializeData = Newtonsoft.Json.JsonConvert.SerializeObject(model.ds2.Tables[2]);
                        TempData[StudentMain.Key.difficulty.ToString()] = serializeData;
                    }

                    _getTopicWiseAnalysis.AppFormNo = model.Fno;
                    model.ds3 = _layer.GetTopicWiseAnalysis(_getTopicWiseAnalysis); // Fetch Topic-wise detailed from Database
                }
            }

            return View(model);
        }

        
    }

    /// <summary>
    /// for AJAX method call in Jquery
    /// </summary>
    [SessionExpire]
    public class AnalysisAjaxController : Controller
    {
        #region Ajax Call
        /// <summary>
        /// Async Method to Display apexchart[dashboard.js] using Ajax call
        /// </summary>
        /// <returns>JSON array that contains subjects and their percentages</returns>
        public async Task<ActionResult> getPercent()
        {
            //var date = await _context.Job.Select(j => j.JobCompletion).Distinct().ToListAsync();
            //var success = _context.Job
            //    .Where(j => j.JobStatus == "Success")
            //    .GroupBy(j => j.JobCompletion)
            //    .Select(group => new
            //    {
            //        JobCompletion = group.Key,
            //        Count = group.Count()
            //    });
            //var countSuccess = success.Select(a => a.Count).ToArray();
            //var exception = _context.Job
            //    .Where(j => j.JobStatus == "Exception")
            //    .GroupBy(j => j.JobCompletion)
            //    .Select(group => new
            //    {
            //        JobCompletion = group.Key,
            //        Count = group.Count()
            //    });
            //var countException = exception.Select(a => a.Count).ToArray();

            var JsonString = TempData[StudentMain.Key.percentages.ToString()];
            if (JsonString != null)
            {
                // Deserializes the JSON string and convert it into datatable using temdata
                DataTable? dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>((string)JsonString);

                // Create a list that hold student percentage subject-wise
                List<string> percentages = await Task.FromResult(
                       new List<string>
                           {
                                (Convert.ToDecimal(dt.Rows[0]["Per"].ToString())<0 ? "0" : dt.Rows[0]["Per"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["PhyPer"].ToString())<0 ? "0" : dt.Rows[0]["PhyPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["ChemPer"].ToString())<0 ? "0" : dt.Rows[0]["ChemPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MathPer"].ToString())<0 ? "0" : dt.Rows[0]["MathPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["BioPer"].ToString())<0 ? "0" : dt.Rows[0]["BioPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MAPer"].ToString())<0 ? "0" : dt.Rows[0]["MAPer"].ToString()) ?? ""
                           });

                // Create a list of subject
                List<string> subjects = await Task.FromResult(
                        new List<string>
                            {
                               "Total",
                               "Physics",
                               "Chemistry",
                               "Maths",
                               "Biology",
                               "Mental Ability"
                            });

                return new JsonResult(new { subjects = subjects, percentages = percentages });
            }


            // return empty JSON in case of empty record
            return new JsonResult(new { subjects = string.Empty, percentages = string.Empty });
        }

        /// <summary>
        /// Async Method to Display chartjs[dashboard-chartjs-data.js] using Ajax call
        /// </summary>
        /// <returns>JSON array that contains subjects, marks and their percentages</returns>
        public async Task<ActionResult> getMarks()
        {
            var JsonString = TempData[StudentMain.Key.marks.ToString()];
            if (JsonString != null)
            {
                // Deserializes the JSON string and convert it into datatable using temdata
                DataTable? dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>((string)JsonString);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Create a list that hold student marks subject-wise
                    List<string> marks = await Task.FromResult(
                        new List<string>
                            {
                                dt.Rows[0]["Phy"].ToString() ?? "",
                                dt.Rows[0]["Chem"].ToString() ?? "",
                                dt.Rows[0]["Math"].ToString() ?? "",
                                dt.Rows[0]["Bio"].ToString() ?? "",
                                dt.Rows[0]["MA"].ToString() ?? ""
                            });

                    // Create a list that hold student percentage subject-wise
                    List<string> percentages = await Task.FromResult(
                       new List<string>
                           {
                                (Convert.ToDecimal(dt.Rows[0]["PhyPer"].ToString())<0 ? "0" : dt.Rows[0]["PhyPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["ChemPer"].ToString())<0 ? "0" : dt.Rows[0]["ChemPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MathPer"].ToString())<0 ? "0" : dt.Rows[0]["MathPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["BioPer"].ToString())<0 ? "0" : dt.Rows[0]["BioPer"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MAPer"].ToString())<0 ? "0" : dt.Rows[0]["MAPer"].ToString()) ?? ""
                           });

                    // Create a list of subject
                    List<string> subjects = await Task.FromResult(
                        new List<string>
                            {
                               "Physics",
                               "Chemistry",
                               "Maths",
                               "Biology",
                               "Mental Ability"
                            });

                    return new JsonResult(new { subjects = subjects, marks = marks, percentages = percentages });
                }
            }

            // return empty JSON in case of empty record
            return new JsonResult(new { subjects = string.Empty, marks = string.Empty, percentages = string.Empty });
        }

        /// <summary>
        /// Async Method to Display highchart[analysishighchart.js] using Ajax call
        /// </summary>
        /// <returns>JSON array that contains subjects, & your, topper, and average marks for comparision</returns>
        public async Task<ActionResult> getScore()
        {
            var JsonString = TempData[StudentMain.Key.score.ToString()];
            if (JsonString != null)
            {
                // Deserializes the JSON string and convert it into datatable using temdata
                DataTable? dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>((string)JsonString);

                // Create a list of subject
                List<string> subjects = await Task.FromResult(
                         new List<string>
                             {
                               "Physics",
                               "Chemistry",
                               "Maths",
                               "Biology",
                               "Mental Ability",
                                "Total"
                             });

                // Create a list that hold student marks subject-wise
                List<string> your = await Task.FromResult(
                       new List<string>
                           {
                                (Convert.ToDecimal(dt.Rows[0]["Phy"].ToString())<0 ? "0" : dt.Rows[0]["Phy"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["Chem"].ToString())<0 ? "0" : dt.Rows[0]["Chem"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["Math"].ToString())<0 ? "0" : dt.Rows[0]["Math"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["Bio"].ToString())<0 ? "0" : dt.Rows[0]["Bio"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MA"].ToString())<0 ? "0" : dt.Rows[0]["MA"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["Total"].ToString())<0 ? "0" : dt.Rows[0]["Total"].ToString()) ?? ""
                           });

                // Create a list that hold topper marks subject-wise
                List<string> topper = await Task.FromResult(
                       new List<string>
                           {
                                (Convert.ToDecimal(dt.Rows[0]["PhyH"].ToString())<0 ? "0" : dt.Rows[0]["PhyH"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["ChemH"].ToString())<0 ? "0" : dt.Rows[0]["ChemH"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MathH"].ToString())<0 ? "0" : dt.Rows[0]["MathH"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["BioH"].ToString())<0 ? "0" : dt.Rows[0]["BioH"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MaH"].ToString())<0 ? "0" : dt.Rows[0]["MaH"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["TotalH"].ToString())<0 ? "0" : dt.Rows[0]["TotalH"].ToString()) ?? ""
                           });

                // Create a list that hold average marks subject-wise
                List<string> average = await Task.FromResult(
                       new List<string>
                           {
                                (Convert.ToDecimal(dt.Rows[0]["PhyAvg"].ToString())<0 ? "0" : dt.Rows[0]["PhyAvg"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["ChemAvg"].ToString())<0 ? "0" : dt.Rows[0]["ChemAvg"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MathAvg"].ToString())<0 ? "0" : dt.Rows[0]["MathAvg"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["BioAvg"].ToString())<0 ? "0" : dt.Rows[0]["BioAvg"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["MaAvg"].ToString())<0 ? "0" : dt.Rows[0]["MaAvg"].ToString()) ?? "",
                                (Convert.ToDecimal(dt.Rows[0]["TotalAvg"].ToString())<0 ? "0" : dt.Rows[0]["TotalAvg"].ToString()) ?? ""
                           });

                return new JsonResult(new
                {
                    subjects = subjects,
                    your = your,
                    topper = topper,
                    average = average
                });
            }

            // return empty JSON in case of empty record
            return new JsonResult(new
            {
                subjects = string.Empty,
                your = string.Empty,
                topper = string.Empty,
                average = string.Empty
            });
        }

        /// <summary>
        /// Async Method to Display highchart[analysishighchart.js] using Ajax call
        /// </summary>
        /// <returns>JSON array that contains difficulty level</returns>
        public async Task<ActionResult> getDifficulty()
        {
            var JsonString = TempData[StudentMain.Key.difficulty.ToString()];
            if (JsonString != null)
            {
                // Deserializes the JSON string and convert it into datatable using temdata
                DataTable? dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>((string)JsonString);

                // Get a value of Difficult level
                var difficult_ = await Task.FromResult(
                    dt.AsEnumerable()
                    .Where(r => r.Field<string>("LevelTitle") == "Difficult Level")
                    .Select(r => r.Field<Int64>("LevelValue")).FirstOrDefault()
                    );

                // Get a value of Moderate level
                var moderate_ = await Task.FromResult(
                   dt.AsEnumerable()
                   .Where(r => r.Field<string>("LevelTitle") == "Moderate Level")
                   .Select(r => r.Field<Int64>("LevelValue")).FirstOrDefault()
                   );

                // Get a value of Easy level
                var easy_ = await Task.FromResult(
                   dt.AsEnumerable()
                   .Where(r => r.Field<string>("LevelTitle") == "Easy Level")
                   .Select(r => r.Field<Int64>("LevelValue")).FirstOrDefault()
                   );

                return new JsonResult(new
                {
                    difficult = difficult_,
                    moderate = moderate_,
                    easy = easy_
                });
            }

            // return empty JSON in case of empty record
            return new JsonResult(new
            {
                difficult = string.Empty,
                moderate = string.Empty,
                easy = string.Empty
            });
        }
        #endregion
    }
}
