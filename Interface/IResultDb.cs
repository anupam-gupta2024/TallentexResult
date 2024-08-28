using System.Data;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Interface
{
    public interface IResultDb
    {
        DataSet AuthenticateStudentResult(IAuthenticateStudentResult authenticateStudentResult);
        DataSet GetResult(IGetResult getResult);
        DataSet GetPerformanceCard(IGetPerformanceCard getPerformanceCard);
        DataSet GetData(IGetData getData);
        DataSet GetTopicWiseAnalysis(IGetTopicWiseAnalysis getTopicWiseAnalysis);
        DataSet GetCenter(IGetCenter getCenter);
        Task<DataSet> GetScholarship(IGetScholarship getScholarship);
        Task<DataSet> GetCourse(IGetCourse getCourse);
        Task<DataSet> GetCourseFee(IGetCourseFee getCourseFee);
    }
}
