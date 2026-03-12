using WebApplicationProject.Data;
using Microsoft.EntityFrameworkCore;

public class EventCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public EventCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var eventsToClose = await context.Events
                    .Where(e => !e.IsRegistrationClosed && DateTime.Now >= e.DateTime.AddMinutes(-2))
                    .ToListAsync();

                if (eventsToClose.Any())
                {
                    foreach (var ev in eventsToClose)
                    {
                        ev.IsRegistrationClosed = true; 
                    }
                    await context.SaveChangesAsync();
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}