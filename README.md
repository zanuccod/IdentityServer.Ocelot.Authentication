# IdentityServer.Ocelot.Authentication
This example explain a simple authentication with JWT using Identity Server 4 (https://identityserver.io/) and an
API Gateway implemented by Ocelot library (https://github.com/ThreeMammals/Ocelot).

The project is composed of:
  - Identity.IdentityServer.API: Responsible to generate and validate the JWT.
  - Identity.UsersService: Project to simulate a data store for retrieving user information. 
    Used to validate user credentials.
  - ApiGateway.Ocelot.API: An API Gateway to reroute and authenticate incoming connection
  - Services.ServiceOne.API: Simple WebApi service used as target for authenticated call from ConsoleClient.
  - Clients.ConsoleClient: Console client used to test the authentication.
 
The scenario is that the client try to get resource from the WebApi service but this one allow the resources only to authorized user.

The authorization is provided from Identity Server and ApiGateway.

The client requests a valid authorization token from the Identity Server that provides it after the credentials have been verified (username and password).

The ApiGateway receive now request from the client with the authorization token and verify again it with the Identity Server.
If the token is valid the request is routed to the WebApi, othewise is blocked.

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
