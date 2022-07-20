using CancunHotel.Services;

namespace CancunHotel.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IGuestService, GuestService>();
            return services;
        }
    }
}
