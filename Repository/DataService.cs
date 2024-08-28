using Microsoft.Extensions.Options;
using System.Data;
using TallentexResult.Interface;
using TallentexResult.Models;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Repository
{
    /// <summary>
    /// The Dependency Inversion Principle (DIP) focuses on decoupling high-level modules from low-level modules by introducing an abstraction layer, 
    /// with the use of interfaces or abstract classes and reducing direct dependencies between classes.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly IResultDb _resultDb;

        public DataService(IOptions<DataConnection> options)
        {
            _resultDb = new ResultDb(options.Value.ResultConnection);
        }

        public DataSet AuthenticateStudentResult(IAuthenticateStudentResult authenticateStudentResult)
        {
            return _resultDb.AuthenticateStudentResult(authenticateStudentResult);
        }

        public DataSet GetResult(IGetResult getResult)
        {
            return _resultDb.GetResult(getResult);
        }

        public DataSet GetPerformanceCard(IGetPerformanceCard getPerformanceCard)
        {
            return _resultDb.GetPerformanceCard(getPerformanceCard);
        }

        public DataSet GetData(IGetData getData)
        {
            return _resultDb.GetData(getData);
        }

        public DataSet GetTopicWiseAnalysis(IGetTopicWiseAnalysis getTopicWiseAnalysis)
        {
            return _resultDb.GetTopicWiseAnalysis(getTopicWiseAnalysis);
        }
    }
}
