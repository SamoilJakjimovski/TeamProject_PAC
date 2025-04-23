
using domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetAdoptionCenter.Domain.DomainEntities;
using PetAdoptionCenter.Repository.implementation;
using PetAdoptionCenter.Repository.Interface;
using PetAdoptionCenter.Service.implementation;
using PetAdoptionCenter.Service.Interface;
using repository;
using repository.implementation;
using repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<PetAdoptionCenterUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IPetRepository), typeof(PetRepository));
builder.Services.AddScoped(typeof(IAdoptionApplicationRepository), typeof(AdoptionApplicationRepository));
builder.Services.AddScoped(typeof(IAgencyRepository), typeof(AgencyRepository));
builder.Services.AddScoped(typeof(ITravelPackageRepository), typeof(TravelPackageRepository));


builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


builder.Services.AddTransient<IPetService, PetService>();
builder.Services.AddTransient<IAdoptionApplicationService, AdoptionApplicationService>();
builder.Services.AddTransient<IAgencyService, AgencyService>();
builder.Services.AddTransient<ITravelPackageService, TravelPackageService>();



builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await PetAdoptionCenter.Web.RoleSeeder.SeedRoles(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
