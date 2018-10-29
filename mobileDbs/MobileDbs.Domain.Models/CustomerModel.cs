using System;
using System.Collections.Generic;

namespace MobileDbs.Domain.Models
{
    public class CustomerModel
    {
        public string  Guid { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTimeOffset LastVisit { get; set; }
        public bool IsActive { get; set; }
        public Double Salary { get; set; }
    }
}
