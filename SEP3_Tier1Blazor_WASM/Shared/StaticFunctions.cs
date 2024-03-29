﻿using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using SEP3_Tier1Blazor_WASM.Models;
using SEP3_Tier1Blazor_WASM.Models.UserModels;

namespace SEP3_Tier1Blazor_WASM.Shared
{
    public static class StaticFunctions
    {
        public static string GetUserUri(string name)
        {
            StringBuilder stringBuilder = new StringBuilder(name);

            if (name.Contains(" "))
            {
                for (int i = 0; i < stringBuilder.Length; i++)
                {
                    if (stringBuilder[i] == (char) 32)
                    {
                        stringBuilder[i] = '.';
                    }
                }
            }

            name = stringBuilder.ToString();
            return name;
        }

        public static UserShortVersion GetLoggedUser(AuthenticationState state)
        {
            UserShortVersion user = new UserShortVersion
            {
                UserId = Int32.Parse(state.User.Claims.First(c => c.Type.Equals("Id")).Value),
                AccountType = state.User.Claims.First(c => c.Type.Equals("AccountType")).Value,
                UserFullName = state.User.Claims.First(c => c.Type.Equals("Name")).Value
            };
            
            if(!user.AccountType.Equals("Administrator"))
            {
                user.Avatar = Convert.FromBase64String(state.User.Claims.First(c => c.Type.Equals("Avatar")).Value);
            }

            return user;
        }

        public static int GetLoggedUserId(AuthenticationState state)
        {
            return Int32.Parse(state.User.Claims.First(c => c.Type.Equals("Id")).Value);
        }
        
        public static string GetUserAvatar(UserShortVersion user)
        {
            return String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(user.Avatar));
        }

        public static string GetImgFromByteArray(byte[] img)
        {
            return String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(img));
        }
    }
}