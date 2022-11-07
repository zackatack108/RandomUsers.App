using Microsoft.AspNetCore.Components;
using RandomUsers.App.Models;
using RandomUsers.App.Services;

namespace RandomUsers.App.Pages
{
    public partial class Index
    {
        [Inject]
        public IRandomUsersApiService? RandomUsersApiService { get; set; }

        public List<UserModel>? Users { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Users = (await RandomUsersApiService.GetRandomUsers("null", 12)).ToList();
        }
    }
}
