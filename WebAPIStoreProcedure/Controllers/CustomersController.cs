using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebAPIStoreProcedure.Models;

namespace WebAPIStoreProcedure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public CustomersController(IConfiguration configuration)
        {
            this.configuration = configuration;

            connectionString = configuration["ConnectionStrings"];
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_GetAllCustomer", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Customer customer = new Customer();
                        customer.ID = Convert.ToInt32(sqlDataReader["CustomerID"]);
                        customer.Name = sqlDataReader["Name"].ToString();
                        customer.Address = sqlDataReader["Address"].ToString();
                        customer.Gender = sqlDataReader["Gender"].ToString();
                        customer.Country = sqlDataReader["Country"].ToString();
                        customer.City = sqlDataReader["City"].ToString();
                        customer.Mobile = sqlDataReader["MobileNo"].ToString();
                        customer.Email = sqlDataReader["MailId"].ToString();
                        customers.Add(customer);
                    }
                }
                catch (Exception exception)
                {

                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return Ok(customers);
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            Customer customer = new Customer();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_GetCustomerById", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CustomerId", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        customer.ID = Convert.ToInt32(sqlDataReader["CustomerID"]);
                        customer.Name = sqlDataReader["Name"].ToString();
                        customer.Address = sqlDataReader["Address"].ToString();
                        customer.Gender = sqlDataReader["Gender"].ToString();
                        customer.Country = sqlDataReader["Country"].ToString();
                        customer.City = sqlDataReader["City"].ToString();
                        customer.Mobile = sqlDataReader["MobileNo"].ToString();
                        customer.Email = sqlDataReader["MailId"].ToString();
                    }
                }
                catch (Exception exception)
                {

                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return Ok(customer);
        }

        // POST api/<CustomersController>
        [HttpPost]
        public ActionResult Post([FromBody] Customer customer)
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
                    sqlCommand.Parameters.AddWithValue("@Name", customer.Name);
                    sqlCommand.Parameters.AddWithValue("@Address", customer.Address);
                    sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                    sqlCommand.Parameters.AddWithValue("@Country", customer.Country);
                    sqlCommand.Parameters.AddWithValue("@City", customer.City);
                    sqlCommand.Parameters.AddWithValue("@MobileNo", customer.Mobile);
                    sqlCommand.Parameters.AddWithValue("@MailId", customer.Email);

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

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Customer customer)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_UpdateCustomer", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlConnection.Open();

                SqlTransaction transaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = transaction;

                try
                {
                    sqlCommand.Parameters.AddWithValue("@CustomerId", customer.ID);
                    sqlCommand.Parameters.AddWithValue("@Name", customer.Name);
                    sqlCommand.Parameters.AddWithValue("@Address", customer.Address);
                    sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                    sqlCommand.Parameters.AddWithValue("@Country", customer.Country);
                    sqlCommand.Parameters.AddWithValue("@City", customer.City);
                    sqlCommand.Parameters.AddWithValue("@MobileNo", customer.Mobile);
                    sqlCommand.Parameters.AddWithValue("@MailId", customer.Email);

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

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_DeleteCustomer", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlConnection.Open();

                SqlTransaction transaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = transaction;

                try
                {
                    sqlCommand.Parameters.AddWithValue("@CustomerId", id);

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
