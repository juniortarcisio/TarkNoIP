using System;
using TarkOrm.Attributes;

namespace TarkNoIP.Models
{
    public class Server
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ServiceId { get; set; }

        public DateTime LastKeepAlive { get; set; }
    }
}