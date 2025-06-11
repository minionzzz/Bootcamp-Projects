using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HotelBookingAPI.Data;
using HotelBookingAPI.Models;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HotelBookingAPIControllers : ControllerBase
    {
        private readonly ApiContext _context;
        
        // Constructor to inject the DbContext
        public HotelBookingAPIControllers(ApiContext context)
        {
            _context = context;
        }

        // Define your API endpoints here

        //Create/Edit
        [HttpPost]
        public JsonResult CreateEdit(HotelBooking booking)
        {
            if (booking.Id == 0)
            {
                _context.Bookings.Add(booking);
            }
            else
            {
                var bookingInDb = _context.Bookings.Find(booking.Id);

                if (bookingInDb == null)
                {
                    return new JsonResult(NotFound());
                }
                bookingInDb = booking;
            }

            _context.SaveChanges();
            return new JsonResult(Ok(booking));
        }

        //Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.Bookings.Find(id);
            if (result == null)
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(result));
        }

        //Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Bookings.Find(id);
            if (result == null)
            {
                return new JsonResult(NotFound());
            }
            _context.Bookings.Remove(result);
            _context.SaveChanges();
            return new JsonResult(NoContent());
        }

        //Get All
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.Bookings.ToList();

            return new JsonResult(Ok(result));
            
        }

    }
}