using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TallentexResult.Entities;
using TallentexResult.Interface;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Controllers
{
    // Explicitly call the baseController ctor and have each ChildController ctor inject the BaseController's dependencies
    public class ResultController : baseController
    {
        public ResultController(IDataService layer, IAmazonUploader amazonUploader) : base(layer, amazonUploader)
        {
        }

        public IActionResult dashboard()
        {
            StudentMain model = new StudentMain();

            model.ds1 = getResult();    // Fetch basic detail of student

            if (model.ds1 != null && model.ds1.Tables.Count > 0)
            {
                DataTable dt = model.ds1.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    model.Fno = dt.Rows[0]["appformno"].ToString() ?? "";

                    #region RTGS -- Get the prefilled RTGS details
                    if (Convert.ToInt32(dt.Rows[0]["Cash"].ToString()) > 0 && (Convert.ToInt32(dt.Rows[0]["IsSend"].ToString()) == 1))
                    {
                        ViewBag.DocUpload = "1";    // set the ViewBag to display the docupload message in _layout

                        IGetData getData = new GetData();   // It is assigning an getdata object of its implemented class.

                        getData.Action = 100;
                        getData.FNO = model.Fno.ToString();
                        DataSet dsDocument = _layer.GetData(getData);   // Fetch RTGS detail

                        if (dsDocument != null && dsDocument.Tables.Count > 0)
                        {
                            DataTable dtDocument = dsDocument.Tables[0];
                            if (dtDocument != null && dtDocument.Rows.Count > 0)
                            {
                                ViewBag.isicard = dtDocument.Rows[0]["ICard"];
                                ViewBag.ismarksheet = dtDocument.Rows[0]["MarkSheet"];
                                ViewBag.ispicard = dtDocument.Rows[0]["PICard"];
                                ViewBag.isbankaccount = dtDocument.Rows[0]["BankAccount"];
                                ViewBag.isRTGS = dtDocument.Rows[0]["RTGS"];
                                ViewBag.isSend = dtDocument.Rows[0]["isSend"];


                                if (model.verify == null) model.verify = new Verification();
                                if (model.verify.rtgs == null) model.verify.rtgs = new RTGS();

                                model.verify.documenttypeicard = dtDocument.Rows[0]["ICardType"].ToString() ?? "";
                                model.verify.documenttypepicard = dtDocument.Rows[0]["PICardType"].ToString() ?? "";
                                model.verify.documenttypebankaccount = dtDocument.Rows[0]["BankAccountType"].ToString() ?? "";

                                model.verify.rtgs.Email = dtDocument.Rows[0]["email1"].ToString() ?? "";
                                model.verify.rtgs.Email2 = dtDocument.Rows[0]["email2"].ToString() ?? "";

                                model.verify.fno = dtDocument.Rows[0]["Fno"].ToString() ?? "";
                                model.verify.rtgs.AcountHolderName = dtDocument.Rows[0]["AccName"].ToString() ?? "";
                                model.verify.rtgs.ACNO = dtDocument.Rows[0]["AccNo"].ToString() ?? "";
                                model.verify.rtgs.BankName = dtDocument.Rows[0]["BankName"].ToString() ?? "";
                                model.verify.rtgs.BranchAdd = dtDocument.Rows[0]["BranchAdd"].ToString() ?? "";
                                model.verify.rtgs.IFSC = dtDocument.Rows[0]["IFSC"].ToString() ?? "";
                            }
                        }
                    }
                    #endregion
                }
            }

            model.ds2 = getPerformanceCard();   // call Performance method to fetch result

            if (model.ds2 != null && model.ds2.Tables.Count > 0)
            {
                // Serializes the datatable object to a JSON string to set TempData value
                var serializeData = Newtonsoft.Json.JsonConvert.SerializeObject(model.ds2.Tables[0]);
                TempData[StudentMain.Key.marks.ToString()] = serializeData;
                TempData[StudentMain.Key.percentages.ToString()] = serializeData;
            }

            return View(model);
        }


        /// <summary>
        /// Async Method to insert/update the document & RTGS detail using Ajax call
        /// </summary>
        /// <param name="verify">Pass the doucment type & account details</param>
        /// <returns>Message for insertion/updation</returns>
        [HttpPost]
        public async Task<IActionResult> updDetail(Verification verify)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isIcard = false;
                    /********************** Upload Student Document in AWS****************************************/
                    if (verify.filesicard != null && verify.filesicard.Length > 0)
                        isIcard = await _amazonUploader.UploadFileToS3(verify.filesicard, String.Format(_amazonUploader.srcdoc, "icard/" + verify.fno));

                    /********************** Upload Parent Document in AWS****************************************/
                    bool isPicard = false;
                    if (verify.filespicard != null && verify.filespicard.Length > 0)
                        isPicard = await _amazonUploader.UploadFileToS3(verify.filespicard, String.Format(_amazonUploader.srcdoc, "picard/" + verify.fno));

                    /********************** Upload Marksheet in AWS****************************************/
                    bool isMarksheet = false;
                    if (verify.filesmarksheet != null && verify.filesmarksheet.Length > 0)
                        isMarksheet = await _amazonUploader.UploadFileToS3(verify.filesmarksheet, String.Format(_amazonUploader.srcdoc, "marksheet/" + verify.fno));

                    /********************** Upload Passbook/Cheque in AWS****************************************/
                    bool isBankAccount = false;
                    if (verify.filesbankaccount != null && verify.filesbankaccount.Length > 0)
                        isBankAccount = await _amazonUploader.UploadFileToS3(verify.filesbankaccount, String.Format(_amazonUploader.srcdoc, "bankaccount/" + verify.fno));

                    // Chaek if all document upload properly
                    if (isIcard && isPicard && isMarksheet && isBankAccount)
                    {
                        IGetData getData = verify.setData();    // Set the FormCollection Data & assign it to SP object
                        getData.Action = 1;

                        DataSet ds = _layer.GetData(getData);   // Insert/Update the details that was assigned to the SP object.
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            if (dt != null && dt.Rows.Count > 0)
                                return Ok(dt.Rows[0]["Message"].ToString());

                        }
                        // return an error if detail is not insert/update properly in database
                        return BadRequest("There is some problem occurs, Please try again or contact TALLENTEX Coordination Cell for assistance.");
                    }
                    else
                        // return an error if all document is not uploaded
                        return BadRequest("Document not uploaded, some error occured.");

                }
                catch (Exception ex) { return BadRequest(ex.Message); }
            }
            else
            {
                //  display an error if parameter is not valid
                var message = string.Join("</li>", ModelState.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(e => "<li>" + e.ErrorMessage));
                message = "<ol>" + message + "</li></ol>";


                return BadRequest(message);
            }
        }
    }
}
