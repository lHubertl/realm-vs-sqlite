using SQLite;

namespace MobileDbs.Infrastructure.SQLite.Dto
{
    [Table("CompanyTable")]
    class CompanyModelDto
    {
        [PrimaryKey]
        public string Guid { get; set; }
        public string Name { get; set; }
        public string FounderName { get; set; }
        public string Co_FounderName { get; set; }
    }
}
