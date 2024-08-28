namespace TallentexResult.Models.StoredProcedure
{
    public interface IGetData
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public int Action { get; set; }
        public string FNO { get; set; }
        public string AccName { get; set; }
        public string AccNo { get; set; }
        public string BankName { get; set; }
        public string BranchAdd { get; set; }
        public string IFSC { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
    }

    class GetData : IGetData
    {
        public string spName => "asp_getdata";   // Mandatory Declaration for stored procedure name

        public int Action { get; set; }
        public string FNO { get; set; }
        public string AccName { get; set; }
        public string AccNo { get; set; }
        public string BankName { get; set; }
        public string BranchAdd { get; set; }
        public string IFSC { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
    }
}
