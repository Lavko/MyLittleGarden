using Application.ActionControl;
using Application.Initializers;
using Application.Scheduler;
using Domain.Configurations;
using Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Application;

public static class ConfigureApplication
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new ScheduledServicesConfiguration();
        configuration.GetSection(ScheduledServicesConfiguration.SectionName).Bind(config);
        
        services.AddScoped<IActionRuleProcessor, ActionRuleProcessor>();
        services.AddScoped<IActionControlService, ActionControlService>();
        services.AddScoped<ISchedulerService, SchedulerService>();
        
        services.AddScoped<OutletInitializer>();

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 10;
            });
            
            q.AddJob<MeasuresJob>(opts => opts.WithIdentity(nameof(MeasuresJob)));
    
            q.AddTrigger(opts => opts
                .ForJob(nameof(MeasuresJob))
                .WithIdentity(nameof(MeasuresJob))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(config.IntervalInSeconds)
                    .RepeatForever())
            );
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