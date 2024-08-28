namespace TallentexResult.Models.StoredProcedure
{
    public interface IGetCenter
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        public int Action { get; set; }
        public int CenterID { get; set; }       
    }

    class GetCenter : IGetCenter
    {
        public string spName => "cp_GetCenter";   // Mandatory Declaration for stored procedure name

        public int Action { get; set; }
        public int CenterID { get; set; }
    }
}
