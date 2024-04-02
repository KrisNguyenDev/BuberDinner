using BuberDinner.Api.Fillter;
using BuberDinner.Api.Middlware;
using BuberDinner.Application;
using BuberDinner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    //builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFillterAttribute>());
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    //app.UseMiddleware<ErrorHandlingMiddlware>();
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}

