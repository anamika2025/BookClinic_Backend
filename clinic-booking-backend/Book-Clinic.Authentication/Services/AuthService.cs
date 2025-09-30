using Book_Clinic.Authentication.DTOs;
using Book_Clinic.Authentication.Utilities;
using Book_Clinic.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Book_Clinic.Authentication.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<MstUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(UserManager<MstUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, JwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> RegisterUserAsync(RegisterRequest request)
    {
        var user = new MstUser
        {
            UserId = request.UserId,
            UserName = request.UserName,
            Email = request.Email,
            CityId = request.CityId,
            Status = request.Status,
            Role = request.Role
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new System.Exception("Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync(user.Role))
            await _roleManager.CreateAsync(new IdentityRole(user.Role));

        await _userManager.AddToRoleAsync(user, user.Role);
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponse { Token = token };
    }

    public async Task<AuthResponse> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new System.Exception("Invalid credentials");

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponse { Token = token };
    }




}
