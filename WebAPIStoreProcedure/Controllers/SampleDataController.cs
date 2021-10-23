using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebAPIStoreProcedure.Controllers
{
    [Route("api/samples")]
    [ApiController]
    public class SampleDataController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public SampleDataController(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings"];
        }

        [HttpGet]
        public ActionResult GenerateCustomers()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_AddCustomer", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlConnection.Open();

                SqlTransaction transaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = transaction;

                try
                {

                    for (int i = 0; i < 2; i++)
                    {
                        var name = sqlCommand.Parameters.AddWithValue("@Name", "Name" + i);
                        
                        var address = sqlCommand.Parameters.AddWithValue("@Address", "Address" + i);
                      
                        //sqlCommand.Parameters.AddWithValue("@Gender", "Gender" + i);
                        //sqlCommand.Parameters.AddWithValue("@Country", "Country" + i);
                        //sqlCommand.Parameters.AddWithValue("@City", "City" + i);
                        //sqlCommand.Parameters.AddWithValue("@MobileNo", "MobileNo" + i);
                        //sqlCommand.Parameters.AddWithValue("@MailId", "MailId" + i);
                    }

                    sqlCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return Ok(true);
        }

    }
}
