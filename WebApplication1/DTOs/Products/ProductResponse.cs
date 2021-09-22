﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Products
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int MinQuantity { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
    }
}