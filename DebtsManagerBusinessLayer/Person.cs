using DebtsManagerDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerBusinessLayer
{
    public class ClsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;


        public Guid ID { get; set; }
        public int UserID { get; set; }
        public string Fullname { get; set; }
        public string? Phone { get; set; }
        public string? Imagepath { get; set; }

        public string UpdateAt { get; set; }

        public PersonDTO DTO => new PersonDTO(ID, UserID, Fullname, Phone, Imagepath, UpdateAt);



        public ClsPerson(PersonDTO dto, enMode mode = enMode.AddNew)
        {
            ID = dto.ID;
            UserID = dto.UserID;
            Fullname = dto.Fullname;
            Phone = dto.Phone;
            Imagepath = dto.Imagepath;
            UpdateAt = dto.UpdateAt;
           this. Mode = mode;
        }

        private bool _AddNewPerson()
        {

            Guid? resultID = ClsPeople_Data.InsertPerson(DTO);

            if (resultID.HasValue)
            {
             ID = resultID.Value;
                return true;    
            }
            return false;
        }

        private bool _UpdatePerson() => ClsPeople_Data.UpdatePerson(DTO);

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;


                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }

        public static List<PersonDTO> GetAll(int userID) => ClsPeople_Data.GetAllPeople(userID);
        public static bool IsExist(Guid id) => ClsPeople_Data.IsPersonExist(id);

        public static ClsPerson? Find(Guid id)
        {
            PersonDTO? dto = ClsPeople_Data.GetPersonByID(id);
            if (dto != null)
                return new ClsPerson(dto, enMode.Update);
            else
                return null;
        }

        public static bool Delete(Guid id) => ClsPeople_Data.DeletePerson(id);
    }
}
