using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsAPI.Models
{
    public class Shipments
    {
        public int Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public string Status { get; set; }
    }
}