﻿using IdentityModel;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Clients.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadKey();
        }

        /*
         *  Addresses
         *  Identity Server: https://localhost:5000;http://localhost:5001;
         *  ServiceOne: https://localhost:5002;http://localhost:5003;
         *  ApiGateway: https://localhost:5004;http://localhost:5005;
         */
        static async Task MainAsync(string[] args)
        {
            var httpClient = new HttpClient();

            // Just a sample call with an invalid access token.
            // The expected response from this call is 401 Unauthorized
            //var apiResponse = await httpClient.GetAsync("https://localhost:5002/api/values");
            var apiResponse = await httpClient.GetAsync("https://localhost:5004/api/values");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_access_token");

            // The API is protected, let's ask the user for credentials and exchanged them with an access token
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Connection aborted: Unauthorized");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Blue;
                // Ask User and password
                Console.Write("Username:");
                var username = Console.ReadLine();
                Console.Write("Password:");
                var password = Console.ReadLine();

                // Make the call and get the access token back
                var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = "https://localhost:5001/connect/token",
                    GrantType = "password",

                    ClientId = "ConsoleApp_ClientId",
                    ClientSecret = "client_key",
                    Scope = "WebApi_ServiceOne",

                    UserName = username,
                    Password = password.ToSha256()
                });

                // there's no errors?
                if (!identityServerResponse.IsError)
                {
                    Console.WriteLine();
                    Console.WriteLine("Access Token: ");
                    Console.WriteLine(identityServerResponse.AccessToken);



                    // Call the API with the correct access token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identityServerResponse.AccessToken);

                    //apiResponse = await httpClient.GetAsync("https://localhost:5002/api/values");
                    apiResponse = await httpClient.GetAsync("https://localhost:5004/api/values");

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Connection aborted from ApiGateway: Unauthorized");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("SUCCESS!!");
                        Console.WriteLine();
                        Console.WriteLine("API response:");
                        Console.WriteLine(await apiResponse.Content.ReadAsStringAsync());
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Failed to login with error:");
                    Console.WriteLine(identityServerResponse.ErrorDescription);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine($"Status code {apiResponse.StatusCode}");
                Console.WriteLine("API response:");
                Console.WriteLine(await apiResponse.Content.ReadAsStringAsync());
                Console.WriteLine("YOU ARE NOT PROTECTED!!!");
            }
        }
    }
}
