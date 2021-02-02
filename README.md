# **IdentityServer4-Angular-CLI-.NET-5-WebAPI-Boilerplate**<!-- omit in toc -->
This project is a boilerplate for [Asp.NET 5 IdentityServer4](https://github.com/IdentityServer/IdentityServer4), [Angular CLI](https://cli.angular.io/), and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis). The included [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) is dependent on [Entity Framework](https://github.com/dotnet/efcore), [SQL server](https://www.microsoft.com/en-us/sql-server/sql-server-2019), and the [Asp.NET Identity System](https://github.com/dotnet/AspNetCore), although these dependencies can be swapped for custom services if desired. The project comes pre-configured with an Implicit flow client that is connected to the [Angular CLI](https://cli.angular.io/) application via [Open ID Connect (oidc)](https://openid.net/connect/) with the npm packages `angular-oauth2-oidc` and `angular-oauth2-oidcs-jwks`. The [Angular CLI](https://cli.angular.io/) project is blank aside from these dependencies, login logic, and 1 example call to the Resource WebAPI server.

* The [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) is configured to run at the following 2 URIs: http://localhost:5000 and https://localhost:5001.
* The [Angular CLI](https://cli.angular.io/) project can be started by running `ng serve` from the command line in the root of the Angular-WebApp folder and visiting http://localhost:4200. To get started with [Angular CLI](https://cli.angular.io/), check out Angular's provided [Tour of Heroes app and Tutorial](https://angular.io/tutorial).
* The WebAPI project can be accessed at http://localhost:5002 or https://localhost:5003, and a [Swagger](https://swagger.io/)] is available at http://localhost:5002/swagger or https://localhost:5003/swagger.
* [SQL server](https://www.microsoft.com/en-us/sql-server/sql-server-2019) can be easily setup using [DockerHub for SQL Server](https://hub.docker.com/_/microsoft-mssql-server) for either Linux or Window machines.
---
## **Table of Contents**<!-- omit in toc -->
- [**To get started**](#to-get-started)
- [**Configure for https**](#configure-for-https)
- [**Configure for http**](#configure-for-http)
- [**Additional settings**](#additional-settings)
- [**Additional info**](#additional-info)
---
## **To get started**
1. Open the Visual Studio solution, find the **Variables** project, and set the required **Globals.cs** variables for your environment. These variables include:
     * ***Required Variables***
       * **Client_Id** - A unique identifier for your implicit client.
       * **Api_Resource_Name** - The name of the Resource Server which will be validated as the Issuer in Startup.cs of the WebAPI project.
       * **Api_Resource_Scope** - A scope the Resource Server can request from the IdentityServer4
       * **Connection_String** - A SQL server connection string to your SQL server instance to store the database. An example template connection string is below:
         * `Server=;Database=;Trusted_Connection=false;User Id=;Password=;MultipleActiveResultSets=true`
       * **Admin_Username** - The username of the admin user, defaults to 'admin'.
       * **Admin_Password** - The password of the admin user. Requires at least 8 characters with 1 lowercase, 1 uppercase, 1 number, and 1 special character (!, %, &, etc.).
       * **Admin_User_Email** - The email registered in the claims for the Admin user.
     * ***Fields to change when pushing to production***
       * **Certificate_Password** - The private key you specified when creating your .pfx ssl certificate (creating a .pfx certificate with a password is outside of the scope of this readme).
       * **Client_Uri** - The Uri for the implicit client (the Angular CLI WebApp), defaults to `localhost:4200`.
       * **Client_Redirect_Uri** - The Uri to be redirected to after a successful login, defaults to `http://localhost:4200`.
       * **Client_Post_Logout_Redirect_Uri** - The Uri to be redirected to after successful logout, defaults to `http://localhost:4200`.
       * **Client_Allowed_Cors_Origins** - An array of strings of validate origin Uris to request authorization from the IdentityServer, defaults to string[] { `http://localhost:4200` }.
     * ***Optional claims fields that can be specified are***
       * **Admin_User_Name** - The full name of the Admin User.
       * **Admin_User_Given_Name** - The given (first) name of the Admin user.
       * **Admin_User_Family_Name** - The family (last) name of the Admin user.
       * **Admin_User_Website** - The website of the Admin user registered as claims.
       * **Admin_Role** - The default role for the admin, defaults to 'admin'.
     * ***Other fields that can be changed if the hosting Uris are modified include***
       * **IdentityServer_Http_Uri** - The Uri where IdentityServer is hosted over http, defaults to http://localhost:5000.
       * **IdentityServer_Https_Uri** - The Uri where IdentityServer is hosted over https, defaults to https://localhost:5001.
       * **WebApp_Uri** - The Uri where the Angular CLI webapp is hosted, defaults to http://localhost:4200.
       * **WebAPI_Http_Uri** - The Uri where the Asp.NET 5 WebAPI is hosted over http, defaults to http://localhost:5002.
       * **WebAPI_Https_Uri** - The Uri where the Asp.NET 5 WebAPI is hosted over https, defaults to https://localhost:5003.
2. Open the Angular CLI WebApp project and navigate to src -> app -> services -> `globals.service.ts`. Set the required variables for your environment which include:
    * ***Required Variables***
      * **Client_Id** - The unique identifier for your implicit client specified in the **Globals.cs** class.
      * **Client_Scopes** - Space-deliminated list of scopes requested by the Angular CLI webapplication. Be sure to include `openid` (required as it is an OIDC connection) and the `Api_Resource_Scope` specified in the **Globals.cs** class above. `Roles` have been included as an identity resource, and profile is a standard scope for retrieving profile information.
        * e.g. `openid profile roles api.resource.scope`
     * ***Other fields that can be changed if the hosting Uris are modified include:***
       * **IdentityServer_Http_Uri** - 
       * **IdentityServer_Https_Uri** - 
       * **WebAPI_Http_Uri** - 
       * **WebAPI_Https_Uri** - 
       * **WebApp_URI** - 
       * **WebApp_Redirect_Uri** - 
       * **WebApp_Post_Logout_Redirect_Uri** - 
3. Configure desired 3rd Party Authentication:
     * Google
     * Facebook
     * Twitter
     * Twitch
     * Microsoft
4. Configure the projects for [http](#to-configure-for-http) or [https](#to-configure-for-https) (below).
5. Set any other [additional variables](#additional-settings).
6. Start IdentityServer to auto-create / migrate your database, then start the WebAPI and run `ng serve`. Your boilerplate is now complete!
---
## **Configure for https**
1. **Setup Signing Certificate**
    * Create a .pfx certificate in the wwwroot folder called `cert.pfx` from a certified CA for your desired URI. This cert will need a private key which is specified in the **Globals.cs** class. If you do not have an SSL certificate for your domain, a free one can be obtained from [Let's Encrypt](https://letsencrypt.org/). 
    * **By default when IdentityServer is used in production, the signing SSL certificate will need to be included in the IdentityServer wwwroot called `cert.pfx`.**
    * You can modify this logic to fit your needs (i.e. .cer certificate, different name, etc), and this logic is located in IdentityServer project **Startup.cs** `ConfigureServices` (approximately lines 90 - 107):
      ``` c#
          if (Environment.IsDevelopment())
          {
              builder.AddDeveloperSigningCredential();
          }
          else
          {
              // ** IMPORTANT **
              // Required for Production! - Make sure to add cert.pfx into the wwwroot folder
              // If running in Http (not recommended for production), comment out this entire block!
              var filename = Path.Combine(Environment.WebRootPath, "cert.pfx");

              if (!File.Exists(filename))
              {
                  throw new FileNotFoundException("Signing Certificate is missing!");
              }

              var cert = new X509Certificate2(filename, globals.CERTIFICATE_PASSWORD);
              builder.AddSigningCredential(cert);
          }
      ```
2. **Enable HttpsRedirection**
    * In IdentityServer **Startup.cs**, `Configure()`, uncomment (approximately line 170) `app.UseHttpRedirection()` so Asp.NET will automatically redirect all non-secured requested to SSL. 
    
      ``` c#
          app.UseHttpsRedirection();
      ```
      * In ResourceServer **Startup.cs**, `Configure()`, uncomment (approximately line 91) `app.UseHttpRedirection()` so Asp.NET will automatically redirect all non-secured requested to SSL. 
    
      ``` c#
          app.UseHttpsRedirection();
      ```
3. **Verify URIs point to Https**
    * This is by d
---
## **Configure for http**
1. **Disable Signing Certificate**
    * IdentityServer4 requires a TLS / SSL connection - however, you can terminate the SSL connection before the Asp.NET app and pass requests from the reverse proxy in over unsecured port 80. In production, you will still need to include a .pfx SSL certificate with private key called `cert.pfx` in the wwwroot folder. You can remove this requirement by commenting / removing the following lines from IdentityServer Startup.cs in ConfigureServices (approximately lines 90 - 107):
      ``` c#
          if (Environment.IsDevelopment())
          {
              builder.AddDeveloperSigningCredential();
          }
          else
          {
              // ** IMPORTANT **
              // Required for Production! - Make sure to add cert.pfx into the wwwroot folder
              // If running in Http (not recommended for production), comment out this entire block!
              // var filename = Path.Combine(Environment.WebRootPath, "cert.pfx");

              // if (!File.Exists(filename))
              // {
              //     throw new FileNotFoundException("Signing Certificate is missing!");
              // }

              // var cert = new X509Certificate2(filename, globals.CERTIFICATE_PASSWORD);
              // builder.AddSigningCredential(cert);
          }
      ```
2. **Change URIs to Http**
    * Explain how to do this
3. **FOR DEVELOPMENT USING HTTP**
    * In IdentityServer **Startup.cs** `Configure()`, uncomment (approximately line 167) `app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict })`. This is required to store unsecured cookies and redirect correctly from IdentityServer after a successful login. 
    * **Since SSL is required (can be terminated before Asp.NET), in production, Cookies will be secured and this line should be commented out / removed. By default this setting is disabled and should be disabled in production.**
  
      ``` c#
          // app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
      ```
---
## **Additional settings**

To use reference tokens instead of bearer tokens:

To auto redirect after logout:


---
## **Additional info**
For additional information / documentation on how to use the included resources, please see the following links:
* IdentityServer4 Documentation
* AngularCLI Tour of Heroes Example Project
* Asp.NET 5 WebAPI Tutorial
* OAuth 2.0 Protocol
* OIDC Protocol

---
[IdentityServer4-Angular-.NET-5-WebAPI-Boilerplate](https://www.github.com/liveordevtrying/IdentityServer4-Angular-.NET-5-WebAPI-Boilerplate) was created by [LiveOrDevTrying](https://www.liveordevtrying.com) and is maintained by [Pixel Horror Studios](https://www.pixelhorrorstudios.com). 

![Pixel Horror Studios Logo](https://pixelhorrorstudios.s3-us-west-2.amazonaws.com/Packages/PHS.png)
