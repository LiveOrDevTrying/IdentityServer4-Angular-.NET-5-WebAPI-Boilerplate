namespace IdentityServer
{
    public static class Globals
    {
        public const string ADMIN_USERNAME = "admin";
        public const string ADMIN_PASSWORD = "Password1!";
        public const string ADMIN_ROLE = "admin"; // Roles are not required but included for convenience
        public const string SMS_EMAIL_SENDER_NAME = "admin@pixelhorrorstudios.com";

        public const string API_RESOURCE_SCOPE = "api.resource.scope";
        public const string API_RESOURCE_SECRET = "sdfkl3898432823klf;as;dfk!$%*@lsdfjmods;";  // This is for Introspection (if required)

        public const string CLIENT_ID = "AngularClient";
        public const string CLIENT_SECRET = ""; // Implicit clients have no secret, but included for sample reference token introspection check in webAPI
        public const string CLIENT_URI = "localhost:4200";
        public const string CLIENT_REDIRECT_URI = "http://localhost:4200";
        public const string CLIENT_POST_LOGOUT_REDIRECT_URI = "http://localhost:4200";
        public const string CLIENT_ALLOWED_CORS_ORIGIN = "http://localhost:4200";

        public const string CERTIFICATE_PASSWORD = ""; // Password for the cer or pfx certificate included in the wwwRoot folder for IdentityServer

        // Connection string for SQL server
        public const string CONNECTION_STRING_IDENTITY = "Server=;Database=;Trusted_Connection=false;User Id=;Password=;MultipleActiveResultSets=true";

        public const string IDENTITYSERVER_URI = "https://localhost:5001";
        public const string WEBAPP_URI = "http://localhost:4200";
        public const string WEBAPI_URI = "https://localhost:5002";
    }
}
