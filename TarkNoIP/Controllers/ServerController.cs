using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using TarkNoIP.Models;
using TarkNoIP.ViewModels;
using System.Net.Http;
using System.Web;

namespace TarkNoIP.Controllers
{
    [RoutePrefix("Server")]
    public class ServerController : ApiController
    {
        TarkOrm.TarkOrm torm = new TarkOrm.TarkOrm("localhost");

        public IEnumerable<Server> Get()
        {
            return torm.GetAll<Server>();
        }

        public IEnumerable<UpdatedServer> Get(int serviceId)
        {
            var servers = torm.GetWhere<Server, int>(x => x.ServiceId, serviceId);
            var addresses = torm.GetAll<Address>();

            var updatedServers = servers
                                .Where(x => x.LastKeepAlive > DateTime.Now.AddMinutes(-30))
                                .Select(x => new UpdatedServer()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Description = x.Description
                                })
                                .ToList();

            foreach (var item in updatedServers)
            {
                var address = addresses.Where(x => x.ServerId == item.Id).
                              OrderByDescending(x => x.CreationDate).
                              FirstOrDefault();

                if (address != null)
                    item.Address = address.IP;
            }

            return updatedServers;
        }
        
        public void Post([FromBody] Server server)
        {
            torm.Add(server);
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
        
        [HttpPost]
        [Route("KeepAlive/{id}")]
        public string KeepAlive(int id)
        {
            var requestIp = GetClientIp(Request);
            var server = torm.GetById<Server>(id);
            var addresses = torm.GetWhere<Address, int>(x => x.ServerId, id).OrderByDescending(x => x.CreationDate).FirstOrDefault();

            if (addresses == null || requestIp != addresses.IP)
            {
                var newAddress = new Address()
                {
                    CreationDate = DateTime.Now,
                    IP = requestIp,
                    ServerId = id
                };

                torm.Add(newAddress);
            }

            server.LastKeepAlive = DateTime.Now;
            torm.Update(server);

            return requestIp;
        }   
    }
}
