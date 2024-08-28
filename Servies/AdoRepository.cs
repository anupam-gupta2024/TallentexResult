using System.Data;
using System.Data.SqlClient;

namespace TallentexResult.Servies
{
    /// <summary>
    /// ActiveX Data Objects class for database call
    /// </summary>
    public abstract class AdoRepository
    {
        private readonly string _connectionString;

        public AdoRepository(string connectionString)
        {
            // Get Connection string from inherited class Constructor
            _connectionString = connectionString;
        }

        protected DataSet callStoredProcedure(string spName, object obj)
        {
            IDictionary<string, object?> parameters = getParameter(obj);
            return Execute(spName, parameters);
        }

        protected DataSet callStoredProcedure(object obj)
        {
            string spName = string.Empty;

            IDictionary<string, object?> parameters = getParameter(obj);
            if (parameters != null && parameters.Count > 0 && parameters.ContainsKey("spName"))
            {
                spName = $"{parameters["spName"]}"; // using interpolation to Resolve nullable warnings.
                parameters.Remove("spName");
            }

            if (string.IsNullOrEmpty(spName)) { throw new ArgumentNullException("In a stored procedure call, the {spName} field is mandatory and cannot be left empty."); }

            return Execute(spName, parameters);
        }

        protected async Task<DataSet> callStoredProcedureAsync(object obj)
        {
            string spName = string.Empty;

            IDictionary<string, object?> parameters = getParameter(obj);
            if (parameters != null && parameters.Count > 0 && parameters.ContainsKey("spName"))
            {
                spName = $"{parameters["spName"]}"; // using interpolation to Resolve nullable warnings.
                parameters.Remove("spName");
            }

            if (string.IsNullOrEmpty(spName)) { throw new ArgumentNullException("In a stored procedure call, the {spName} field is mandatory and cannot be left empty."); }

            return await ExecuteAsync(spName, parameters);
        }


        #region Peoperties
        // Convert Class properties into Dictionary
        private IDictionary<string, object?> getParameter(object obj)
        {
            // Fluent Builder Design Pattern : It is based on the concept of method chaining, where each method returns an object itself, allowing multiple actions to be invoked in a single statement sequence.
            return obj.GetType().GetProperties()
                .Where(prop => prop.GetValue(obj) != null)  // select only non-nullable properties
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null) as object);
        }

        /// <summary>
        /// Execute Stored Proc with result set returned.
        /// </summary>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="parameters">SP Parameter</param>
        /// <returns>DataSet</returns>
        private DataSet Execute(string spName, IDictionary<string, object?>? parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(_connectionString))  // Creating Connection
            {
                cn.Open();

                using (var cmd = new SqlCommand(spName, cn))    // Create a SqlCommand to execute the stored procedure.
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //System.Diagnostics.Debug.WriteLine(cmd.Connection.ConnectionTimeout);

                    if (parameters != null && parameters.Count > 0) 
                    {
                        foreach (var kvp in parameters)
                        {
                            cmd.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);  // Passing parameter values
                        }
                    }

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// Execute Async Stored Proc with result set returned.
        /// </summary>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="parameters">SP Parameter</param>
        /// <returns>DataSet</returns>
        private async Task<DataSet> ExecuteAsync(string spName, IDictionary<string, object?>? parameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cn = new SqlConnection(_connectionString)) // Creating Connection
            {
                await cn.OpenAsync().ConfigureAwait(false);

                using (var cmd = new SqlCommand(spName, cn))    // Create a SqlCommand to execute the stored procedure.
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    System.Diagnostics.Debug.WriteLine(cmd.Connection.ConnectionTimeout);

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var kvp in parameters)
                        {
                            cmd.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);  // Passing parameter values
                        }
                    }

                    using (var da =  new SqlDataAdapter(cmd))
                    {
                        await Task.Run(() => da.Fill(ds));
                    }
                }
            }

            return ds;
        }
        #endregion
    }
}
