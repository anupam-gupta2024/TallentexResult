namespace TallentexResult.Models.StoredProcedure
{
    public interface IGetCourseFee
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public int AppFormNo { get; set; }
        public int CenterID { get; set; }
        public int CCID { get; set; }
        public string AcademicSession { get; set; }
    }

    class GetCourseFee : IGetCourseFee
    {
        public string spName => "usp_GetCourseFee";   // Mandatory Declaration for stored procedure name

        public int AppFormNo { get; set; }
        public int CenterID { get; set; }
        public int CCID { get; set; }
        public string AcademicSession { get; set; }
    }
}
