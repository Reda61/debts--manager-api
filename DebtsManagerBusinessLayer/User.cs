using DebtsManagerDataAccessLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerBusinessLayer
{
    public class ClsUser
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public UserDTO DTO => new UserDTO(ID, GoogleID, Email, Name);

        public int ID { get; set; }
        public string GoogleID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public ClsUser(UserDTO dto, enMode mode = enMode.AddNew)
        {
            ID = dto.ID;
            GoogleID = dto.GoogleID;
            Email = dto.Email;
            Name = dto.Name;
            Mode = mode;
        }

        private bool _AddNewUser()
        {
            ID = ClsUserData.InsertUser(DTO);
            return ID > 0;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
            }
            return false;
        }

        public static ClsUser? FindByGoogleID(string googleID)
        {
            UserDTO? dto = ClsUserData.GetUserByGoogleID(googleID);
            if (dto != null)
                return new ClsUser(dto, enMode.Update);
            return null;
        }

        public static bool IsUserExistByGoogleID(string googleID) => ClsUserData.IsUserExistByGoogleID(googleID);

        public static ClsUser? FindByEmail(string email)
        {
            UserDTO? dto = ClsUserData.GetUserByEmail(email);
            if (dto != null)
                return new ClsUser(dto, enMode.Update);
            return null;
        }
    }
}
