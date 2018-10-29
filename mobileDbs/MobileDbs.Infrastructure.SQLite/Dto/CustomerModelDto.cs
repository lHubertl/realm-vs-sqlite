using SQLite;
using System;

namespace MobileDbs.Infrastructure.SQLite.Dto
{
    [Table("CustomerTable")]
    class CustomerModelDto
    {
        [PrimaryKey]
        public string Guid { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTimeOffset LastVisit { get; set; }
        public bool IsActive { get; set; }
        public Double Salary { get; set; }
    }
}
