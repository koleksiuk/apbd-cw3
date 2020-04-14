using System.Data.SqlClient;

namespace cw3.DAL.MsSql
{
    public class BaseDbService
    {
        private readonly string _connectionDetails =
    "Server=localhost,1433;Database=university;User Id=SA;Password=Pass@word;";
        protected readonly SqlConnection Connection;

        public BaseDbService()
        {
            Connection = new SqlConnection(_connectionDetails);
        }
    }
}
