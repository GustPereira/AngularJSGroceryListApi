using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ApiGroceries.Models;
using GroceriesList.Models;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;

namespace ApiGroceries.Controllers
{
    [Authorize]
    public class GroceriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Groceries
        public IHttpActionResult GetGroceries()
        {
            var user = User.Identity.GetUserId();
            var groceries = db.Groceries.Where(t => t.UserId.ToString() == user).OrderBy(o => o.Index);
            if (groceries == null)
            {
                return NotFound();
            }

            return Ok(groceries);
        }

        // GET: api/Groceries/5
        [ResponseType(typeof(Groceries))]
        public IHttpActionResult GetGroceries(Guid id)
        {
            Groceries groceries = db.Groceries.Find(id);
            if (groceries == null)
            {
                return NotFound();
            }

            return Ok(groceries);
        }

        // PUT: api/Groceries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroceries(Guid id, Groceries groceries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groceries.Id)
            {
                return BadRequest();
            }

            db.Entry(groceries).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroceriesExists(id))
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

        // PUT: api/Groceriesindex/5
        [ResponseType(typeof(void))]
        [Route("api/groceries/index")]
        public IHttpActionResult PutIndexGroceries(List<Groceries> groceries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = User.Identity.GetUserId();
            db.Groceries.Where(c => c.UserId.ToString() == user).ToList().ForEach(a => a.Index = groceries.First(t => t.Id == a.Id).Index); 

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Groceries
        [ResponseType(typeof(Groceries))]
        public IHttpActionResult PostGroceries(Groceries groceries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groceries.Add(groceries);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (GroceriesExists(groceries.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = groceries.Id }, groceries);
        }

        // DELETE: api/Groceries/5
        [ResponseType(typeof(Groceries))]
        public IHttpActionResult DeleteGroceries(Guid id)
        {
            Groceries groceries = db.Groceries.Find(id);
            if (groceries == null)
            {
                return NotFound();
            }

            db.Groceries.Remove(groceries);
            db.SaveChanges();

            return Ok(groceries);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroceriesExists(Guid id)
        {
            return db.Groceries.Count(e => e.Id == id) > 0;
        }
    }
}