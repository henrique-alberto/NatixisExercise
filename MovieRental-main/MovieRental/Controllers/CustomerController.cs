using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRental.Common;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly Customer.ICustomerFeatures _features;

        public CustomerController(Customer.ICustomerFeatures features)
        {
            _features = features;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1)
                    return BadRequest(new ErrorResponse("Page number must be greater than 0"));

                if (pageSize < 1)
                    return BadRequest(new ErrorResponse("Page size must be greater than 0"));

                var result = await _features.GetAllAsync(page, pageSize);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to retrieve customers", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("An unexpected error occurred", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var customer = await _features.GetByIdAsync(id);
                if (customer == null)
                    return NotFound(new ErrorResponse($"Customer with ID {id} not found"));

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("An unexpected error occurred", ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer.Customer customer)
        {
            try
            {
                if (customer == null)
                    return BadRequest(new ErrorResponse("Customer data is required"));

                if (string.IsNullOrWhiteSpace(customer.Name))
                    return BadRequest(new ErrorResponse("Customer name is required"));

                if (string.IsNullOrWhiteSpace(customer.Email))
                    return BadRequest(new ErrorResponse("Customer email is required"));

                var result = await _features.Save(customer);
                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ErrorResponse("Failed to save customer to database", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse("An unexpected error occurred", ex.Message));
            }
        }
    }
}