using System.Data;
using TallentexResult.Interface;
using TallentexResult.Models.StoredProcedure;
using TallentexResult.Servies;

namespace TallentexResult.Repository
{
    public class ResultDb : AdoRepository, IResultDb
    {
        public ResultDb(string connectionString) : base(connectionString)
        {
        }

        public DataSet AuthenticateStudentResult(IAuthenticateStudentResult authenticateStudentResult)
        {
            return callStoredProcedure(authenticateStudentResult);
        }

        public DataSet GetResult(IGetResult getResult)
        {
            return callStoredProcedure(getResult);
        }

        public DataSet GetPerformanceCard(IGetPerformanceCard getPerformanceCard)
        {
            return callStoredProcedure(getPerformanceCard);
        }

        public DataSet GetData(IGetData getData)
        {
            return callStoredProcedure(getData);
        }

        public DataSet GetTopicWiseAnalysis(IGetTopicWiseAnalysis getTopicWiseAnalysis) 
        {
            return callStoredProcedure(getTopicWiseAnalysis);
        }

        public DataSet GetCenter(IGetCenter getCenter)
        {
            return callStoredProcedure(getCenter);
        }

        public async Task<DataSet> GetScholarship(IGetScholarship getScholarship)
        {
            return await callStoredProcedureAsync(getScholarship);
        }

        public async Task<DataSet> GetCourse(IGetCourse getCourse)
        {
            return await callStoredProcedureAsync(getCourse);
        }

        public async Task<DataSet> GetCourseFee(IGetCourseFee getCourseFee)
        {
            return await callStoredProcedureAsync(getCourseFee);
        }
    }
}
