using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Search_Filter.Models;

namespace Search_Filter.DataContext
{
    public class DatabaseHelper
    {
        private readonly string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionLoginDB"].ConnectionString;

        public List<Product> SearchedRecord(string filterQuery)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SearchProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parameters for the stored procedure
                        cmd.Parameters.AddWithValue("@filterQuery", filterQuery);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            List<Product> productList = new List<Product>();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                // Map data from DataRow to Product object
                                Product product = new Product
                                {
                                    ProductId = Convert.ToInt32(dr["ProductId"]),
                                    ProductName = dr["ProductName"].ToString(),
                                    Size = dr["Size"].ToString(),
                                    Price = Convert.ToDecimal(dr["Price"]),
                                    MfgDate = Convert.ToDateTime(dr["MfgDate"]),
                                    Category = dr["Category"].ToString()
                                };

                                // Add the Product object to the list
                                productList.Add(product);
                            }

                            return productList;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception here or throw it
                    throw ex;
                }
            }
        }

        public List<Product> GetAllRecords()
        {
            try
            {
                var products = new List<Product>();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAllProducts", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new Product
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    Size = reader["Size"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    MfgDate = Convert.ToDateTime(reader["MfgDate"]),
                                    Category = reader["Category"].ToString()
                                };

                                products.Add(product);
                            }
                        }
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log or throw)
                throw ex;
            }
        }
    }
}
