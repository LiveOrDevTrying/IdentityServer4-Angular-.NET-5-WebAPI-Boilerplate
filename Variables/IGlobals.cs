namespace Variables
{
    public interface IGlobals
    {
        string CLIENT_ID { get; set; }
        string CLIENT_URI { get; set; }
        string CLIENT_REDIRECT_URI { get; set; }
        string CLIENT_POST_LOGOUT_REDIRECT_URI { get; set; }
        string[] CLIENT_ALLOWED_CORS_ORIGINS { get; set; }
        
        string CERTIFICATE_PASSWORD { get; set; }
        
        string API_RESOURCE_NAME { get; set; }
        //string API_RESOURCE_SECRET { get; set; }
        string API_RESOURCE_SCOPE { get; set; }
        string[] WEBAPI_REQUESTED_SCOPES { get; set; }

        string CONNECTION_STRING { get; set; }

        string IDENTITYSERVER_HTTP_URI { get; set; }
        string IDENTITYSERVER_HTTPS_URI { get; set; }
        string WEBAPP_URI { get; set; }
        string WEBAPI_HTTP_URI { get; set; }
        string WEBAPI_HTTPS_URI { get; set; }


        string ADMIN_USERNAME { get; set; }
        string ADMIN_PASSWORD { get; set; }

        // These are Default Claims for the Admin User
        string ADMIN_USER_FULL_NAME { get; set; }
        string ADMIN_USER_GIVEN_NAME { get; set; }
        string ADMIN_USER_FAMILY_NAME { get; set; }
        string ADMIN_USER_EMAIL { get; set; }
        string ADMIN_USER_WEBSITE { get; set; }
        string ADMIN_ROLE { get; set; }
    }
}