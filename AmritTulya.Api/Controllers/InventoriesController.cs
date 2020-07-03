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
using AmritTulya.EntityLayer;

namespace AmritTulya.Api.Controllers
{
    public class InventoriesController : ApiController
    {
        private AmritTulyaDbContext db = new AmritTulyaDbContext();

        // GET: api/Inventories
        [Route("api/Inventories/GetProducts")]
        public IQueryable<Inventory> GetInventories()
        {
            return db.Inventories;
        }

        // GET: api/Inventories/5
        [Route("api/Inventories/{id}")]
        [ResponseType(typeof(Inventory))]
        public IHttpActionResult GetInventory(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // PUT: api/Inventories/5
        [HttpPost,Route("api/Inventories/UpdateProduct/")]
        //[ResponseType(typeof(void))]
        [ResponseType(typeof(Inventory))]
        public IHttpActionResult PutInventory(Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (inventory.Id != inventory.Id)
            {
                return BadRequest();
            }

            db.Entry(inventory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(inventory.Id))
                {
                    return NotFound();
                }
                    else
                {
                    throw;
                }
            }
            return Ok(inventory);
        }


        // PUT: api/Inventories/5
        [HttpPost, Route("api/Inventories/UpdateImage/")]
        //[ResponseType(typeof(void))]
        [ResponseType(typeof(Inventory))]
        public IHttpActionResult UpdateImage(Inventory inventory)
        {
            if (inventory.Id != inventory.Id)
            {
                return BadRequest();
            }

            Inventory invObj = db.Inventories.Find(inventory.Id);

           // invObj.Id = invObj.Id;
            invObj.Name = invObj.Name;
            invObj.IsDeleted = invObj.IsDeleted;
            invObj.Price = invObj.Price;
            invObj.InventoryImage = inventory.InventoryImage;
            invObj.Description = invObj.Description;
            invObj.ImagePath = inventory.ImagePath;

            //db.Entry(inventory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(inventory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(inventory);

        }


        // POST: api/Inventories
        [Route("api/Inventories/AddProduct")]
        [ResponseType(typeof(Inventory))]
        public IHttpActionResult PostInventory([FromBody]Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Inventories.Add(inventory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = inventory.Id }, inventory);
        }

        // DELETE: api/Inventories/5
        [Route("api/Inventories/DeleteProduct/{id}")]
        [ResponseType(typeof(Inventory))]
        public IHttpActionResult DeleteInventory([FromUri]int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return NotFound();
            }

            db.Inventories.Remove(inventory);
            db.SaveChanges();

            return Ok(inventory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InventoryExists(int id)
        {
            return db.Inventories.Count(e => e.Id == id) > 0;
        }
    }
}