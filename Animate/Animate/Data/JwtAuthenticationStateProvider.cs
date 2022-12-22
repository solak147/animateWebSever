using Animate.Data.Utility;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Animate.Data
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient httpClient;

        private AuthenticationState anonymous;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            this.localStorageService = localStorageService;
            this.httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            string tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            if (string.IsNullOrEmpty(tokenInLocalStorage))
            {
                //沒有的話，回傳匿名使用者
                return anonymous;
            }

            var claims = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", tokenInLocalStorage);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        public void NotifyUserAuthentication(string token)
        {
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogOut()
        {
            var authState = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
