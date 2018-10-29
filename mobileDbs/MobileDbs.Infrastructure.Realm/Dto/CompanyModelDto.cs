using Realms;

namespace MobileDbs.Infrastructure.Realm.Dto
{
    class CompanyModelDto : RealmObject
    {
        [PrimaryKey]
        public string  Guid { get; set; }
        public string Name { get; set; }
        public string FounderName { get; set; }
        public string Co_FounderName { get; set; }
    }
}
