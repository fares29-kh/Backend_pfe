using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.Hubs;
using pfe.models;

var builder = WebApplication.CreateBuilder(args);


  ///////// important 
//pour creer la base de donnee il faut vreer une base donnee dans le ssms nommee
//sitewebpfe  et il faut fair ce 2 "commandes add-migration init1" &   "update-database" 



// Add services to the container.
builder.Services.AddDbContext<DBContext>(option => option.UseSqlServer("Server =DESKTOP-060J0DT; Database=sitewebpfe; Trusted_Connection=True; MultipleActiveResultSets=True")); builder.Services.AddIdentity<User, Role>(option =>
{
    //rules for password validation 
    option.Password.RequireDigit = true;
    option.Password.RequireUppercase = true; 
    option.Password.RequiredLength = 5;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireLowercase = true;
    option.Password.RequireNonAlphanumeric = false;
    option.SignIn.RequireConfirmedEmail = false;
    option.Lockout.MaxFailedAccessAttempts = 5;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
}).AddEntityFrameworkStores<DBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// allow cors for the front end url 
app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
.AllowCredentials());
app.UseRouting();
app.MapHub<ChatHub>("/chatHub");

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
