using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using TarkNoIP.Models;
using TarkNoIP.ViewModels;

namespace TarkNoIP.Controllers
{
    public class ServerController : ApiController
    {
        TarkOrm.TarkOrm torm = new TarkOrm.TarkOrm("localhost");

        public string Get()
        {
            return "abc";
        }

        public IEnumerable<UpdatedServer> Get(int Id)
        {
            var servers = torm.GetWhere<Server, int>(x => x.ServiceId, 1);
            var addresses = torm.GetAll<Address>();

            var updatedServers = from server in servers
                                 where server.LastKeepAlive > DateTime.Now.AddMinutes(-120)
                                 select new UpdatedServer()
                                 {
                                     Name = server.Name,
                                     Description = server.Description,
                                     Address = addresses.Where(x => x.ServerId == server.Id).
                                                         OrderByDescending(x => x.CreationDate).
                                                         FirstOrDefault().IP
                                 };

            return updatedServers;
        }
        
        public string Post()
        {
            return "abc";
        }
        
    }
}
