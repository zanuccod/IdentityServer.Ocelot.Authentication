using System.Collections.Generic;
using Identity.IdentityServer.API.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity.UsersService.Services;

namespace Identity.IdentityServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new IdentityResource[]
                    {
                        new IdentityResources.OpenId()
                    })
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    // defined in the startup.xs of ServicesOne.API
                    new ApiResource("WebApi_ServiceOne")
                        {
                            ApiSecrets = {new Secret("web_api_service_one_key".Sha256())}
                        }
                })

                .AddInMemoryClients(new List<Client>
                {
                    // defined when ConsoleClient call identityServer to request the token
                    new Client
                        {
                            ClientId = "ConsoleApp_ClientId",
                            ClientSecrets = { new Secret("client_key".Sha256()) },
                            AccessTokenType = AccessTokenType.Reference,
                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            AllowedScopes = { "WebApi_ServiceOne" },
                        }
                })
                .AddProfileService<ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseIdentityServer();
        }
    }
}
