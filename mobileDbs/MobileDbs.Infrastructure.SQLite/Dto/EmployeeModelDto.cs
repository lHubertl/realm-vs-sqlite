using SQLite;

namespace MobileDbs.Infrastructure.SQLite.Dto
{
    [Table("EmployeeTable")]
    class EmployeeModelDto
    {
        [PrimaryKey]
        public string Guid { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public string CustomerId { get; set; }
    }
}
