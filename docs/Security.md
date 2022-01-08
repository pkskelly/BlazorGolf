## Security

## OAuth and PKCE

## Azure AD (not B2C) Configuration

The app uses both permission scopes and roles.  More to come

## Blazor Specific Custom User Factory and User Objects

The application uses Azure AD App Roles and scopes to determine what UI features to show and to secure certain actions (POST for some API calls for exmaple).

## Links and Resources
[Azure Active Directory (AAD) groups, Administrator Roles, and App Roles](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/azure-active-directory-groups-and-roles?view=aspnetcore-6.0)


## OAuth 2.0 and PKCE

The application uses PKCE and does not store token in cookies. 

### Using Postman and PKCE

So far, i can configure Postman to use PKCE by creating a "Mobile and desktop app" redirect uri to the postman PKCE url, but the roles are not returned to test with Postman. 