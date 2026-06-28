using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerDataAccessLayer
{
    public class PersonDTO
    {

        //public  enum enMode { AddNew,Updated}
        //enMode mode = enMode.AddNew;
        public Guid ID { get; set; }
        public int    UserID { get; set; }
        public string Fullname { get; set; }
        public string? Phone { get; set; }
        public string? Imagepath { get; set; }
        public string UpdateAt { get; set; }

        public PersonDTO(Guid id, int userID, string fullname, string? phone, string? imagepath, string UpdateAt)
        {
            ID = id;
            UserID = userID;
            Fullname = fullname;
            Phone = phone;
            Imagepath = imagepath;
            this.UpdateAt = UpdateAt;
        }
    }


    public class ClsPeople_Data
    {

        public static List<PersonDTO> GetAllPeople(int userID)
        {
            var list = new List<PersonDTO>();

            try
            {

                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetAllPeople", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PersonDTO(
                                reader.GetGuid(reader.GetOrdinal("ID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Fullname")),
                                reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                reader.IsDBNull(reader.GetOrdinal("Imagepath")) ? null : reader.GetString(reader.GetOrdinal("Imagepath")),
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

        public static PersonDTO? GetPersonByID(Guid id)
        {
            try
            {


                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_GetPersonByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PersonDTO(
                                reader.GetGuid(reader.GetOrdinal("ID")),
                                reader.GetInt32(reader.GetOrdinal("UserID")),
                                reader.GetString(reader.GetOrdinal("Fullname")),
                                reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                reader.IsDBNull(reader.GetOrdinal("Imagepath")) ? null : reader.GetString(reader.GetOrdinal("Imagepath")),
                                (reader.GetDateTime(reader.GetOrdinal("UpdateAt"))).ToString()

                            );
                        }
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public static Guid? InsertPerson(PersonDTO person)
        {

            try
            {


                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_InsertPerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", person.ID);
                    cmd.Parameters.AddWithValue("@UserID", person.UserID);
                    cmd.Parameters.AddWithValue("@Fullname", person.Fullname);
                    cmd.Parameters.AddWithValue("@Phone", (object?)person.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Imagepath", (object?)person.Imagepath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdateAt", (object?) Convert.ToDateTime(person.UpdateAt));


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

        public static bool UpdatePerson(PersonDTO person)
        {
            try
            {


                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_UpdatePerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", person.ID);
                    cmd.Parameters.AddWithValue("@Fullname", person.Fullname);
                    cmd.Parameters.AddWithValue("@Phone", (object?)person.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Imagepath", (object?)person.Imagepath ?? DBNull.Value);

                    conn.Open();
                   int updatedResult = cmd.ExecuteNonQuery();
                    
                    return updatedResult > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool IsPersonExist(Guid id)
        {
            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_IsExistPerson", conn))
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

        public static bool DeletePerson(Guid id)
        {

            try
            {
                using (var conn = new SqlConnection(ClsConnectionSettings.ConnectionString))
                using (var cmd = new SqlCommand("SP_DeletePerson", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;

                    conn.Open();
                    int rows = Convert.ToInt32( cmd.ExecuteScalar());
                    return rows > 0;
                }
            }


            catch (Exception ex) {

                return false;
            }
        }

    }
}
