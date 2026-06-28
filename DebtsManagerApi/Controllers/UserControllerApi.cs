using DebtsManagerDataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using DebtsManagerBusinessLayer;

namespace DebtsManagerApi.Controllers
{
    [ApiController]
    [Route("api/Users/")]
    public class UserControllerApi : ControllerBase
    {
        [HttpGet("FindByGoogleID/{googleID}", Name = "GetUserByGoogleID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserByGoogleID(string googleID)
        {
            if (string.IsNullOrEmpty(googleID))
                return BadRequest("Invalid Google ID.");

            ClsUser? user = ClsUser.FindByGoogleID(googleID);
            if (user == null)
                return NotFound($"User with Google ID {googleID} not found.");

            return Ok(user.DTO);
        }



        [HttpGet("FindByEmail/{email}", Name = "GetUserByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Invalid Email.");

            ClsUser? user = ClsUser.FindByEmail(email);
            if (user == null)
                return NotFound($"User with Email {email} not found.");

            return Ok(user.DTO);
        }






        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> AddUser(UserDTO newUserDTO)
        {
            if (newUserDTO == null || string.IsNullOrEmpty(newUserDTO.Email))
                return BadRequest("Invalid user data.");

            try
            {

                bool isUserExist  = ClsUser.IsUserExistByGoogleID(newUserDTO.GoogleID);
                ClsUser? user;

                if (isUserExist)
                {
                    user = ClsUser.FindByGoogleID(newUserDTO.GoogleID);
                    if (user != null)
                    return Ok(user.DTO);

                    return StatusCode(500, "Internal Server Error");
                }


                user = new ClsUser(newUserDTO);
              
                if (user.Save())
                {
                    newUserDTO.ID = user.ID;
                    return CreatedAtRoute("GetUserByGoogleID", new { googleID = newUserDTO.GoogleID }, newUserDTO);
                }
                return StatusCode(400, "Internal Server Error333");

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
