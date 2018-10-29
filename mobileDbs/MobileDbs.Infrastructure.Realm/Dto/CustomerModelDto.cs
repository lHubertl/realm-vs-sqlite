using Realms;
using System;

namespace MobileDbs.Infrastructure.Realm.Dto
{
    class CustomerModelDto : RealmObject
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
