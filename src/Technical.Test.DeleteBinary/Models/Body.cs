using System;

namespace Technical.Test.DeleteBinary.Models
{
    public class Body
    {
        public string Key { get; set; }

        public bool IsRecycler { get; set; }

        public string RecyclerId { get; set; }

        public int Days { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
