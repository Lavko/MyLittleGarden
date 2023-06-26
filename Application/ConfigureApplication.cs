using Application.Scheduler;
using Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Application;

public static class ConfigureApplication
{
    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var config = new ScheduledServicesConfiguration();
        configuration.GetSection(ScheduledServicesConfiguration.SectionName).Bind(config);

        services.AddScoped<ISchedulerService, SchedulerService>();

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 10;
            });
        });

        // ASP.NET Core hosting
        services.AddQuartzServer(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
