namespace TallentexResult.Models.StoredProcedure
{

    public interface IGetResult
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public string AppFormNo { get; set; }
    }

    class GetResult : IGetResult
    {
        public string spName => "usp_2015_GetResult";   // Mandatory Declaration for stored procedure name

        public string AppFormNo { get; set; }
    }
}
