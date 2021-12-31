using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using FluentValidation;
using BlazorGolfApi.Services;
using Microsoft.EntityFrameworkCore;
using BlazorGolfApi.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddScoped<IRepository<Course>, CourseRespository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEveryone",
        options =>
        {
            options.WithOrigins("*").
                    AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationContext>(options => {
                    options.EnableSensitiveDataLogging()
                    .UseCosmos(builder.Configuration.GetConnectionString("DefaultConnection"), nameof(ApplicationContext));

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

