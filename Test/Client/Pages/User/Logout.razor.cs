using Microsoft.AspNetCore.Components;
using Test.Client.Interfaces;
using Test.Shared.Entities.DTO;

namespace Test.Client.Pages.User
{
    public partial class Logout
    {

        [Inject]
        public IAuthenticationService? AuthenticationService { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string? Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService?.Logout();
            NavigationManager?.NavigateTo("/");
        }
    }
}
