namespace TallentexResult.Models.StoredProcedure
{
    public interface IGetTopicWiseAnalysis
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public string AppFormNo { get; set; }
    }

    class GetTopicWiseAnalysis : IGetTopicWiseAnalysis
    {
        public string spName => "usp_GetTopicWiseAnalysis";   // Mandatory Declaration for stored procedure name

        public string AppFormNo { get; set; }
    }
}
