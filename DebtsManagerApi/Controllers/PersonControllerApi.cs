using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using DebtsManagerBusinessLayer;
using DebtsManagerDataAccessLayer;


namespace DebtsManagerApi.Controllers
{
    [ApiController]
    [Route("api/People/")]
    public class PeopleController : ControllerBase
    {
        [HttpGet("All/{userID}", Name = "GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PersonDTO>> GetAllPeople(int userID)
        {
            List<PersonDTO> list = ClsPerson.GetAll(userID);
            if (list.Count == 0)
                return NotFound("No People Found!");
            return Ok(list);
        }

        [HttpGet("{id}", Name = "GetPersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonDTO> GetPersonByID(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest($"Not accepted ID {id}");

            ClsPerson? clsperson = ClsPerson.Find(id);
            if (clsperson == null)
                return NotFound($"ClsPerson with ID {id} not found.");

            return Ok(clsperson.DTO);
        }


        //add or update if person exist
        [HttpPost(Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> AddPerson(PersonDTO newPersonDTO)
        {
            if (newPersonDTO == null || string.IsNullOrEmpty(newPersonDTO.Fullname))
                return BadRequest("Invalid Person data.");

            try
            {


                ClsPerson? person;

                if (ClsPerson.IsExist(newPersonDTO.ID))
                {
                 

                    person = new ClsPerson(newPersonDTO,ClsPerson.enMode.Update);


                    if (!person.Save())
                    {
                        return BadRequest("Invalid Save Person Info.");

                    }

                    return Ok(person.DTO);

                }
                else
                {

                    person = new ClsPerson(newPersonDTO);

                    if (!person.Save())
                    {
                        return BadRequest("Invalid Save Person Info.");

                    }
                 


                    return CreatedAtRoute("GetPersonByID", new { id = newPersonDTO.ID }, newPersonDTO);

                }






            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> UpdatePerson(Guid id, PersonDTO updatedPerson)
        {
            if (id == Guid.Empty || updatedPerson == null || string.IsNullOrEmpty(updatedPerson.Fullname))
                return BadRequest("Invalid clsperson data.");

            ClsPerson? clsperson = ClsPerson.Find(id);
            if (clsperson == null)
                return NotFound($"ClsPerson with ID {id} not found.");

            try
            {
                clsperson.Fullname = updatedPerson.Fullname;
                clsperson.Phone = updatedPerson.Phone;
                clsperson.Imagepath = updatedPerson.Imagepath;
                clsperson.Save();
                return Ok(clsperson.DTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}/{userID}", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePerson(Guid id,int userID)
        {
            if (id == Guid.Empty)
                return BadRequest($"Not accepted ID {id}");

            if (ClsPerson.Delete(id))
                return Ok($"ClsPerson with ID {id} has been deleted.");
            else
                return NotFound($"ClsPerson with ID {id} not found.");
        }
    }
}
