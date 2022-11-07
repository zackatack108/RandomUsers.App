
namespace RandomUsers.App.Models
{
    public class UserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Picture { get; set; }
        public Loc? Location { get; set; }

        public class Loc
        {
            public Street? Street { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? Country { get; set; }
            public int Postcode { get; set; }
        }
        public class Street
        {
            public int?Number { get; set; }
            public string? Name { get; set; }
        }
    }
}
