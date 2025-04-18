using GroupExpenseManagement01.BAL;
using System.Data.SqlClient;
using System.Data;

namespace GroupExpenseManagement01.CommonClasses
{
    public class CommonFuntions
    {
        #region Connection String
        private static string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("ConnectionString");
        #endregion

        #region ReceivableAmout
        public static decimal GetReceivableAmout()
        {
            decimal count = 0;
            
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Contribution_Receivable";
                    sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();

                    object result = sqlCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        count = Convert.ToDecimal(result);
                    }
                    else
                    {
                        count = 0; // Or whatever default you want when nothing comes back
                    }
                }
            }

            return count;

        }
        #endregion

        #region PayableAmout
        public static decimal GetPayableAmout()
        {
            decimal count = 0;
            
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Contribution_Payable";
                    sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();

                    object result = sqlCommand.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        count = Convert.ToDecimal(result);
                    }
                    else
                    {
                        count = 0; // Handle null scenario by setting count to 0 or some default value
                    }
                }
            }
            return count;
        }
        #endregion

        #region User Wise Currencies Total
        public static decimal[] UserWiseCurrenciesTotal()
        {
            decimal[] total = new decimal[3];
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_CurrenciesTotal";
                    sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        // First result set - Total INR
                        if (reader.Read())
                        {
                            total[0] = Convert.ToDecimal(reader["TotalINR"]);
                        }

                        // Move to the second result set - Total USD
                        if (reader.NextResult() && reader.Read())
                        {
                            total[1] = Convert.ToDecimal(reader["TotalUSD"]);
                        }

                        // Move to the third result set - Total EUR
                        if (reader.NextResult() && reader.Read())
                        {
                            total[2] = Convert.ToDecimal(reader["TotalEUR"]);
                        }
                    }
                }
            }
            return total;
        }
        #endregion

        public static decimal[] UserWiseBalanceTotal()
        {
            decimal[] total = new decimal[2]; // Index 0 = Total Liabilities, Index 1 = Total Assets
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Balance_ByUser2_Total";
                    sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        // First result set - Total Liabilities
                        if (reader.Read())
                        {
                            total[0] = Math.Abs(Convert.ToDecimal(reader[0])); // Index 0 corresponds to Total Liabilities
                        }

                        // Move to the second result set - Total Assets
                        if (reader.NextResult() && reader.Read())
                        {
                            total[1] = Convert.ToDecimal(reader[0]); // Index 1 corresponds to Total Assets
                        }
                    }
                }
            }
            return total;
        }

    }
}
