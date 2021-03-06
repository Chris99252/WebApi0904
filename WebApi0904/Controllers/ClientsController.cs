﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi0904.Models;

namespace WebApi0904.Controllers
{
    // 前置詞
    [RoutePrefix("clients")]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Clients
        [Route("")]
        public IQueryable<Client> GetClient()
        {
            return db.Client;
        }

        [Route("get1")]
        public IHttpActionResult GetClient1()
        {
            return Ok(db.Client);
        }

        [Route("get2")]
        public HttpResponseMessage GetClient2()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IQueryable<Client>>(db.Client, GlobalConfiguration.Configuration.Formatters.JsonFormatter),
                ReasonPhrase = "VERY OK"
            };
        }

        [Route("get3")]
        public HttpResponseMessage GetClient3()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.Client);
        }

        [Route("getString1")]
        public string GetClient4()
        {
            return "熊貓克里斯";
        }

        [Route("getString2")]
        public HttpResponseMessage GetClient5()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("熊貓克里斯", Encoding.GetEncoding("Big5")),
            };
        }

        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        [Route("{id}", Name = "GetClientById")]
        public IHttpActionResult GetClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // GET: /clients/5/orders
        [ResponseType(typeof(Client))]
        [Route("{id}/orders")]
        [Route("~/ClientOrders/{id}")] // 保留原本路由
        public IHttpActionResult GetClientOrders(int id)
        {
            List<Order> orders = db.Order.Where(p => p.ClientId == id).ToList();

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // GET: /clients/5/orders/2001/11/25
        [ResponseType(typeof(Client))]
        [Route("{id}/orders/{*date:datetime}")]
        public IHttpActionResult GetClientOrders(int id, DateTime date)
        {
            List<Order> orders = db.Order
                .Where(p => p.ClientId == id
                && p.OrderDate.Value.Year == date.Year
                && p.OrderDate.Value.Month == date.Month
                && p.OrderDate.Value.Day == date.Day).ToList();

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // GET: /clients/5/orders/pending
        [ResponseType(typeof(Client))]
        [Route("{id}/orders/pending")]
        public IHttpActionResult GetClientOrdersPending(int id)
        {
            List<Order> orders = db.Order
                .Where(p => p.ClientId == id && p.OrderStatus == "P").ToList();

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        [Route("{id}")]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [ResponseType(typeof(Client))]
        [Route("")]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("GetClientById", new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [ResponseType(typeof(Client))]
        [Route("{id}")]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}