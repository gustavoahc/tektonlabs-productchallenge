using AutoMapper;
using TektonLabs.Core.Application;
using TektonLabs.Infrastructure.DataAccess;
using TektonLabs.Presentation.Api.Helpers.Mapping;

var builder = WebApplication.CreateBuilder(args);
{
    // Add mapping configuration
    var mappingConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MappingConfiguration());
    });
    IMapper mapper = mappingConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);

    //Add services
    builder.Services
        .AddDataAccessServices(builder.Configuration)
        .AddApplicationServices();

    //Cache
    builder.Services.AddLazyCache();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}