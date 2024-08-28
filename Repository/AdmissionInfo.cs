using Microsoft.Extensions.Options;
using System.Data;
using TallentexResult.Interface;
using TallentexResult.Models.StoredProcedure;
using TallentexResult.Models;

namespace TallentexResult.Repository
{
    public class AdmissionInfo : IAdmissionInfo
    {
        private readonly IResultDb _resultDb;

        public AdmissionInfo(IOptions<DataConnection> options)
        {
            _resultDb = new ResultDb(options.Value.ResultConnection);
        }
       
        public async Task<DataSet> GetScholarship(IGetScholarship getScholarship)
        {
            return await _resultDb.GetScholarship(getScholarship);
        }

        public async Task<DataSet> GetCourse(IGetCourse getCourse)
        {
            return await _resultDb.GetCourse(getCourse);
        }

        public async Task<DataSet> GetCourseFee(IGetCourseFee getCourseFee)
        {
            return await _resultDb.GetCourseFee(getCourseFee);
        }
    }
}
