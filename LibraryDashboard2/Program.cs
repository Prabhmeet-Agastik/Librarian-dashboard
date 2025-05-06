using LibraryDashboard2.Data;
using LibraryDashboard2.Areas.Student.Data;
using LibraryDashboard2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext for main and student areas
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDbContext<LibraryDashboard2.Areas.Student.Data.StudentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//JWT Authentication Setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
                                            {
                                                ValidateIssuer = true,
                                                ValidateAudience = true,
                                                ValidateLifetime = true,
                                                ValidIssuer = "LibraryApp",
                                                ValidAudience = "LibraryApp",
                                                ValidateIssuerSigningKey = true,
                                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Z3ZfK8d!xR#9uBt4$P2wL6m@E1qT7nJz\r\n"))
                                            };

options.Events = new JwtBearerEvents
{
    OnMessageReceived = context =>
    {
        context.Token = context.Request.Cookies["jwt"];
        return Task.CompletedTask;
    }
};

    });

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login";
    });

builder.Services.AddAuthorization();


//Authorization Policies for Roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LibrarianOnly", policy => policy.RequireRole("Librarian"));
    options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Set up routing for areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

// Default route for login page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

// Root route for login
app.MapControllerRoute(
    name: "root",
    pattern: "/",
    defaults: new { controller = "Account", action = "Login" }
);

app.Run();
