using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace ASP.NET_FinalTermExam.Models
{
    public class FinalCustomers
    {
        private static string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBconnect"].ConnectionString.ToString();
        }

        public static List<Cust> GetContactTitle()
        {
            List<Cust> result = new List<Cust>();
            DataTable dt = new DataTable();
            string sql = @" SELECT CodeId + '-' + CodeVal AS ContactTitle
                            FROM dbo.CodeTable
                            WHERE CodeType LIKE 'TITLE'";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new Cust()
                {
                    ContactTitle = row["ContactTitle"].ToString()
                });
            }
            return result;
        }

        public List<Cust> SearchCustomers(Cust cust)
        {
            List<Cust> result = new List<Cust>();
            DataTable dt = new DataTable();
            string sql = @" SELECT CustomerID, CompanyName, ContactName, ContactTitle + '-' + CodeVal AS ContactTitle
                            FROM dbo.CodeTable AS code JOIN Sales.Customers AS cust ON code.CodeId = cust.ContactTitle
                            WHERE code.CodeType LIKE 'TITLE' AND ";
            if (cust.CustomerID != 0)
            {
                sql += "CustomerID = " + cust.CustomerID + " AND ";
            }
            if (cust.CompanyName != null)
            {
                sql += "CompanyName LIKE '%" + cust.CompanyName + "%' AND ";
            }
            if (cust.ContactName != null)
            {
                sql += "ContactName LIKE '%" + cust.ContactName + "%' AND ";
            }
            if (cust.ContactTitle != null)
            {
                sql += "ContactTitle + '-' + CodeVal LIKE '%" + cust.ContactTitle + "%' AND ";
            }
            sql += "1=1";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new Cust()
                {
                    CustomerID = (int)row["CustomerID"],
                    CompanyName = row["CompanyName"].ToString(),
                    ContactName = row["ContactName"].ToString(),
                    ContactTitle = row["ContactTitle"].ToString()
                });
            }
            return result;

        }

        public static void NewCustomers(Cust cust)
        {
            string sql = @" INSERT INTO Sales.Customers(CustomerID, CompanyName, ContactName, ContactTitle, CreationDate, Country, City, Region, PostalCode, Address, Phone, Fax)
                            VALUES(NULL, @CompanyName, @ContactName, @ContactTitle, @CreationDate, @Country, @City, @Region, @PostalCode, @Address, @Phone, @Fax)";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(new SqlParameter("@CompanyName", cust.CompanyName == null ? string.Empty : cust.CompanyName));
                cmd.Parameters.Add(new SqlParameter("@ContactName", cust.ContactName == null ? string.Empty : cust.ContactName));
                cmd.Parameters.Add(new SqlParameter("@ContactTitle", cust.ContactTitle.Substring(0, 4)));
                cmd.Parameters.Add(new SqlParameter("@CreationDate", cust.CreationDate));
                cmd.Parameters.Add(new SqlParameter("@Country", cust.Country == null ? string.Empty : cust.Country));
                cmd.Parameters.Add(new SqlParameter("@City", cust.City == null ? string.Empty : cust.City));
                cmd.Parameters.Add(new SqlParameter("@Region", cust.Region == null ? string.Empty : cust.Region));
                cmd.Parameters.Add(new SqlParameter("@PostalCode", cust.PostalCode == null ? string.Empty : cust.PostalCode));
                cmd.Parameters.Add(new SqlParameter("@Address", cust.Address == null ? string.Empty : cust.Address));
                cmd.Parameters.Add(new SqlParameter("@Phone", cust.Phone == null ? string.Empty : cust.Phone));
                cmd.Parameters.Add(new SqlParameter("@Fax", cust.Fax == null ? string.Empty : cust.Fax));

                cmd.ExecuteScalar();
                conn.Close();
            }
        }
    }
}