using CancunHotel.Services;
using Microsoft.AspNetCore.Mvc;

namespace CancunHotel.Controllers
{
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService) 
        {
            _roomService = roomService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> checkAvailableRoom(int roomNumber)
        {
            var result = await _roomService.checkAvailableRoom(roomNumber);
            return Ok(result);
        }
    }
}
