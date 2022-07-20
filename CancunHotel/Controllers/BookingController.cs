using CancunHotel.Models;
using CancunHotel.Services;
using Microsoft.AspNetCore.Mvc;

namespace CancunHotel.Controllers
{

    /// <summary>
    /// Booking Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService) => _bookingService = bookingService;

        /// <summary>
        /// Insert a Booking
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="startDateRequired"></param>
        /// <param name="finalDateRequired"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert(Guest guest, DateTime startDateRequired, DateTime finalDateRequired)
        {
            var result = await _bookingService.insertBooking(guest, startDateRequired, finalDateRequired);
            return Ok(result);
        }
        /// <summary>
        /// Cancel a Booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Cancel(int bookingId)
        {
            var result = await _bookingService.cancelBooking(bookingId);
            return Ok(result);
        }

        /// <summary>
        /// Update a Booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="startDateRequired"></param>
        /// <param name="finalDateRequired"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Update(int bookingId, DateTime startDateRequired, DateTime finalDateRequired)
        {
            var result = await _bookingService.updateBooking(bookingId, startDateRequired, finalDateRequired); 
            return Ok(result);
        }

    }
}
