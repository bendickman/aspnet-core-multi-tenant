﻿using MultiTenant.Core.Interfaces;

namespace MultiTenant.Core.Entities
{
    public class Product 
        : BaseEntity, ITenantable
    {
        public Product(
            string name, 
            string description, 
            decimal rate)
        {
            Name = name;
            Description = description;
            Rate = rate;
        }
        protected Product() { }

        public string TenantId { get; set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Rate { get; private set; }
    }
}
