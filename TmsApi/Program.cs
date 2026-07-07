using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication("Training")
    .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>("Training", null);
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<EnrollmentWorker>();
builder.Services.AddSingleton<IEnrollmentService, EnrollmentService>();

builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/api/enrollments/worker-smoke", (EnrollmentWorker worker) =>
{
    worker.ProcessBatch();
    return Results.Ok("processed");
});

app.MapGet("/api/assessments/results", () => Results.Ok(new
{
    courseCode = "CS-101",
    studentId = "S-001",
    letterGrade = "A"
}))
.RequireAuthorization();

app.MapGet("/api/error", () =>
{
    throw new TmsDatabaseException("Simulated database failure for ProblemDetails testing");
});

app.MapGet("/api/error", () =>
{
    throw new TmsDatabaseException("Simulated database failure for ProblemDetails testing");
});

app.Run();