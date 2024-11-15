using DashboardAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MultilingualDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5173") // Replace with your frontend's URL
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Remove the IsDevelopment() condition to always show Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    // Set Swagger as the default page
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

// Apply the CORS policy before routing
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();

app.Run();