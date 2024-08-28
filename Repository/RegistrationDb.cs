using System.Data;
using TallentexResult.Interface;
using TallentexResult.Models.StoredProcedure;
using TallentexResult.Servies;

namespace TallentexResult.Repository
{
    public class RegistrationDb : AdoRepository, IRegistrationDb
    {
        public RegistrationDb(string connectionString) : base(connectionString)
        {
        }

        public DataSet SetSMSLogs(ISetSMSLogs smsLogs)
        {
            return callStoredProcedure(smsLogs);
        }
    }
}
