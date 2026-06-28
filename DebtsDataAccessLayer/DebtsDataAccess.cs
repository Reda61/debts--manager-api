
using System.Data;
using Microsoft.Data.SqlClient;



namespace DebtsManagerDataAccessLayer
{
    public class DebtDTO
    {
        public Guid ID { get; set; }
        public int UserID { get; set; }
        public Guid PersonID { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsPaidToMe { get; set; }
        public bool IsSettled { get; set; }
        public string Date { get; set; }
        public string? Note { get; set; }

        public string updateAt { get; set; }

        public DebtDTO(Guid id, int userID, Guid personID, decimal amount, decimal paidAmount,
            bool isPaidToMe, bool isSettled, string date, string? note, string updateAt )
        {
            ID = id;
            UserID = userID;
            PersonID = personID;
            Amount = amount;
            PaidAmount = paidAmount;
            IsPaidToMe = isPaidToMe;
            IsSettled = isSettled;
            Date = date;
            Note = note;
            this.updateAt = updateAt;
        }
    }

    public class ClsDebts_Data
    {

        public static List<DebtDTO> GetAllDebts(int userID)
        {
            var list = new List<DebtDTO>();
            try
            {

            using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
            using (var cmd = new SqlCommand("SP_GetAllDebts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userID);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DebtDTO(
                            reader.GetGuid(reader.GetOrdinal("ID")),
                            reader.GetInt32(reader.GetOrdinal("UserID")),
                            reader.GetGuid(reader.GetOrdinal("PersonID")),
                            reader.GetDecimal(reader.GetOrdinal("Amount")),
                            reader.GetDecimal(reader.GetOrdinal("PaidAmount")),
                            reader.GetBoolean(reader.GetOrdinal("IsPaidToMe")),
                            reader.GetBoolean(reader.GetOrdinal("IsSettled")),
                            reader.GetString(reader.GetOrdinal("Date")),
                            reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                            reader.GetDateTime(reader.GetOrdinal("UpdateAt")).ToString("o")

                        ));
                    }
                }
            }
            return list;
            }
            catch(Exception ex)
            {
                return list;
            }
        }

        public static DebtDTO? GetDebtByID(Guid id)
        {
            try
            {

                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetDebtByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new DebtDTO(
                                reader.GetGuid(reader.GetOrdinal("ID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetGuid(reader.GetOrdinal("PersonID")),
                                reader.GetDecimal(reader.GetOrdinal("Amount")),
                                reader.GetDecimal(reader.GetOrdinal("PaidAmount")),
                                reader.GetBoolean(reader.GetOrdinal("IsPaidToMe")),
                                reader.GetBoolean(reader.GetOrdinal("IsSettled")),
                                reader.GetString(reader.GetOrdinal("Date")),
                                reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note")),
                                reader.GetDateTime(reader.GetOrdinal("UpdateAt")).ToString()

                            );
                        }
                        return null;
                    }
                }
            }

            catch {
              return null;
            }
        }

        public static Guid? InsertDebt(DebtDTO debt)
        {
            try
            {

            using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
            using (var cmd = new SqlCommand("SP_InsertDebt", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", debt.ID);
                cmd.Parameters.AddWithValue("@UserID", debt.UserID);
                cmd.Parameters.AddWithValue("@PersonID", debt.PersonID);
                cmd.Parameters.AddWithValue("@Amount", debt.Amount);
                cmd.Parameters.AddWithValue("@PaidAmount", debt.PaidAmount);
                cmd.Parameters.AddWithValue("@IsPaidToMe", debt.IsPaidToMe);
                cmd.Parameters.AddWithValue("@IsSettled", debt.IsSettled);
                cmd.Parameters.AddWithValue("@Date", debt.Date);
                cmd.Parameters.AddWithValue("@Note", (object?)debt.Note ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdateAt",Convert.ToDateTime(debt.updateAt));


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
            catch
            {
                return null;
            }
        }

        public static bool UpdateDebt(DebtDTO debt)
        {
            try
            {

            using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
            using (var cmd = new SqlCommand("SP_UpdateDebt", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", debt.ID);
                cmd.Parameters.AddWithValue("@Amount", debt.Amount);
                cmd.Parameters.AddWithValue("@PaidAmount", debt.PaidAmount);
                cmd.Parameters.AddWithValue("@IsPaidToMe", debt.IsPaidToMe);
                cmd.Parameters.AddWithValue("@IsSettled", debt.IsSettled);
                cmd.Parameters.AddWithValue("@Date", debt.Date);
                cmd.Parameters.AddWithValue("@Note", (object?)debt.Note ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            }
            catch { 
                return false;
            }
        }


        public static bool IsDebtExist(Guid id)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_IsExistDebt", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", id);

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

        public static bool DeleteDebt(Guid id)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_DeleteDebt", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;

                    conn.Open();

                    int rows = Convert.ToInt32(cmd.ExecuteScalar());

                    return rows > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
