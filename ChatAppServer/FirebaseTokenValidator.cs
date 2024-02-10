using ChatAppServer.Services;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

internal class FirebaseTokenValidator : AuthenticationHandler<AuthenticationSchemeOptions>
{

   

    public FirebaseTokenValidator(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

  

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var headers = Context.Request.Headers;

        if (!headers.TryGetValue("Authorization", out var authHeader))
        {
            return AuthenticateResult.Fail("Authorization header is missing.");
        }
        string bearerToken = Context.Request.Headers["Authorization"];
        if (bearerToken == null || !bearerToken.StartsWith("Bearer "))
        {
            return AuthenticateResult.Fail("Authorization header is missing.");

        }
        string token = bearerToken.Substring("Bearer ".Length);
        var fbToken = await FireBaseAdminService.Instance.VerifyIdTokenAsync(token);
        if (fbToken == null) return AuthenticateResult.Fail("Access deny -> Fireabse user token is missing or incorrect");

        try
        {
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new List<ClaimsIdentity>() {

         new ClaimsIdentity(ToClaims(fbToken),nameof(FirebaseTokenValidator))
        }), JwtBearerDefaults.AuthenticationScheme));
        }
        catch (Exception e)
        {

            return AuthenticateResult.Fail($"{e.Message}");
        }
    }

    private IEnumerable<Claim>? ToClaims(FirebaseToken claims)
    {
        string? userId = claims.Claims["user_id"]?.ToString();
        string? email = claims.Claims["email"]?.ToString();
        List<Claim> climesData = new List<Claim> {
          new Claim("user_id",userId),
          new Claim("email",email)
        };
       
        return climesData;
    }
}