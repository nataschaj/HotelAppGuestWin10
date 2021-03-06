﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HotelWebservice2;
using HotelWebservice2.DTO;

namespace HotelWebservice2.Controllers
{
    public class RoomsController : ApiController
    {
        private HotelContext db = new HotelContext();


        /// <summary>
        /// Giver alle single rooms fra det ønskede område
        /// </summary>
        /// <param name="area">område fx. roskilde</param>
        /// <returns>IEnumerable<HotelRoomTypeDTO> </returns>
        [Route("api/Rooms/{area}/{roomtype}/Area")]
        [HttpGet]
        public IEnumerable<HotelRoomTypeDTO> GetSingleRoomsByArea(string area, string roomtype)
        {
            IEnumerable<HotelRoomTypeDTO> singleRoomList = 
                               from r in db.Room
                               join h in db.Hotel on r.Hotel_No equals h.Hotel_No
                               where r.Types == roomtype
                               && h.Address.Contains(area)
                               select new HotelRoomTypeDTO()
                               {
                                   Name = h.Name,
                                   Price = r.Price,
                                   Room_No = r.Room_No,
                                   Types = r.Types
                               };
            return singleRoomList;

            //return db.Booking.Where(x => x.Guest_No == GuestNo);
        }

        // GET: api/Rooms
        public IQueryable<Room> GetRoom()
        {
            return db.Room;
        }

        // GET: api/Rooms/5
        [ResponseType(typeof(Room))]
        public IHttpActionResult GetRoom(int id)
        {
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // PUT: api/Rooms/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoom(int id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.Room_No)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Rooms
        [ResponseType(typeof(Room))]
        public IHttpActionResult PostRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Room.Add(room);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RoomExists(room.Room_No))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = room.Room_No }, room);
        }

        // DELETE: api/Rooms/5
        [ResponseType(typeof(Room))]
        public IHttpActionResult DeleteRoom(int id)
        {
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            db.Room.Remove(room);
            db.SaveChanges();

            return Ok(room);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Room.Count(e => e.Room_No == id) > 0;
        }
    }
}