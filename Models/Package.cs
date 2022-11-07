using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCpMercadoLibre.Models
{
    public class Package
    {
        public List<Items> items { get; set; }
        public int total_items { get; set; }
    }
}
