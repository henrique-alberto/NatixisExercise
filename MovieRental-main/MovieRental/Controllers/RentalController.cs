using Microsoft.AspNetCore.Mvc;
using MovieRental.Rental;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rental.Rental rental)
        {
	        return Ok(await _features.Save(rental));
        }

        [HttpGet("by-customer/{customerName}")]
        public async Task<IActionResult> GetByCustomerId([FromRoute] string customerName)
        {
            var rentals = await _features.GetRentalsByCustomerId(customerName);
            return Ok(rentals);
        }
	}
}