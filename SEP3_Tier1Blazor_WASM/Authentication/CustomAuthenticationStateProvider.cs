﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SEP3_Tier1Blazor_WASM.Data.UserData;
using SEP3_Tier1Blazor_WASM.Models.UserModels;

namespace SEP3_Tier1Blazor_WASM.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IUserManger userManger;

        public UserShortVersion CachedUser;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUserManger userManger)
        {
            this.jsRuntime = jsRuntime;
            this.userManger = userManger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (CachedUser == null)
            {
                string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    CachedUser = JsonSerializer.Deserialize<UserShortVersion>(userAsJson);
                    identity = SetupClaimsForUser(CachedUser);
                }
            }
            else
            {
                identity = SetupClaimsForUser(CachedUser);
            }

            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public async Task Login(Login login)
        {
            Console.WriteLine("Validating log in");
            if (string.IsNullOrEmpty(login.Email)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(login.Password)) throw new Exception("Enter password");

            ClaimsIdentity identity = new ClaimsIdentity();
            try
            {
                UserShortVersion currentLogged = await userManger.Login(login);

                identity = SetupClaimsForUser(currentLogged);

                string serialisedData = JsonSerializer.Serialize(currentLogged);
                await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
                CachedUser = currentLogged;
            }
            catch (Exception e)
            {
                throw e;
            }


            NotifyAuthenticationStateChanged(
                Task.FromResult<AuthenticationState>(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async void Logout()
        {
            CachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            
            //websocket request 
        }

        private ClaimsIdentity SetupClaimsForUser(UserShortVersion user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("Id", user.UserId.ToString()));
            if (user.UserFullName != null)
                claims.Add(new Claim("Name", user.UserFullName));
            claims.Add(new Claim("AccountType", user.AccountType));
            if (user.Avatar != null)
                claims.Add(new Claim("Avatar", Convert.ToBase64String(user.Avatar)));

            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }

        private ClaimsIdentity SetupClaimsForAdmin(UserShortVersion admin)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("Id", admin.UserId.ToString()));
            claims.Add(new Claim("AccountType", admin.AccountType));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }
    }
}