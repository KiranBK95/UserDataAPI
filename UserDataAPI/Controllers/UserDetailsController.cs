using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDataAPI.Model;

namespace UserDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserDetailsController(AppDbContext   context) 
        {
            _context = context;
        }


        [HttpPost]
        // this is because i've added JWT authentication, users can submit form as Guest
        [Authorize(Roles = "Guest")]
        public async Task<ActionResult> PostForm(Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest(new { message = "Invalid form data." });
                }

                // This section is toadd the student record to the database-- students table
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // Once the form data is submitted here, we return success response
                return Ok(new { message = "Your form is submitted and saved successfully." });
            }
            catch (DbUpdateException ex)
            {
                // This could be something related to a database-specific exception if occured
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { message = "An error occurred while saving the data to the database.", error = ex.Message });
            }
            catch (Exception ex)
            {
                // for any other General error handling
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { message = "An unexpected error occurred. Please try again later.", error = ex.Message });
            }
        }


    }
}
