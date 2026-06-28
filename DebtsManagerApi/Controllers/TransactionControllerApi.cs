using DebtsManagerBusinessLayer;
using DebtsManagerDataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using System.Transactions;

namespace DebtsManagerApi.Controllers
{
    [ApiController]
    [Route("api/Transactions/")]
    public class TransactionsController : ControllerBase
    {
        [HttpGet("All/{userID}", Name = "GetAllTransactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TransactionDTO>> GetAllTransactions(int userID)
        {
            if (userID < 0)
                return BadRequest("Invalid ID");
            List<TransactionDTO> list = ClsTransaction.GetAll(userID);
            if (list.Count == 0)
                return NotFound("No Transactions Found!");
            return Ok(list);
        }

        [HttpPost(Name = "AddTransaction")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TransactionDTO> AddTransaction(TransactionDTO newTransactionDTO)
        {
            if (newTransactionDTO == null || newTransactionDTO.Amount <= 0)
                return BadRequest("Invalid transaction data.");

            if (ClsTransaction.IsExist(newTransactionDTO.ID))
                return Ok("Is Exist Already");

            try
            {
                ClsTransaction transaction = new ClsTransaction(newTransactionDTO);
                if(!transaction.Save())
                    return BadRequest("Invalid Saved data data.");

                newTransactionDTO.ID = transaction.ID;
                return CreatedAtRoute("GetAllTransactions", new { userID = newTransactionDTO.DebtID }, newTransactionDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}/{userID}", Name = "DeleteTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteTransaction(Guid id, int userID)
        {
            if (id == Guid.Empty)
                return BadRequest(new { result = -1, message = $"Not accepted ID {id}" });

            if (ClsTransaction.Delete(id))
                return Ok(new { result = 1, message = $"Transaction with ID {id} has been deleted."});
            else
                return NotFound(new { result = 0, message = $"Transaction with ID {id} not found." });
        }
    }
}
