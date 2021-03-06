﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Identity.UsersService.Services;

namespace Identity.IdentityServer.API.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        // repository to get user from db
        private readonly IUserService _userRepository;

        public ResourceOwnerPasswordValidator(IUserService userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await _userRepository.FindAsync(context.UserName);
                if (user != null)
                {
                    // TODO: check also for username
                    if (user.Password.ToSha256() == context.Password)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtClaimTypes.Id, user.UserId.ToString() ??  string.Empty),
                            new Claim(JwtClaimTypes.PreferredUserName, user.Username ?? string.Empty),
                            new Claim(JwtClaimTypes.GivenName, user.Firstname  ??  string.Empty),
                            new Claim(JwtClaimTypes.FamilyName, user.Lastname  ??  string.Empty),
                            new Claim(JwtClaimTypes.Email, user.Email  ??  string.Empty),

                            // roles
                            new Claim(JwtClaimTypes.Role, user.Role)
                        };

                        //set the result
                        context.Result = new GrantValidationResult(
                            subject: user.UserId.ToString(),
                            authenticationMethod: "custom",
                            claims: claims);

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType().Name}:Erorr {ex}");
            }
        }
    }
}
