using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerDataAccessLayer
{
    public class TransactionDTO
    {
        public Guid ID { get; set; }
        public Guid DebtID { get; set; }
        public Guid PaymentID { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }

        public string UpdateAt { get; set; }


        public TransactionDTO(Guid id, Guid debtID, Guid paymentID, decimal amount, string date, string updateAt)
        {
            ID = id;
            DebtID = debtID;
            PaymentID = paymentID;
            Amount = amount;
            Date = date;
            UpdateAt = updateAt;
        }
    }

    public class ClsTransactionData
    {

        public static List<TransactionDTO> GetAllTransactions(int userID)
        {
            var list = new List<TransactionDTO>();

            try
            {

                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetAllTransactions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TransactionDTO(
                                reader.GetGuid(reader.GetOrdinal("ID")),
                                reader.GetGuid(reader.GetOrdinal("DebtID")),
                                reader.GetGuid(reader.GetOrdinal("PaymentID")),
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

        public static Guid? InsertTransaction(TransactionDTO transaction)
        {
            try
            {


                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_InsertTransaction", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", transaction.ID);
                    cmd.Parameters.AddWithValue("@DebtID", transaction.DebtID);
                    cmd.Parameters.AddWithValue("@PaymentID", transaction.PaymentID);
                    cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
                    cmd.Parameters.AddWithValue("@Date", transaction.Date);
                    cmd.Parameters.AddWithValue("@UpdateAt",Convert.ToDateTime( transaction.UpdateAt));


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
            catch (Exception ex)
            {
                return null;
            }
        }
        public static bool IsTransactionExist(Guid id)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_IsExistTransaction", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;

                    conn.Open();

                    var result = cmd.ExecuteScalar();

                    return Convert.ToInt32(result) > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool DeleteTransaction(Guid id)
        {

            try
            {

                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_DeleteTransaction", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                    conn.Open();
                    int rows = Convert.ToInt32(cmd.ExecuteScalar());
                    return rows > 0;
                }

            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
