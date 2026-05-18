using DAL.EF;
using DAL.Repos;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// DAL Repositories
builder.Services.AddScoped<FeatureRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<PlanRepository>();
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<SubscriptionRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserRoleRepository>();

// DAL Factory + BLL Services
builder.Services.AddScoped<DAL.DataAccessFactory>();
builder.Services.AddScoped<BLL.Services.UserService>();
builder.Services.AddScoped<BLL.Services.PlanService>();
builder.Services.AddScoped<BLL.Services.FeatureService>();
builder.Services.AddScoped<BLL.Services.PaymentService>();
builder.Services.AddScoped<BLL.Services.RoleService>();
builder.Services.AddScoped<BLL.Services.SubscriptionService>();
builder.Services.AddScoped<BLL.Services.UserRoleService>();

builder.Services.AddDbContext<SmartSubcriptionsContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConn"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();