using Amazon.Util.Internal;
using System.ComponentModel.DataAnnotations;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Entities
{
    public class Verification
    {
        public string fno { get; set; }

        [Required(ErrorMessage = "Please Select Document Type for Student in dropdown")]
        public string documenttypeicard { get; set; }
        [Required(ErrorMessage = "Please upload document for Student Verification")]
        public IFormFile filesicard { get; set; }

        [Required(ErrorMessage = "Please Select Document Type for Parent in dropdown")]
        public string documenttypepicard { get; set; }
        [Required(ErrorMessage = "Please upload document for Parent Verification")]
        public IFormFile filespicard { get; set; }

        [Required(ErrorMessage = "Please upload Marksheet for Class Verification")]
        public IFormFile filesmarksheet { get; set; }

        [Required(ErrorMessage = "Please Select Cancelled Cheque or Passbook in dropdown")]
        public string documenttypebankaccount { get; set; }
        [Required(ErrorMessage = "Please upload Cancelled Cheque or Passbook for Student Bank account")]
        public IFormFile filesbankaccount { get; set; }

        public RTGS rtgs { get; set; }

        public IGetData setData()
        {
            IGetData data = new GetData();

            data.FNO = fno;
            data.AccName = rtgs.AcountHolderName;
            data.AccNo = rtgs.ACNO;
            data.BankName = rtgs.BankName;
            data.BranchAdd = rtgs.BranchAdd;
            data.IFSC = rtgs.IFSC;
            data.Email = rtgs.Email;
            data.Email2 = rtgs.Email2;

            data.Remark1 = documenttypeicard;
            data.Remark2 = documenttypepicard;
            data.Remark3 = documenttypebankaccount;

            return data;
        }

    }

    public class RTGS
    {
        [Required]
        public string AcountHolderName { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "Please type 11 character IFSC Code", MinimumLength = 4)]
        public string IFSC { get; set; }

        [Required]
        public string BankName { get; set; }
        [Required]
        public string BranchAdd { get; set; }

        [Required(ErrorMessage = "Account No is required")]
        [RegularExpression("^\\d{9,18}$", ErrorMessage = "Please type Account Number length varies from 9 digits to 18 digits.")]
        public string ACNO { get; set; }

        [Required]
        public string Email { get; set; }
        public string Email2 { get; set; }
    }
}
