using Microsoft.AspNetCore.Components;
using RandomUsers.App.Models;

namespace RandomUsers.App.Components
{
    public partial class UserCard
    {
        [Parameter]
        public UserModel UserModel { get; set; }
    }
}
