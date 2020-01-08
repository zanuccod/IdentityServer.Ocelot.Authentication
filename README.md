# IdentityServer.Ocelot.Authentication
This example explain a simple authentication with JWT using Identity Server 4 (https://identityserver.io/) and an
API Gateway implemented by Ocelot library (https://github.com/ThreeMammals/Ocelot).

The project is composed of:
  - Identity.IdentityServer.API: Responsible to generate and validate the JWT.
  - Identity.UsersService: Project to simulate a data store for retrieving user information. 
    Used to validate user credentials.
  - ApiGateway.Ocelot.API: An API Gateway to reroute and authenticate incoming connection
  - Services.ServiceOne.API: Simple WebApi project used as target for authenticated call from ConsoleClient.
  - Clients.ConsoleClient: Console client used to test the authentication.
  
# Test
  
To test the implementation run the Identity.IdentityServer.API, ApiGateway.Ocelot.API and the Clients.ConsoleClient.

On ConsoleClient insert username 
```
demo 
```
and password 
```
demo
```
as valid user.
