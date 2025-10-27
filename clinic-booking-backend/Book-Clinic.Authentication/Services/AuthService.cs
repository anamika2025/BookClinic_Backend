using Book_Clinic.Authentication.DTOs;
using Book_Clinic.Authentication.Utilities;
using Book_Clinic.Data;
using Book_Clinic.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Book_Clinic.Authentication.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly ApplicationDbContext _context;

    public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, JwtTokenGenerator jwtTokenGenerator, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
    }

    public async Task<AuthResponse> RegisterUserAsync(RegisterRequest request)
    {
        var user = new User
        {
            //UserId = request.UserId,
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
        var token = await  _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponse
        {
            Token = token,
            User = new { user.Id, user.UserName, user.Email, user.Role }
        };
    }

    public async Task<AuthResponse> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("User not found with email: " + request.Email);

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            throw new Exception("Password incorrect for user: " + request.Email);

        var token = await _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponse
        {
            Token = token,
            User = new { user.Id, user.UserName, user.Email, user.Role }
        };
    }


    public Task<string> GetJwtKeyFromDbAsync()
    {
        var key = _context.JwtKeys
           .Where(s => s.KeyName == "JwtKey")
        .Select(s => s.Value)
        .FirstOrDefaultAsync();

        return key ?? throw new Exception("JWT key not found in database.");
    }






}