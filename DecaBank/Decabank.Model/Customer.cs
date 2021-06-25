using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecaBank.Models
{
    public class Customer
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte Gender { get; set; }
        public Address Address { get; set; }
    }
}
