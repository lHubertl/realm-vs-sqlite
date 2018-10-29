using Realms;

namespace MobileDbs.Infrastructure.Realm.Dto
{
    class EmployeeModelDto : RealmObject
    {
        [PrimaryKey]
        public string Guid { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public string CustomerId { get; set; }
    }
}
