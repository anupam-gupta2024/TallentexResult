using System.Data;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Interface
{
    public interface IDataService
    {
        DataSet AuthenticateStudentResult(IAuthenticateStudentResult authenticateStudentResult);
        DataSet GetResult(IGetResult getResult);
        DataSet GetPerformanceCard(IGetPerformanceCard getPerformanceCard);
        DataSet GetData(IGetData getData);
        DataSet GetTopicWiseAnalysis(IGetTopicWiseAnalysis getTopicWiseAnalysis);
    }
}
