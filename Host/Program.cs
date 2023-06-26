using Application;
using Application.Scheduler;
using Domain.Configurations;
using Infrastructure;
using Infrastructure.Persistence.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .RegisterPersistence(builder.Configuration)
    .RegisterHardware(builder.Environment.IsDevelopment())
    .RegisterApplicationServices(builder.Configuration)
    .RegisterConfiguration(builder.Configuration)
    .RegisterDomain();

builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureApplication).Assembly)
);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngularDevClient",
        b =>
        {
            b.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        }
    );
    options.AddPolicy(
        "AllowAngularProdClient",
        b =>
        {
            b.WithOrigins("https://mypi.local").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAngularDevClient");
}
else
{
    app.UseCors("AllowAngularProdClient");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<AppDbContextInitialiser>();
    await db.InitialiseAsync();
    await db.SeedAsync();

    var schedulerService = services.GetRequiredService<ISchedulerService>();
    await schedulerService.InitializeSavedJobs();
}

app.Run();
