// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;
using Variables;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources(IGlobals globals)
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new [] { JwtClaimTypes.Role })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(IGlobals globals)
        {
            return new List<ApiResource>
            {
                new ApiResource(globals.API_RESOURCE_NAME, "ResourceServer.WebAPI")
                {
                    // No secrets for Implicit Flow
                    //ApiSecrets =
                    //{
                    //    new Secret(globals.API_RESOURCE_SECRET.Sha256())
                    //},
                    Scopes = globals.WEBAPI_REQUESTED_SCOPES.ToList()
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes(IGlobals globals)
        {
            return new ApiScope[]
            {
                new ApiScope(globals.API_RESOURCE_SCOPE, "ResourceServer.WebAPI")
            };
        }

        public static IEnumerable<Client> GetClients(IGlobals globals)
        {
            return new Client[]
            { 
                // Angular CLI Client
                new Client
                {
                    ClientId = globals.CLIENT_ID,
                    ClientName = "Angular WebApplication",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { globals.CLIENT_REDIRECT_URI },
                    PostLogoutRedirectUris = { globals.CLIENT_POST_LOGOUT_REDIRECT_URI },
                    // LogoUri = "",
                    AllowedCorsOrigins = globals.CLIENT_ALLOWED_CORS_ORIGINS,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        globals.API_RESOURCE_SCOPE
                    },
                    EnableLocalLogin = true,
                    ClientUri = globals.CLIENT_URI,
                    // AccessTokenType = AccessTokenType.Reference
                },
            };
        }
    }
}