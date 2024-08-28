using System.Data;
using TallentexResult.Models.StoredProcedure;

namespace TallentexResult.Interface
{
    public interface IRegistrationDb
    {
        DataSet SetSMSLogs(ISetSMSLogs smsLogs);
    }
}
