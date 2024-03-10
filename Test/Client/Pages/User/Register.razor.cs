using Microsoft.AspNetCore.Components;
using Test.Client.Interfaces;
using Test.Shared.Entities.DTO;

namespace Test.Client.Pages.User
{
    public partial class Register
    {
        private UserRegisterRequest _registerModel = new UserRegisterRequest();

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public bool ShowRegistrationErros { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public bool success { get; set; }
        public async Task RegisterUser()
        {
            ShowRegistrationErros = false;

            var result = await AuthenticationService.RegisterUser(_registerModel);
            if (!result.IsSuccessfulRegistration)
            {
                Errors = result.Errors;
                ShowRegistrationErros = true;
            }
            else
            {
                success = true;
                NavigationManager.NavigateTo("/");
            }
        }

    }
}
