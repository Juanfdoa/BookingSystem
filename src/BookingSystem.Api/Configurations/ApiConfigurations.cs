using BookingSystem.Api.Filters;
using BookingSystem.Application.Validator.Auth;
using FluentValidation;

namespace BookingSystem.Api.Configurations
{
    public static class ApiConfigurations
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            //Validation Filter
            services.AddScoped<ValidationFilter>();
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            //Fluent Validation
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

            //Routing
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });


            return services;
        }
    }
}
