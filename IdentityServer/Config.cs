// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new [] { JwtClaimTypes.Role })
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource(Globals.API_RESOURCE_SCOPE, "ResourceServer.WebAPI")
                {
                    ApiSecrets =
                    {
                        new Secret(Globals.API_RESOURCE_SECRET.Sha256())
                    },
                    Scopes = new List<string> { "openid", "profile", "roles", Globals.API_RESOURCE_SCOPE }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(Globals.API_RESOURCE_SCOPE, "ResourceServer.WebAPI")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            { 
                // Angular CLI Client
                new Client
                {
                    ClientId = Globals.CLIENT_ID,
                    ClientName = "Angular WebApplication",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { Globals.CLIENT_REDIRECT_URI },
                    PostLogoutRedirectUris = { Globals.CLIENT_POST_LOGOUT_REDIRECT_URI },
                    // LogoUri = "",
                    AllowedCorsOrigins = { Globals.CLIENT_ALLOWED_CORS_ORIGIN },
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        Globals.API_RESOURCE_SCOPE
                    },
                    EnableLocalLogin = true,
                    ClientUri = Globals.CLIENT_URI,
                },
            };
    }
}