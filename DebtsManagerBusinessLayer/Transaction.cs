using DebtsManagerDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtsManagerBusinessLayer
{
    public class ClsTransaction
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;


        public TransactionDTO DTO => new TransactionDTO(ID, DebtID, PaymentID, Amount, Date, UpdateAt);

        public Guid ID { get; set; }
        public Guid DebtID { get; set; }
        public Guid PaymentID { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }

        public string UpdateAt { get; set; }


        public ClsTransaction(TransactionDTO dto, enMode mode = enMode.AddNew)
        {
            ID = dto.ID;
            DebtID = dto.DebtID;
            PaymentID = dto.PaymentID;
            Amount = dto.Amount;
            Date = dto.Date;
            UpdateAt = dto.UpdateAt;
            Mode = mode;
        }

        private bool _AddNewTransaction()
        {

            Guid? resultID = ClsTransactionData.InsertTransaction(DTO);

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
                    if (_AddNewTransaction())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
            }
            return false;
        }

        public static List<TransactionDTO> GetAll(int userID) => ClsTransactionData.GetAllTransactions(userID);

        public static bool IsExist(Guid id) => ClsTransactionData.IsTransactionExist(id);

        public static bool Delete(Guid id) => ClsTransactionData.DeleteTransaction(id);
    }
}
