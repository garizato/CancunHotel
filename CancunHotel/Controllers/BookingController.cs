using CancunHotel.Models;
using CancunHotel.Services;
using Microsoft.AspNetCore.Mvc;

namespace CancunHotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService) => _bookingService = bookingService;
        
        [HttpPost]
        public async Task<IActionResult> Insert(Guest guest, DateTime startDateRequired, DateTime finalDateRequired)
        {
            var result = await _bookingService.insertBooking(guest, startDateRequired, finalDateRequired);
            return Ok(result);
        }
    }
}
