﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCpMercadoLibre.Models
{
    public class Destination
    {
        public Fiscal_Information fiscal_information { get; set; }
        public Address address { get; set; }
    }
}
