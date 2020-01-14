// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using TheMonitaur.Domain.Variables;

namespace TheMonitaur.WebApps.Identity
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new [] { JwtClaimTypes.Role })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(Globals.THEMONITAUR_WEBAPI_SCOPE, "PHS.TheMonitaur.WebAPI", new []
                {
                    "profile", "roles", Globals.THEMONITAUR_WEBAPI_SCOPE
                })
                {
                    ApiSecrets =
                    {
                        new Secret(Globals.THEMONITAUR_WEBAPI_SECRET.Sha256())
                    }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Angular CLI Client
                new Client
                {
                    ClientId = Globals.THEMONITAUR_WEBAPP_CLIENT_ID,
                    ClientName = "The Monitaur WebApplication",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { Globals.THEMONITAUR_WEBAPP_URI },
                    PostLogoutRedirectUris = { Globals.THEMONITAUR_LANDINGPAGE_URI },
                    // LogoUri = "",
                    AllowedCorsOrigins = { Globals.THEMONITAUR_WEBAPP_URI },
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        Globals.THEMONITAUR_WEBAPI_SCOPE
                    },
                    EnableLocalLogin = true,
                    ClientUri = Globals.THEMONITAUR_WEBAPP_URI
                },
            };
        }
    }
}