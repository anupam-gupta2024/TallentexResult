namespace TallentexResult.Models.StoredProcedure
{
    public interface IGetPerformanceCard
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public string AppFormNo { get; set; }
    }

    class GetPerformanceCard : IGetPerformanceCard
    {
        public string spName => "usp_2015_GetPerformanceCard";   // Mandatory Declaration for stored procedure name

        public string AppFormNo { get; set; }
    }
}
