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
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationDbContext _context;

    public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<User> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<AuthResponse> RegisterUserAsync(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            CityId = request.CityId,
            Status = request.Status,
            Role = request.Role
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new Exception("Registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        // Ensure role exists
        if (!await _roleManager.RoleExistsAsync(user.Role))
            await _roleManager.CreateAsync(new IdentityRole(user.Role));

        await _userManager.AddToRoleAsync(user, user.Role);

        // Auto sign-in user after registration
        await _signInManager.SignInAsync(user, isPersistent: true);

        return new AuthResponse
        {
            User = new { user.Id, user.UserName, user.Email, user.Role },
            Message = "Registration successful"
        };
    }


    public async Task<AuthResponse> LoginUserAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("User not found with email: " + request.Email);

        var result = await _signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: true, // true → persistent cookie
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
            throw new Exception("Invalid login credentials.");

        // ✅ Cookie is automatically issued
        return new AuthResponse
        {
            User = new { user.Id, user.UserName, user.Email, user.Role },
            Message = "Login successful"
        };
    }

    public async Task LogoutUserAsync()
    {
        await _signInManager.SignOutAsync(); // Deletes authentication cookie
    }






}