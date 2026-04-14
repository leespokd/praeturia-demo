using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using praetura_demo.Data;
using praetura_demo.Middlewares;
using praetura_demo.Profiles;
using praetura_demo.Repositories;
using praetura_demo.Repositories.Interfaces;
using praetura_demo.Services;
using praetura_demo.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILoanApplicationsRepository, LoanApplicationsRepository>();
builder.Services.AddScoped<IDecisionLogEntryRepository, DecisionLogEntryRepository>();

builder.Services.AddScoped<ILoanApplicationsService, LoanApplicationsService>();

builder.Services.AddSingleton<ILoanEligibilityService, LoanEligibilityService>();
builder.Services.AddScoped<ILoanProcessingService, LoanProcessingService>();
builder.Services.AddHostedService<LoanAssessmentBackgroundService>();

builder.Services.AddAutoMapper(cfg => { }, typeof(LoanApplicationProfile));

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddMvc();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(setup =>
    {
        //customise the problem details
        setup.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            var problemDetailsFactory = context.HttpContext.RequestServices
            .GetRequiredService<ProblemDetailsFactory>();

            var correlationId =
                context.HttpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                ?? context.HttpContext.TraceIdentifier;

            logger.LogWarning(
                "Validation failed for {Method} {Path}. CorrelationId: {CorrelationId}. Errors: {@Errors}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path,
                correlationId,
                context.ModelState);

            var validationProblemDetails = problemDetailsFactory
            .CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState);

            validationProblemDetails.Detail = "See the errors field for details.";
            //this will help to identify which endpoint caused the error
            validationProblemDetails.Instance = context.HttpContext.Request.Path;
            //more specific than error 400
            validationProblemDetails.Status = StatusCodes.Status400BadRequest;
            validationProblemDetails.Title = "One or more validation errors occurred.";
            validationProblemDetails.Extensions["correlationId"] = correlationId;
            validationProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            return new UnprocessableEntityObjectResult(validationProblemDetails)
            {
                ContentTypes = { "application/problem+json", "application/problem+xml" }
            };
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
