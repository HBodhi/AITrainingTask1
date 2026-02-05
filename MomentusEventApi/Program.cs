using MomentusEventApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Ungerboeck API Client Factory
builder.Services.AddSingleton<IUngerboeckApiClientFactory, UngerboeckApiClientFactory>();

// Register Momentus Event Service
builder.Services.AddScoped<IMomentusEventService, MomentusEventService>();

// Register Service Order Service
builder.Services.AddScoped<IServiceOrderService, ServiceOrderService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();
