using System;
using Oracle.ManagedDataAccess.Client; // This namespace is for Oracle connection
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBV
{
    internal class dbConnect : IDisposable
    {
        private OracleConnection cn;

        public dbConnect()
        {
            // Initialize the OracleConnection object with the connection string
            cn = new OracleConnection(ConnectionString());
        }

        // This method constructs the connection string
        private string ConnectionString()
        {
            // Use the details from the Oracle SQL Developer connection setup
            string host = "localhost"; // The hostname where Oracle is running
            string port = "1521"; // The port Oracle is listening on
            string serviceName = "orcl21"; // The service name of your Oracle database
            string user = "system"; // Your Oracle username
            string password = "123456"; // Your Oracle password
                                        // Construct the Oracle connection string
            return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port})))(CONNECT_DATA=(SERVICE_NAME={serviceName})));User Id={user};Password={password};";
        }

        // This method can be used to open the connection
        public void OpenConnection()
        {
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine(ex.Message);
            }
        }

        // This method can be used to close the connection
        public void CloseConnection()
        {
            if (cn.State == ConnectionState.Open)
                cn.Close();
        }

        public OracleConnection GetConnection()
        {
            return cn;
        }

        // This method checks the login credentials
        public bool CheckLogin(string username, string password)
        {
            bool isAuthenticated = false;
            try
            {
                OpenConnection();
                string query = "SELECT COUNT(1) FROM TAIKHOAN WHERE TaiKhoan = :username AND MatKhau = :password";
                using (OracleCommand cmd = new OracleCommand(query, cn))
                {
                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("password", OracleDbType.Varchar2).Value = password;
                    isAuthenticated = Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return isAuthenticated;
        }

        // IDisposable implementation
        public void Dispose()
        {
            if (cn != null)
            {
                cn.Dispose();
            }
        }
    }

}