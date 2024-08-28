namespace TallentexResult.Models.StoredProcedure
{
    public interface IGetCourse
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public int AppFormNo { get; set; }
        public int CenterID { get; set; }
    }

    class GetCourse : IGetCourse
    {
        public string spName => "usp_GetCourse";   // Mandatory Declaration for stored procedure name

        public int AppFormNo { get; set; }
        public int CenterID { get; set; }
    }
}
