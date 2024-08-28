namespace TallentexResult.Models.StoredProcedure
{
    public interface IAuthenticateStudentResult
    {
        protected string spName { get; }    // Mandatory Declaration for stored procedure name

        string AppFormNo { get; set; }
        public string StudentName { get; set; }
        public string IP { get; set; }
    }

    class AuthenticateStudentResult : IAuthenticateStudentResult
    {
        public string spName => "usp_AuthenticateStudentResult";   // Mandatory Declaration for stored procedure name

        public string AppFormNo { get; set; }
        public string StudentName { get; set; }
        public string IP { get; set; }
    }
}
