using System;
using TarkOrm.Attributes;

namespace TarkNoIP.Models
{
    public class Address
    {
        [Key]
        [ReadOnly]
        public int Id { get; set; }
        
        [Column("Address")]
        public string IP { get; set; }

        public int ServerId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}