﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persistence.Entities
{
    public class City
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }
    }
}
