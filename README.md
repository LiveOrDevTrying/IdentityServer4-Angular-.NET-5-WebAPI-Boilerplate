# **Boilerplate for: [Asp.NET 5 IdentityServer4](https://github.com/IdentityServer/IdentityServer4), [Angular CLI](https://cli.angular.io/), and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis)**<!-- omit in toc -->
This project is a boilerplate for [Asp.NET 5 IdentityServer4](https://github.com/IdentityServer/IdentityServer4), [Angular CLI](https://cli.angular.io/), and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis). The included [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server is dependent on [Entity Framework](https://github.com/dotnet/efcore), [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2019), and the [Asp.NET Identity System](https://github.com/dotnet/AspNetCore), although these dependencies can be swapped for custom services / alternative database services if desired. The project comes pre-configured with an [implicit flow client](https://tools.ietf.org/html/rfc6749#section-4.2) that is configured to the provided [Angular CLI](https://cli.angular.io/) webapp via [Open ID Connect (OIDC)](https://openid.net/connect/) using the [npm packages](https://www.npmjs.com/) [angular-oauth2-oidc](https://www.npmjs.com/package/angular-oauth2-oidc) and [angular-oauth2-oidc-jwks](https://www.npmjs.com/package/angular-oauth2-oidc-jwks). The [Angular CLI](https://cli.angular.io/) project is empty aside from these dependencies, the example login logic, and 1 example call to the protected Resource WebAPI server.

* The [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server (`IdentityServer.csproj`) is configured to run in development at the following 2 URIs:
  * [http://localhost:5000](#)
  * [https://localhost:5001](#)
* The [Angular CLI](https://cli.angular.io/) project (`/Angular-WebApp`) can be started in development by running `ng serve` from the command line in the root of the Angular-WebApp folder and visiting [http://localhost:4200](#). To get started with [Angular CLI](https://cli.angular.io/), check out the Angular provided [Tour of Heroes app and Tutorial](https://angular.io/tutorial).
* The [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) project (`ResourceServer.csproj`) is configured to run in development at the following 2 URIs:
  * [http://localhost:5002](#)
  * [https://localhost:5003](#)
  * A [Swagger](https://swagger.io/) is available at: 
    * [http://localhost:5002/swagger](#)
    * [https://localhost:5003/swagger](#)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2019) can be easily setup using [DockerHub for SQL Server](https://hub.docker.com/_/microsoft-mssql-server) for either Linux or Window machines.
## **To Get Started**<!-- omit in toc -->
1. First, [set the Globals.cs variables](#set-globals.cs-variables).
2. Next, [set the Angular CLI webapp variables](#set-the-angular-cli-webapp-variables).
3. Configure desired [3rd Party Authentication](#configure-for-3rd-party-auth).
4. Configure for either [Https](#to-configure-for-https) (**recommended**) or [Http](#to-configure-for-http).
5. Set any [additional variables](#additional-settings).
6. Start the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server to auto-create / migrate your database specified in the **`Connection_String`** value in **Globals.cs**. Next start the [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis). Run `ng serve` from the command line in the Angular-WebApp folder to start the development [Angular CLI](https://cli.angular.io/) webapp. Your boilerplate is now complete!
---
## **Table of Contents**<!-- omit in toc -->
- [**Set Globals.cs Variables**](#set-globals.cs-variables)
- [**Set the Angular CLI Webapp Variables**](#set-the-angular-cli-webapp-variables)
- [**Configure for 3rd Party Auth**](#configure-for-3rd-party-auth)
- [**Configure for Https**](#configure-for-https)
- [**Configure for Http**](#configure-for-http)
- [**Additional Settings**](#additional-settings)
  - [**Reference Tokens**](#reference-tokens)
  - [**Auto-Redirect After Logout**](#auto-redirect-after-logout)
- [**Additional Info**](#additional-info)
---
## **Set Globals.cs Variables**
Open the Visual Studio solution, find the **Variables** project, and set the required **Globals.cs** variables for your environment. These variables include:
  * ***Required variables***
    * **`Client_Id`** - A unique identifier for your [implicit client](https://tools.ietf.org/html/rfc6749#section-4.2). More information regarding `Client_id` selection is available in [Section 2.2 of the RFC](https://tools.ietf.org/html/rfc6749#section-2.2).
    * **`Api_Resource_Name`** - The name of the Resource Server which will be validated as the Issuer in the `UseJWTBearer()` call in **Startup.cs** `ConfigureServices()` of the Resource Server WebAPI project.
      ``` c#
        // This is for access tokens
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = globals.IDENTITYSERVER_HTTPS_URI;
                options.RequireHttpsMetadata = false;
                options.Audience = globals.API_RESOURCE_NAME;
            });
      ```
    * **`Api_Resource_Scope`** - A [scope](https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims) registered to the [implicit client](https://tools.ietf.org/html/rfc6749#section-4.2) in the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server database that a Resource Server can request.
    * **`Connection_String`** - A [SQL server connection string](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring?view=dotnet-plat-ext-5.0) to your [SQL Server](https://hub.docker.com/_/microsoft-mssql-server) instance to store the database. An example template connection string is below:
      * `Server=;Database=;Trusted_Connection=false;User Id=;Password=;MultipleActiveResultSets=true`
    * **`Admin_Password`** - The password of the Admin user. Requires at least 8 characters with 1 lowercase, 1 uppercase, 1 number, and 1 special character (!, %, &, etc.).
    * **`Admin_User_Email`** - The email registered as a claim for the Admin user.
  * ***Variables to change when pushing to production***
    * **`Certificate_Password`** - The private key you specified when [creating your .pfx ssl certificate](#configure-for-https).
    * **`Client_Uri`** - The Uri for the [implicit client](https://tools.ietf.org/html/rfc6749#section-4.2) (the [Angular CLI](https://cli.angular.io/) webapp). Defaults to [localhost:4200](#).
    * **`Client_Redirect_Uri`** - The Uri the browser will be redirected to after a successful login. Defaults to [http://localhost:4200](#).
    * **`Client_Post_Logout_Redirect_Uri`** - The Uri the browser will be redirected to after a successful logout. Defaults to [http://localhost:4200](#).
    * **`Client_Allowed_Cors_Origins`** - An array of strings containing valid origin Uris from which to request authorization from the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server. Defaults to string[] { [http://localhost:4200](#) }.
  * ***Optional admin user / claims variables that can be specified***
    * **`Admin_Username`** - The username of the Admin user. Defaults to `admin`.
    * **`Admin_User_Full_Name`** - The full name of the Admin User.
    * **`Admin_User_Given_Name`** - The given (first) name of the Admin user.
    * **`Admin_User_Family_Name`** - The family (last) name of the Admin user.
    * **`Admin_User_Website`** - The website of the Admin user registered as a claim.
    * **`Admin_Role`** - The default role for the Admin user. Defaults to `admin`.
  * ***Other variables that can be changed if the hosting Uris are modified***
    * **`IdentityServer_Http_Uri`** - The Uri where the development [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server is hosted over Http. Defaults to [http://localhost:5000](#).
    * **`IdentityServer_Https_Uri`** - The Uri where the development [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server is hosted over Https. Defaults to [https://localhost:5001](#).
    * **`WebApp_Uri`** - The Uri where the development [Angular CLI](https://cli.angular.io/) webapp is hosted. Defaults to [http://localhost:4200](#).
    * **`WebAPI_Http_Uri`** - The Uri where the development [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) is hosted over Http. Defaults to [http://localhost:5002](#).
    * **`WebAPI_Https_Uri`** - The Uri where the development [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) is hosted over Https. Defaults to [https://localhost:5003](#).
---
## **Set the [Angular CLI](https://cli.angular.io/) Webapp Variables**
Open the [Angular CLI](https://cli.angular.io/) webapp project and navigate to src -> app -> services -> `globals.service.ts`. Set the required variables for your environment which include:
  * ***Required variables***
    * **`Client_Id`** - The unique identifier for your [implicit client](https://tools.ietf.org/html/rfc6749#section-4.2). This should match the value specified in **Globals.cs** above.
    * **`Client_Scopes`** - Space-deliminated list of scopes requested by the [Angular CLI](https://cli.angular.io/) webapp. Be sure to include `openid` (required as this boilerplate uses [OIDC](https://openid.net/connect/) to connect to the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server) and the `Api_Resource_Scope` value specified in **Globals.cs** above. `Roles` have been included as an additional identity resource, and `profile` is a [standard claim for retrieving profile information](https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims).
      * e.g. `openid profile roles api.resource.scope`
  * ***Other variables that can be changed if the hosting Uris are modified***
    * **`IdentityServer_Http_Uri`** - The Uri where the development [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server is hosted over Http. Defaults to [http://localhost:5000](#).
    * **`IdentityServer_Https_Uri`** - The Uri where the development [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server is hosted over Https. Defaults to [https://localhost:5001](#).
    * **`WebAPI_Http_Uri`** - The Uri where the development [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) is hosted over Http. Defaults to [http://localhost:5002](#).
    * **`WebAPI_Https_Uri`** - The Uri where the development [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) is hosted over Https. Defaults to [https://localhost:5003](#).
    * **`WebApp_URI`** - The Uri where the development [Angular CLI](https://cli.angular.io/) webapp is hosted. Defaults to [http://localhost:4200](#).
    * **`WebApp_Redirect_Uri`** - The Uri where the development [Angular CLI](https://cli.angular.io/) webapp will be redirected to after a successful login. Defaults to [http://localhost:4200](#).
    * **`WebApp_Post_Logout_Redirect_Uri`** - The Uri where the development [Angular CLI](https://cli.angular.io/) webapp will be redirected to after a successful logout. Defaults to [http://localhost:4200](#).
---
## **Configure for 3rd Party Auth**
If you are using multiple 3rd party providers, make sure to call `services.AddAuthentication()` once and chain your provider calls together like below:
  
  ``` c#
    services.AddAuthentication()
      .AddGoogle(options =>
      {
          options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
          options.ClientId = "";
          options.ClientSecret = "";
      })
      .AddTwitch(options =>
      {
          options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
          options.ClientId = "";
          options.ClientSecret = "";
      });
  ```

  * **Google**
    * Login to the [Google Developer Portal](https://console.developers.google.com/) and create a new project. Register an OAuth 2.0 client. You will need to specify an `authorized redirect uri` (e.g. for development, [http://localhost:5000/signin-google](#) or [https://localhost:5001/signin-google](#)) and if you like, an `authorized JavaScript origin` (e.g. for development, [http://localhost:4200](#)). Make note of the `Client Id` and `Client Secret` provides your OAuth 2.0 client - you will need these in the next step.
    * In the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server project **Startup.cs** `ConfigureServices()` method, uncomment the following lines and add in the `Client Id` and `Client Secret` you retrieved in the previous step:

      ``` c#
        services.AddAuthentication()
          .AddGoogle(options =>
          {
              options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
              options.ClientId = "";
              options.ClientSecret = "";
          });
      ```

    * In production, you will need to change these Uris to match the domain / Uri where the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) are hosted.
  * **Facebook**
    * Login to the [Facebook Developer Portal](https://developers.facebook.com/) and create a new app. Add in the specified Uris such as the app domain (`localhost` for development), privacy Uri, and terms of service Uri. On the left under products, click on **Facebook Login** -> **Settings**. Make sure `Client OAuth Login` and `WebOAuth Login` are enabled and optionally enable `Enforce Https` (if your [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server is being hosted on SSL). Add in the `Valid OAuth Redirect URIs` (e.g. for development, [http://localhost:5000/signin-facebook](#) or [https://localhost:5001/signin-facebook](#)). Under **Settings** -> **Basic**, make note of the `app Id` and `app secret` - you will need these in the next step.
    * In the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server project **Startup.cs** `ConfigureServices()` method, uncomment the following lines and add in the `app id` (as **Client Id**) and `app secret` (as **Client Secret**) you retrieved in the previous step:

      ``` c#
        services.AddAuthentication()
          .AddFacebook(options =>
          {
              options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
              options.ClientId = "";
              options.ClientSecret = "";
          });
      ```

    * In production, you will need to change these Uris to match the domain / Uri where the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) are hosted.
  * **Twitter**
    * Login to the [Twitter Developer Portal](https://developer.twitter.com/) and create a new project and a new app in the project. Under the details for the app, add in under `callback urls` the appropriate Uri for your [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server (e.g. for development, [http://localhost:5000/signin-twitter](#) or [https://localhost:5001/signin-twitter](#)). Click on `Keys and Token` under the title of the app on the App details page, and under `Consumer Keys`, create a new `API Key & Secret`. Make note of the `api key` and `app secret` - you will need these in the next step.
    * In the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server project **Startup.cs** `ConfigureServices()` method, uncomment the following lines and add in the `api key` (as **Consumer Key**) and `api secret` (as **Client Secret**) you retrieved in the previous step:

      ``` c#
        services.AddAuthentication()
          .AddTwitter(options =>
          {
              options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
              options.ConsumerKey = "";
              options.ConsumerSecret = "";
          });
      ```

      * In production, you will need to change these Uris to match the domain / Uri where the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) are hosted.
  * **Twitch**
    * Login to the [Twitch Developer Portal](https://dev.twitch.tv/) and create a new application. Specify the category of the application as `Website integration`, and add in the appropriate `OAuth Redirect URIs` (e.g. for development, [http://localhost:5000/signin-twitch](#) or [https://localhost:5001/signin-twitch](#)). Make note of the `Client Id` and `Client Secret` - you will need these in the next step.
    * In the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server project **Startup.cs** `ConfigureServices()` method, uncomment the following lines and add in the `Client Id` and `Client Secret` you retrieved in the previous step:

      ``` c#
        services.AddAuthentication()
          .AddTwitch(options =>
          {
              options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
              options.ClientId = "";
              options.ClientSecret = "";
          });
      ```
    * In production, you will need to change these Uris to match the domain / Uri where the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) are hosted.
  * **Microsoft**
    * Login to [Azure portal - App Registrations](https://go.microsoft.com/fwlink/?linkid=2083908) and create a new app registration. Specify the app registration name and add the appropriate `Redirect Uri` (e.g. set it as `Web`, and for development, the Uri is either [http://localhost:5000/signin-microsoft](#) or [https://localhost:5001/signin-microsoft](#)). Click `Register`. Make note of the `Application (client) ID` - you will need this in the next step. On the left, click on `Certificates & secrets`. Under `Client Secrets`, add a new `Client secret` and take note of the value - you will need this in the next step.
    * In the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server project **Startup.cs** `ConfigureServices()` method, uncomment the following lines and add in the `Client Id` and `Client Secret` you retrieved in the previous step:

      ``` c#
        services.AddAuthentication()
          .AddMicrosoftAccount(options =>
          {
              options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
              options.ClientId = "";
              options.ClientSecret = "";
          });
      ```

    * In production, you will need to change these Uris to match the domain / Uri where the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server and [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) are hosted.
---
## **Configure for Https**
1. **Setup Signing Certificate**
    * Copy a `.pfx certificate` into the wwwroot folder called `cert.pfx` from a known certified authority (CA) for your desired domain / Uri for the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server. You will need to export this certificate as a .pfx certificate with a private key that matches the key specified in **Globals.cs**. If you do not have an SSL certificate for your domain, a free one can be obtained from [Let's Encrypt](https://letsencrypt.org/). After obtaining a certificate, you will need to export it with a private key (preferably in the .pfx format).
    
    > **When the IdentityServer is used in production, the signing SSL certificate will need to be included in the IdentityServer wwwroot called `cert.pfx` unless you change the signing certificate logic (below).**
    
    * Copy your `cert.pfx` into the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server wwwroot folder.
    * You can modify the certificate logic to fit your needs (i.e. .cer certificate, different name, etc). This logic is located in [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server project **Startup.cs** `ConfigureServices` (approximately lines 90 - 107):
      
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
    * In the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server **Startup.cs**, `Configure()`, uncomment (approximately line 170) `app.UseHttpRedirection()` so [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) will automatically redirect all non-secured requested to SSL. 
    
      ``` c#
          app.UseHttpsRedirection();
      ```
      * In ResourceServer **Startup.cs**, `Configure()`, uncomment (approximately line 91) `app.UseHttpRedirection()` so [Asp.NET 5 WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) will automatically redirect all non-secured requested to SSL. 
    
      ``` c#
          app.UseHttpsRedirection();
      ```

3. **Verify URIs point to Https**
    * By default, all Uris in the solution will point to Https endpoints. If these have changed but you wish to re-enable Https, the following locations should be checked:

      * In the [Angular CLI](https://cli.angular.io/) webapp, go to src -> app -> `app.component.ts`. At line approximately 21, change the `issuer` to the Https endpoint:

        ``` ts
          authConfig: AuthConfig = {
            issuer: this.globalsService.IdentityServer_Https_URI,
            redirectUri: window.location.origin,
            clientId: this.globalsService.Client_Id,
            scope: this.globalsService.Client_Scopes,
            postLogoutRedirectUri: this.globalsService.WebApp_Post_Logout_Redirect_URI
          }
        ```

      * In the [Angular CLI](https://cli.angular.io/) webapp, go to src -> app -> `app.component.ts`. At line approximately 18, change the webapi_http endpoint to Https:

        ``` ts
          requestWeatherForecast() {
            this.httpClient.get<string>(`${this.globalsService.WebAPI_Https_URI}/WeatherForecast`)
              .subscribe((response: string) => {
                this.$apiSubject.next(JSON.stringify(response));
              });
          }
        ```

      * In ResourceServer **Startup.cs** `ConfigureServices()`, change `options.Authority = globals.IDENTITYSERVER_HTTPS_URI`.

        ``` c#
          .AddJwtBearer("Bearer", options =>
          {
              options.Authority = globals.IDENTITYSERVER_HTTPS_URI;
              options.RequireHttpsMetadata = false;
              options.Audience = globals.API_RESOURCE_NAME;
          });
        ```

---
## **Configure for Http**
1. **Disable Signing Certificate**
    * [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) requires a TLS / SSL connection - however, you can terminate the SSL connection before the [Asp.NET app](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-5.0) and pass requests from a reverse proxy to an unsecured port bound to the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server.
    
    > In production, you will still need to include a .pfx SSL certificate with private key called `cert.pfx` in the wwwroot folder. You can remove this requirement by commenting / removing the following lines from [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server **Startup.cs** in `ConfigureServices()` (approximately lines 90 - 107):

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

2. **Change Uris to Http**
    * By default, all Uris in the solution will point to Https endpoints. If you wish to change to Http, the following locations should be checked:

      * In the [Angular CLI](https://cli.angular.io/) webapp, go to src -> app -> `app.component.ts. At line approximately 21, change the `issuer` to the Http endpoint:

        ``` ts
          authConfig: AuthConfig = {
            issuer: this.globalsService.IdentityServer_Http_URI,
            redirectUri: window.location.origin,
            clientId: this.globalsService.Client_Id,
            scope: this.globalsService.Client_Scopes,
            postLogoutRedirectUri: this.globalsService.WebApp_Post_Logout_Redirect_URI
          }
        ```

      * In the [Angular CLI](https://cli.angular.io/) webapp, go to src -> app -> `app.component.ts`. At line approximately 18, change the webapi_https endpoint to Http:

        ``` ts
          requestWeatherForecast() {
            this.httpClient.get<string>(`${this.globalsService.WebAPI_Http_URI}/WeatherForecast`)
              .subscribe((response: string) => {
                this.$apiSubject.next(JSON.stringify(response));
              });
          }
        ```

      * In ResourceServer **Startup.cs** `ConfigureServices()`, change `options.Authority = globals.IDENTITYSERVER_HTTP_URI`.

        ``` c#
          .AddJwtBearer("Bearer", options =>
          {
              options.Authority = globals.IDENTITYSERVER_HTTP_URI;
              options.RequireHttpsMetadata = false;
              options.Audience = globals.API_RESOURCE_NAME;
          });
        ```

3. **FOR DEVELOPMENT USING HTTP**
    * In the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server **Startup.cs** `Configure()`, uncomment (approximately line 167) `app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict })`. This is required to store unsecured cookies and redirect correctly from the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server after a successful login on Http. 
  
    > **In production, SSL is required (but can be terminated before the [Asp.NET app](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-5.0)) and Cookies will always be secured. This line should be commented out / removed for production.**
  
      ``` c#
          // app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
      ```
---
## **Additional Settings**
### **Reference Tokens**
  * If you would like to use `reference tokens` instead of `bearer tokens`, you will need to change the `client.AccessTokenType` and add an `ApiSecret` to the `ApiResource` in the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server **Config.cs**.
  * This boilerplate includes commented out code that can be used to change to `reference tokens`. The commented code sections are:
    * [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server **Config.cs** `GetClients()`, `AccessTokenType` needs to be changed to `AccessTokenType.Reference`.

      ``` c#
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
                AccessTokenType = AccessTokenType.Reference
            },
        };
      ```

    * [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server **Config.cs** `GetApiResources()`, the ApiResource `ApiSecrets` needs to be un-commented.

      ``` c#
        return new List<ApiResource>
        {
            new ApiResource(globals.API_RESOURCE_NAME, "ResourceServer.WebAPI")
            {
                // No secrets for Implicit Flow
                ApiSecrets =
                {
                    new Secret(globals.API_RESOURCE_SECRET.Sha256())
                },
                Scopes = globals.WEBAPI_REQUESTED_SCOPES.ToList()
            }
        };
      ```

    * ResourceServer **Startup.cs** `ConfigureServers()`, `AddOauth2Introspection()` needs to be un-commented. If the Resource Server is only going to support `reference tokens`, `AddJwtBearer()` can also be be removed.

      ``` c#
        // This is for access tokens
        services.AddAuthentication("Bearer")
        //.AddJwtBearer("Bearer", options =>
        //{
        //    options.Authority = globals.IDENTITYSERVER_HTTPS_URI;
        //    options.RequireHttpsMetadata = false;
        //    options.Audience = globals.API_RESOURCE_NAME;
        //});
        // This is for reference tokens
          .AddOAuth2Introspection("token", options =>
          {
              options.Authority = Globals.IDENTITYSERVER_URI;

              // this maps to the API resource name and secret
              options.ClientId = Globals.CLIENT_ID;
              options.ClientSecret = Globals.API_RESOURCE_SECRET;
          });
      ```

  * More information regarding `reference tokens` can be found at the [IdentityServer4 reference documentation](https://identityserver4.readthedocs.io/en/latest/topics/reference_tokens.html). 

### **Auto-Redirect After Logout**
If you would like the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server to automatically redirect the user back to a specified (and authorized) post logout redirect uri after a successful logout, in the [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) server **Startup.cs** `ConfigureServices()`, uncomment the line specifying `AuthRedirectAfterSignOut = true` (approximately line 57):

  ``` c#
    AccountOptions.AutomaticRedirectAfterSignOut = true;
  ```

---
## **Additional Info**
For additional information / documentation on how to use the included applications, please see the following links:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/en/latest/)
* [AngularCLI Tour of Heroes Example Project](https://angular.io/tutorial)
* [Asp.NET 5 WebAPI Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio)
* [OAuth 2.0 Protocol](https://tools.ietf.org/html/rfc6749)
* [OIDC Protocol](https://openid.net/specs/openid-connect-core-1_0.html)

---
[IdentityServer4-Angular-.NET-5-WebAPI-Boilerplate](https://www.github.com/liveordevtrying/IdentityServer4-Angular-.NET-5-WebAPI-Boilerplate) was created by [LiveOrDevTrying](https://www.liveordevtrying.com) and is maintained by [Pixel Horror Studios](https://www.pixelhorrorstudios.com). 

![Pixel Horror Studios Logo](https://pixelhorrorstudios.s3-us-west-2.amazonaws.com/Packages/PHS.png)
