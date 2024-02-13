﻿using Newtonsoft.Json;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerAccounts.Types.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Stubs;

public class StubEmployerAccountsApiClient : IEmployerAccountsApiClient
{
    private readonly HttpClient _httpClient;

    public StubEmployerAccountsApiClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:3999/accounts-api/") };
    }

    public async Task<bool> IsUserInRole(IsUserInRoleRequest roleRequest, CancellationToken cancellationToken)
    {
        var user = await GetUser(roleRequest.AccountId, roleRequest.UserRef, cancellationToken).ConfigureAwait(false); ;
        if (user == null)
        {
            return false;
        }

        return user.Roles.Any(role => roleRequest.Roles.Contains(role));
    }

    public async Task<bool> IsUserInAnyRole(IsUserInAnyRoleRequest roleRequest, CancellationToken cancellationToken)
    {
        var user = await GetUser(roleRequest.AccountId, roleRequest.UserRef, cancellationToken).ConfigureAwait(false); ;
        return user != null && user.Roles.Any();
    }

    public Task Ping(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<UserDto> GetUser(long accountId, Guid userRef, CancellationToken cancellationToken)
    {
        var users = await GetUsers(accountId, userRef, cancellationToken).ConfigureAwait(false); ;
        return users.FirstOrDefault(x => x.UserRef == userRef);
    }

    private async Task<IList<UserDto>> GetUsers(long accountId, Guid currentUserRef, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"accounts/{accountId}/users", cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            // This is a temp fix to overcome the now defunct Employer UserAccounts heroku stub, if the stub fails we simply return the current user
            // so we can run this locally, the stub can be run locally, but the ROI isn't worthwhile considering we not generaLLY interested in permissions
            return new List<UserDto> { new UserDto { UserRef = currentUserRef, Roles = new HashSet<UserRole> { UserRole.Owner } } };
        }
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var items = JsonConvert.DeserializeObject<List<UserDto>>(content);
        return items;
    }

    public class UserDto
    {
        public Guid UserRef { get; set;  }
        public HashSet<UserRole> Roles { get; set; }
    }
}