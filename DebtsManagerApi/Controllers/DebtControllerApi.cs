using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DebtsManagerBusinessLayer;
using DebtsManagerDataAccessLayer;

namespace DebtsManagerApi.Controllers
{
    [Route("api/Debts/")]
    [ApiController]
    public class DebtsControllerApi : ControllerBase
    {
       
            [HttpGet("All/{userID}", Name = "GetAllDebts")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public ActionResult<IEnumerable<DebtDTO>> GetAllDebts(int userID)
            {
                List<DebtDTO> list = ClsDebt.GetAll(userID);
                if (list.Count == 0)
                    return NotFound("No Debts Found!");
                return Ok(list);
            }

            [HttpGet("{id}", Name = "GetDebtByID")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public ActionResult<DebtDTO> GetDebtByID(Guid id)
            {
                if (id == Guid.Empty)
                    return BadRequest($"Not accepted ID {id}");

                ClsDebt? clsdebt = ClsDebt.Find(id);
                if (clsdebt == null)
                    return NotFound($"Debt with ID {id} not found.");

                return Ok(clsdebt.DTO);
            }



        //add or update if debt exist
        [HttpPost(Name = "AddDebt")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DebtDTO> AddDebt(DebtDTO newDebtDTO)
        {
            if (newDebtDTO == null || newDebtDTO.Amount <= 0)
                return BadRequest("Invalid debt data.");


            ClsDebt? debt;

            if (ClsDebt.IsExist(newDebtDTO.ID))
            {
                debt = new ClsDebt(newDebtDTO,ClsDebt.enMode.Update);

                if (!debt.Save())
                    return BadRequest("Invalid Not save");


                return Ok(newDebtDTO);
            }
            else
            {
                debt = new ClsDebt(newDebtDTO);

                if (!debt.Save())
                    return BadRequest("Invalid Not save");


                return CreatedAtRoute("GetDebtByID", new { id = newDebtDTO.ID }, newDebtDTO);

            }


           
        }

        [HttpPut("{id}", Name = "UpdateDebt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DebtDTO> UpdateDebt(Guid id, DebtDTO updatedDebt)
            {
                if (id == Guid.Empty || updatedDebt == null || updatedDebt.Amount <= 0)
                    return BadRequest("Invalid debt data.");

                ClsDebt? clsdebt = ClsDebt.Find(id);
                if (clsdebt == null)
                    return NotFound($"Debt with ID {id} not found.");

                clsdebt.PersonID = updatedDebt.PersonID;
                clsdebt.Amount = updatedDebt.Amount;
                clsdebt.PaidAmount = updatedDebt.PaidAmount;
                clsdebt.IsPaidToMe = updatedDebt.IsPaidToMe;
                clsdebt.IsSettled = updatedDebt.IsSettled;
                clsdebt.Date = updatedDebt.Date;
                clsdebt.Note = updatedDebt.Note;
                clsdebt.Save();

                return Ok(clsdebt.DTO);
            }

        [HttpDelete("{id}/{userID}", Name = "DeleteDebt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteDebt(Guid id,int userID)
        {
            if (id == Guid.Empty)
                return BadRequest($"Not accepted ID {id}");

            if (ClsDebt.Delete(id))
                return Ok($"Debt with ID {id} has been deleted.");
            else
                return NotFound($"Debt with ID {id} not found.");
        }
        

    }
}
