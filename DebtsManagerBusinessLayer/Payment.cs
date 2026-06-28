using DebtsManagerDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerBusinessLayer
{
    public class ClsPayment
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public PaymentDTO DTO => new PaymentDTO(ID, UserID, DebtID, Amount, Date, UpdateAt);

        public Guid ID { get; set; }
        public int UserID { get; set; }
        public Guid DebtID { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }

        public string UpdateAt { get; set; }


        public ClsPayment(PaymentDTO dto, enMode mode = enMode.AddNew)
        {
            ID = dto.ID;
            UserID = dto.UserID;
            DebtID = dto.DebtID;
            Amount = dto.Amount;
            Date = dto.Date;
            UpdateAt = dto.UpdateAt;
            Mode = mode;
        }

        private bool _AddNewPayment()
        {
            Guid? resultID = ClsPaymentData.InsertPayment(DTO);

            if (resultID.HasValue)
            {
                ID = resultID.Value;
                return true;
            }
            return false;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPayment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
            }
            return false;
        }

        public static List<PaymentDTO> GetPaymentsByDebtID(Guid debtID)
        {
            return ClsPaymentData.GetPaymentsByDebtID(debtID);
        }

        public static bool IsExist(Guid id) => ClsPaymentData.IsPaymentExist(id);

        public static List<PaymentDTO> GetAll(int userID) => ClsPaymentData.GetAllPayments(userID);

        public static bool Delete(Guid id) => ClsPaymentData.DeletePayment(id);
    }
}
