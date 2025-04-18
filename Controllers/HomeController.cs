using GroupExpenseManagement01.BAL;
using GroupExpenseManagement01.CommonClasses;
using GroupExpenseManagement01.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GroupExpenseManagement01.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private IConfiguration configuration;
        public HomeController(IConfiguration? _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            //throw new Exception("This is a test exception");
            ViewData["GroupCount"] = GetGroupCount();
            decimal[] totals = CommonFuntions.UserWiseBalanceTotal();
            ViewData["ReceivableAmout"] = totals[0];
            ViewData["PayableAmout"] = totals[1];
            ViewData["Balance"] = Math.Abs(totals[0] - totals[1]);
            ViewData["WhichBalance"] = totals[0] >= totals[1] ? true : false;
            //decimal ReceivableAmout = CommonFuntions.GetReceivableAmout(), PayableAmout = CommonFuntions.GetPayableAmout();
            //ViewData["ReceivableAmout"] = ReceivableAmout;
            //ViewData["PayableAmout"] = PayableAmout;
            ViewData["TotalExpense"] = GetTotalExpense();
            //ViewData["Balance"] = Math.Abs(ReceivableAmout - PayableAmout);
            //ViewData["WhichBalance"] = ReceivableAmout >= PayableAmout ? true : false;
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int GetGroupCount()
        {
            int count = 0;
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Group_Count";
            sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();
            count = (int)sqlCommand.ExecuteScalar();
            sqlConnection.Close();
            return count;
        }

        //public decimal GetReceivableAmout()
        //{
        //    decimal count = 0;
        //    string connectionString = this.configuration.GetConnectionString("ConnectionString");

        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        sqlConnection.Open();

        //        using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
        //        {
        //            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //            sqlCommand.CommandText = "PR_Contribution_Receivable";
        //            sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();

        //            object result = sqlCommand.ExecuteScalar();

        //            if (result != null && result != DBNull.Value)
        //            {
        //                count = Convert.ToDecimal(result);
        //            }
        //            else
        //            {
        //                count = 0; // Or whatever default you want when nothing comes back
        //            }
        //        }
        //    }

        //    return count;

        //}
        //public decimal GetPayableAmout()
        //{
        //    decimal count = 0;
        //    string connectionString = this.configuration.GetConnectionString("ConnectionString");

        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        sqlConnection.Open();

        //        using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
        //        {
        //            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //            sqlCommand.CommandText = "PR_Contribution_Payable";
        //            sqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = CV.UserID();

        //            object result = sqlCommand.ExecuteScalar();

        //            if (result != null && result != DBNull.Value)
        //            {
        //                count = Convert.ToDecimal(result);
        //            }
        //            else
        //            {
        //                count = 0; // Handle null scenario by setting count to 0 or some default value
        //            }
        //        }
        //    }
        //    return count;
        //}

        public decimal GetTotalExpense()
        {
            decimal count = 0;
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_Expense_TotalByUser";
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


    }
}
