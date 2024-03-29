using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;
using SIEL_1836109025062022.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(mySQLConfiguration);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICredentialsRepository, Credentials>();
builder.Services.AddTransient<ICourseProgramRepository, CourseProgramRepository>();
builder.Services.AddTransient<ILevelsRepository, LevelsRepository>();
builder.Services.AddTransient<IModalityRepository, ModalityRepository>();
builder.Services.AddTransient<IScheduleRepository, ScheduleRepository>();
builder.Services.AddTransient<IStudentsRepository, StudentsRepository>();
builder.Services.AddTransient<ITeachersRepository, TeachersRepository>();
builder.Services.AddTransient<IClassesRepository, ClassesRepository>();
builder.Services.AddTransient<IInscriptionRepository, InscriptionRepository>();
builder.Services.AddTransient<IAccountantRepository, AccountantRepository>();
builder.Services.AddTransient<IStatusRepository, StatusIncriptionRepostitory>();
builder.Services.AddTransient<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddTransient<IListRepository, ListRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<SignInManager<User>>();
builder.Services.AddIdentityCore<User>(opciones =>
{
    opciones.Password.RequireDigit = false;
    opciones.Password.RequireLowercase = false;
    opciones.Password.RequireUppercase = false;
    opciones.Password.RequireNonAlphanumeric = false;
    opciones.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
}).AddErrorDescriber<ErrorMessagesIdentity>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath,"../Rotativa/Windows");


app.Run();
