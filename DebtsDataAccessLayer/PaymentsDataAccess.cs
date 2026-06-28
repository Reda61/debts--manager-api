using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerDataAccessLayer
{
    public class PaymentDTO
    {
        public Guid ID { get; set; }
        public int UserID { get; set; }
        public Guid DebtID { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }

        public string UpdateAt { get; set; }

        public PaymentDTO(Guid id, int userID, Guid debtID, decimal amount, string date, string updateAt)
        {
           this. ID = id;
           this. UserID = userID;
           this. DebtID = debtID;
           this. Amount = amount;
           this. Date = date;
           this. UpdateAt = updateAt;
        }
    }

    public class ClsPaymentData
    {

        public static List<PaymentDTO> GetAllPayments(int userID)
        {
            var list = new List<PaymentDTO>();

            try
            {

                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetAllPayments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PaymentDTO(
                                reader.GetGuid(reader.GetOrdinal("ID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetGuid(reader.GetOrdinal("DebtID")),
                                reader.GetDecimal(reader.GetOrdinal("Amount")),
                                reader.GetString(reader.GetOrdinal("Date")),
                                reader.GetDateTime(reader.GetOrdinal("UpdateAt")).ToString("o")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public static List<PaymentDTO> GetPaymentsByDebtID(Guid debtID)
        {
            List<PaymentDTO> list = new List<PaymentDTO>();
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetPaymentsByDebtID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DebtID", debtID);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PaymentDTO(
                                reader.GetGuid(reader.GetOrdinal("ID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetGuid(reader.GetOrdinal("DebtID")),
                                reader.GetDecimal(reader.GetOrdinal("Amount")),
                                reader.GetString(reader.GetOrdinal("Date")),
                                reader.GetDateTime(reader.GetOrdinal("UpdateAt")).ToString()

                            ));
                        }
                    }
                }
            }
            catch (SqlException)
            {
            }
            return list;
        }

        public static Guid? InsertPayment(PaymentDTO payment)
        {

            try
            {


                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_InsertPayment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", payment.ID);
                    cmd.Parameters.AddWithValue("@UserID", payment.UserID);
                    cmd.Parameters.AddWithValue("@DebtID", payment.DebtID);
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@Date", payment.Date);
                    cmd.Parameters.AddWithValue("@UpdateAt",Convert.ToDateTime(payment.UpdateAt));


                    var outputID = new SqlParameter("@NewID", SqlDbType.UniqueIdentifier)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return (Guid)outputID.Value;
                }
            }
            catch (SqlException)
            {
                return null;
            }
        }
        public static bool IsPaymentExist(Guid id)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_IsExistPayment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;

                    conn.Open();

                    var result = cmd.ExecuteScalar();

                    return Convert.ToInt32(result) == 1;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool DeletePayment(Guid id)
        {

            try
            {


            using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
            using (var cmd = new SqlCommand("SP_DeletePayment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;

                conn.Open();
                int rows = Convert.ToInt32( cmd.ExecuteScalar());
                return rows > 0;
            }
            }
            catch (Exception e)
            {
                return false;

            }
        }
    }
}
