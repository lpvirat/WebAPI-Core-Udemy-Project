using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Fluent validation service
builder.Services.AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Program>());

//Injecting the DBContext class in service collection
builder.Services.AddDbContext<NZWalksDbContext>
    ( options => { options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks")); } );

//Dependency Injection for Region,Walk,Walk Difficulty
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

//Injecting AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

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
