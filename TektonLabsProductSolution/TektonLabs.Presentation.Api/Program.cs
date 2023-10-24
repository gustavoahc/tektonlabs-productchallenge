using AutoMapper;
using Microsoft.OpenApi.Models;
using NLog;
using System.Reflection;
using TektonLabs.Core.Application;
using TektonLabs.Infrastructure.DataAccess;
using TektonLabs.Presentation.Api.ApiModels.SettingParameters;
using TektonLabs.Presentation.Api.Helpers.ErrorHandling;
using TektonLabs.Presentation.Api.Helpers.Logging;
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

    //Load app parameters
    builder.Services.Configure<Parameter>(builder.Configuration.GetSection(Parameter.SectionName));

    //Add services
    builder.Services
        .AddDataAccessServices(builder.Configuration)
        .AddApplicationServices();

    //Cache
    builder.Services.AddLazyCache();

    //Logging
    LogManager.Setup().LoadConfigurationFromFile(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
    builder.Services.AddSingleton<ILogging, Logging>();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ErrorHandlingFilterAttribute>();
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen((options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Products API",
            Version = "v1",
            Description = "An ASP.NET Core Web API for managing products in Tekton Labs challenge"
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }));
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