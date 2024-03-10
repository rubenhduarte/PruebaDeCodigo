using Microsoft.AspNetCore.Components;
using Test.Client.Interfaces;
using Test.Shared.Entities.DTO;

namespace Test.Client.Pages.User
{
    public partial class Login
    {
        private UserAuthenticationRequest _loginModel = new UserAuthenticationRequest();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string? Error { get; set; }

        public async Task ExecuteLogin()
        {
            ShowAuthError = false;

            var result = await AuthenticationService.Login(_loginModel);
            if (!result.IsAuthSuccessful)
            {
                Error = result.ErrorMessage;
                ShowAuthError = true;
            }
            else
            {
                NavigationManager.NavigateTo("/product");
            }
        }
    }
}
