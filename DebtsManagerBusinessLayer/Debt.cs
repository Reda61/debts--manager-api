
using DebtsManagerDataAccessLayer;

namespace DebtsManagerBusinessLayer
{

    public class ClsDebt
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public DebtDTO DTO => new DebtDTO(ID, UserID, PersonID, Amount, PaidAmount,
            IsPaidToMe, IsSettled, Date, Note, UpdateAt);

        public Guid ID { get; set; }
        public int UserID { get; set; }
        public Guid PersonID { get; set; }
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsPaidToMe { get; set; }
        public bool IsSettled { get; set; }
        public string Date { get; set; }
        public string? Note { get; set; }

        public string UpdateAt { get; set; }


        public ClsDebt(DebtDTO dto, enMode mode = enMode.AddNew)
        {
            ID = dto.ID;
            UserID = dto.UserID;
            PersonID = dto.PersonID;
            Amount = dto.Amount;
            PaidAmount = dto.PaidAmount;
            IsPaidToMe = dto.IsPaidToMe;
            IsSettled = dto.IsSettled;
            Date = dto.Date;
            Note = dto.Note;
            UpdateAt = dto.updateAt;
            Mode = mode;
        }

        private bool _AddNewDebt()
        {

            Guid? resultID = ClsDebts_Data.InsertDebt(DTO);

            if (resultID.HasValue)
            {
                ID = resultID.Value;
                return true;
            }
            return false;
        }

        private bool _UpdateDebt() => ClsDebts_Data.UpdateDebt(DTO);

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDebt())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateDebt();
            }
            return false;
        }

        public static List<DebtDTO> GetAll(int userID) => ClsDebts_Data.GetAllDebts(userID);


        public static bool IsExist(Guid id) => ClsDebts_Data.IsDebtExist(id);

        public static ClsDebt? Find(Guid id)
        {
            
            DebtDTO? dto = ClsDebts_Data.GetDebtByID(id);
            return dto != null ? new ClsDebt(dto, enMode.Update) : null;
        }

        public static bool Delete(Guid id) => ClsDebts_Data.DeleteDebt(id);
    }
}

