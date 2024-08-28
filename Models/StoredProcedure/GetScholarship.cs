namespace TallentexResult.Models.StoredProcedure
{
    public interface IGetScholarship
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public int AppFormNo { get; set; }
        public int CenterID { get; set; }
        public string AcademicSession { get; set; }
    }

    class GetScholarship : IGetScholarship
    {
        public string spName => "usp_GetScholarship";   // Mandatory Declaration for stored procedure name

        public int AppFormNo { get; set; }
        public int CenterID { get; set; }
        public string AcademicSession { get; set; }
    }
}
