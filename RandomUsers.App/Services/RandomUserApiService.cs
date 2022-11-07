using Blazored.LocalStorage;
using RandomUsers.App.Helper;
using RandomUsers.App.Models;
using System.Text.Json;

namespace RandomUsers.App.Services
{
    public class RandomUserApiService : IRandomUsersApiService
    {
        private readonly HttpClient? _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public RandomUserApiService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<List<UserModel>> GetRandomUsers(string gender, int total, bool refreshedRequired = false)
        {
            if (refreshedRequired)
            {
                bool UserExpirationExists = await _localStorageService.ContainKeyAsync(LocalStorageConstants.UserListExpirationKey);
                if (UserExpirationExists)
                {
                    DateTime userListExpiration = await _localStorageService.GetItemAsync<DateTime>(LocalStorageConstants.UserListExpirationKey);
                    if(userListExpiration > DateTime.Now)
                    {
                        if(await _localStorageService.ContainKeyAsync(LocalStorageConstants.UserListKey))
                        {
                            return await _localStorageService.GetItemAsync<List<UserModel>>(LocalStorageConstants.UserListKey);
                        }
                    }
                }
            }
            var url = string.Format($"/api/?nat=us&gender={gender}&results={total}&inc=gender,name,location,email,phone,picture");
            var resultRoot = new RandomUserModel.Rootobject();
            var result = new List<UserModel>();
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                //var test = JsonSerializer.Deserialize<UserModel.Rootobject>(stringResponse);
                if (stringResponse != null)
                {
                    resultRoot = JsonSerializer.Deserialize<RandomUserModel.Rootobject>(stringResponse,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }                

                if (resultRoot != null)
                {
                    foreach (var r in resultRoot.Results)
                    {
                        UserModel user = new UserModel
                        {
                            FirstName = r.Name.First,
                            LastName = r.Name.Last,
                            Gender = r.Gender,
                            Phone = r.Phone,
                            Picture = r.Picture.Large,
                            Location = new UserModel.Loc
                            {
                                City = r.Location.City,
                                State = r.Location.State,
                                Country = r.Location.Country,
                                Postcode = r.Location.Postcode,
                                Street = new UserModel.Street
                                {
                                    Name = r.Location.Street.Name,
                                    Number = r.Location.Street.Number
                                }
                            }
                        };
                        result.Add(user);
                    }
                }
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            await _localStorageService.SetItemAsync(LocalStorageConstants.UserListKey, result);
            await _localStorageService.SetItemAsync(LocalStorageConstants.UserListExpirationKey, DateTime.Now.AddMinutes(1));

            return result;
        }

    }
}
