using System.Data;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Interface
{
    public interface IAdmissionInfo
    {
        Task<DataSet> GetScholarship(IGetScholarship getScholarship);
        Task<DataSet> GetCourse(IGetCourse getCourse);
        Task<DataSet> GetCourseFee(IGetCourseFee getCourseFee);
    }
}
