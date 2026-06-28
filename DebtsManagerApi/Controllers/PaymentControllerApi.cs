using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DebtsManagerDataAccessLayer;
using DebtsManagerBusinessLayer;

namespace DebtsManagerApi.Controllers
{
    [ApiController]
    [Route("api/Payments/")]
    public class PaymentsController : ControllerBase
    {
        [HttpGet("All/{userID}", Name = "GetAllPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PaymentDTO>> GetAllPayments(int userID)
        {
            List<PaymentDTO> list = ClsPayment.GetAll(userID);
            if (list.Count == 0)
                return NotFound("No Payments Found!");
            return Ok(list);
        }

        [HttpGet("ByDebt/{debtID}", Name = "GetPaymentsByDebtID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PaymentDTO>> GetPaymentsByDebtID(Guid debtID)
        {
            List<PaymentDTO> list = ClsPayment.GetPaymentsByDebtID(debtID);
            if (list.Count == 0)
                return NotFound("No Payments Found!");
            return Ok(list);
        }

        [HttpPost(Name = "AddPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PaymentDTO> AddPayment(PaymentDTO newPaymentDTO)
        {
            if (newPaymentDTO == null || newPaymentDTO.Amount <= 0)
                return BadRequest("Invalid payment data.");

            if(ClsPayment.IsExist(newPaymentDTO.ID))
                return Ok("Is Exist Already");


            try
            {
                ClsPayment payment = new ClsPayment(newPaymentDTO);
                if(!payment.Save())
                return BadRequest("Save failed");

                return CreatedAtRoute("GetAllPayments", new { userID = newPaymentDTO.UserID }, newPaymentDTO);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error -> {e.Message}");
            }
        }

        [HttpDelete("{id}/{userID}", Name = "DeletePayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePayment(Guid id, int userID)
        {
            if (id == Guid.Empty)
                return BadRequest($"Not accepted ID {id}");

            if (ClsPayment.Delete(id))
                return Ok($"Payment with ID {id} has been deleted.");
            else
                return NotFound($"Payment with ID {id} not found.");
        }
    }
}
