using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerAccounts.Types.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Stubs
{
    public class StubEmployerAccountsApiClient : IEmployerAccountsApiClient
    {
        private readonly HttpClient _httpClient;

        public StubEmployerAccountsApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("https://sfa-stub-employeraccountapi.herokuapp.com/api-v2/") };
        }

        public async Task<bool> IsUserInRole(IsUserInRoleRequest roleRequest, CancellationToken cancellationToken)
        {
            var user = await GetUser(roleRequest.AccountId, roleRequest.UserRef, cancellationToken).ConfigureAwait(false); ;
            if (user == null) return false;

            foreach (var role in user.Roles)
            {
                if (roleRequest.Roles.Contains(role))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsUserInAnyRole(IsUserInAnyRoleRequest roleRequest, CancellationToken cancellationToken)
        {
            var user = await GetUser(roleRequest.AccountId, roleRequest.UserRef, cancellationToken).ConfigureAwait(false); ;
            return user != null && user.Roles.Any();
        }

        public Task Ping(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        private async Task<UserDto> GetUser(long accountId, Guid userRef, CancellationToken cancellationToken)
        {
            var users = await GetUsers(accountId, cancellationToken).ConfigureAwait(false); ;
            return users.FirstOrDefault(x => x.UserRef == userRef);
        }

        private async Task<IList<UserDto>> GetUsers(long accountId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"accounts/{accountId}/users", cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return new List<UserDto>();
            }
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var items = JsonConvert.DeserializeObject<List<UserDto>>(content);
            return items;
        }

        public class UserDto
        {
            public Guid UserRef { get; set;  }
            public HashSet<UserRole> Roles { get; set; }
        }
    }
}
