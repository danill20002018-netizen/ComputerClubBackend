
using AuthService.API.Controllers.Extensions;
using AuthService.Application.Services.Abstractions;
using AuthService.Application.Tools;
using AuthService.Domain.Requests.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IHttpCookieService _cookieService;

    public AuthController(
        IAuthService authService,
        IHttpCookieService cookieService)
    {
        _authService = authService;
        _cookieService = cookieService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest fromBody,  HttpContext httpContext,CancellationToken cancellationToken)
    {
        var userAgent =  httpContext.Request.Headers.UserAgent;
        var ipAddress = httpContext.GetClientIpAddress();
        //
        var result = await _authService.Login(new()
        {
            Login = fromBody.Login,
            Password = fromBody.Password,
            //
            UserAgent = userAgent,
            IpAddress = ipAddress!=null? ipAddress.ToString() : null,
            //
            HttpCookies=httpContext.Request.GetCookies()
        }, cancellationToken);
        _cookieService.Apply(httpContext.Response, result.Cookies);
        //
        return Ok(result.Data);
    }
    
    
    // public static IEndpointRouteBuilder UseAuthEndpoints(this IEndpointRouteBuilder app)
    // {
    //     var group = app.MapGroup("api/v1/auth");
    //  
    //     //login
    //     group.MapPost("/login", async ([FromBody] LoginUserRequest requestBody, HttpResponse response, HttpContext httpContext, IAuthService authService, HttpRequest httpRequest, IHttpCookieService cookieService, CancellationToken cancellationToken) =>
    //     {
    //         var userAgent =  httpRequest.Headers.UserAgent;
    //         var ipAddress = httpContext.GetClientIpAddress();
    //         //
    //         var result = await authService.Login(new()
    //         {
    //             Login = requestBody.Login,
    //             Password = requestBody.Password,
    //             //
    //             UserAgent = userAgent,
    //             IpAddress = ipAddress!=null? ipAddress.ToString() : null,
    //             //
    //             HttpCookies=httpRequest.GetCookies()
    //         }, cancellationToken);
    //         cookieService.Apply(response, result.Cookies);
    //
    //     });
    //     
    //     //register
    //     group.MapPost("/register", async ([FromBody] RegisterUserRequest requestBody, HttpContext httpContext, IAuthService authService, HttpRequest request, CancellationToken cancellationToken) =>
    //     {
    //         var userAgent =  request.Headers.UserAgent;
    //         var ipAddress = httpContext.GetClientIpAddress();
    //
    //         var result = await authService.Register(new() {
    //             Name = requestBody.UserName,
    //             Password = requestBody.Password,
    //             ConfirmPassword = requestBody.ConfirmPassword,
    //             Email = requestBody.Email,
    //             //
    //             UserAgent = userAgent,
    //             IpAddress = ipAddress!=null? ipAddress.ToString() :  null
    //             
    //         }, cancellationToken);
    //     });
    //     
    //     //refresh token
    //     group.MapPost("/refresh", async (HttpContext httpContext, IAuthService authService, HttpRequest httpRequest, CancellationToken cancellationToken) =>  
    //     {
    //         var userAgent =  httpRequest.Headers.UserAgent;
    //         var ipAddress = httpContext.GetClientIpAddress();
    //
    //         var result = await authService.RefreshToken(new() {
    //             UserAgent = userAgent,
    //             IpAddress = ipAddress!=null? ipAddress.ToString() :  null,
    //             //
    //             HttpCookies = httpRequest.GetCookies()
    //             
    //         }, cancellationToken);
    //     });
    //
    //     //logout for session with currently id
    //     group.MapDelete("/session/{id}", async ( [FromRoute] Guid id, HttpContext httpContext, IAuthService authService, HttpRequest httpRequest, CancellationToken cancellationToken) =>
    //     {
    //         var result= await authService.Logout(new() {
    //             SessionIdToDelete = id,
    //             //
    //             HttpCookies = httpRequest.GetCookies()
    //             
    //         }, cancellationToken);
    //     });
    //     
    //     //logout for session device from what endpoint calling
    //     group.MapDelete("/session", async ( [FromRoute] Guid id, HttpContext httpContext, IAuthService authService, HttpRequest httpRequest, CancellationToken cancellationToken) =>
    //     {
    //         var result = await authService.Logout(new() {
    //             HttpCookies = httpRequest.GetCookies()
    //             
    //         }, cancellationToken);
    //     });
    //     return app;
    //     
    // }
}