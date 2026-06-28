using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerDataAccessLayer
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string GoogleID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public UserDTO(int id, string googleID, string email, string name)
        {
            ID = id;
            GoogleID = googleID;
            Email = email;
            Name = name;
        }
    }

    public class ClsUserData
    {

        //public static UserDTO? GetUserByGoogleID(string googleID)
        //{
        //    try
        //    {
        //        using (var conn = new SqlConnection(_connectionString))
        //        using (var cmd = new SqlCommand("SP_GetUserByGoogleID", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@GoogleID", googleID);
        //            conn.Open();
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                    return new UserDTO(
        //                        reader.GetInt32(reader.GetOrdinal("ID")),
        //                        reader.GetString(reader.GetOrdinal("GoogleID")),
        //                        reader.GetString(reader.GetOrdinal("Email")),
        //                        reader.GetString(reader.GetOrdinal("Name"))
        //                    );
        //                return null;
        //            }
        //        }
        //    }
        //    catch (SqlException) { return null; }
        //}


        public static UserDTO? GetUserByEmail(string email)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetUserByEmail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return new UserDTO(
                                reader.GetInt32(reader.GetOrdinal("ID")),
                                reader.GetString(reader.GetOrdinal("GoogleID")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Name"))
                            );
                        return null;
                    }
                }
            }
            catch (SqlException) { return null; }
        }


        public static UserDTO? GetUserByGoogleID(string googleID)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetUserByGoogleID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GoogleID", googleID);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return new UserDTO(
                                reader.GetInt32(reader.GetOrdinal("ID")),
                                reader.GetString(reader.GetOrdinal("GoogleID")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("Name"))
                            );
                        return null;
                    }
                }
            }
            catch (SqlException) { return null; }
        }






        public static bool IsUserExist(Guid id)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_IsExistUser", conn))
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

        public static bool IsUserExistByGoogleID(string googleID)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_IsExistUserByGoogleID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@GoogleID", googleID);

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


        public static int InsertUser(UserDTO user)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_InsertUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GoogleID", user.GoogleID);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Name", user.Name);

                    var outputID = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return (int)outputID.Value;
                }
            }
            catch (SqlException) {
                throw;
                }


        }
    }
}
