using Application.Interfaces.IApprovalStatus;
using Application.Interfaces.IArea;
using Application.Interfaces.IProjectType;
using Application.Interfaces.IRole;
using Application.Interfaces.IUser;
using Application.UseCases;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Infrastructure.Query;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["ConnectionString"];

builder.Services.AddDbContext<AprobacionDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAreaQuery, AreaQuery>();
builder.Services.AddScoped<IAreaCommands, AreaCommands>();
builder.Services.AddScoped<IAreaService, AreaService>();

builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IProjectTypeQuery, ProjectTypeQuery>();
builder.Services.AddScoped<IProjectTypeService, ProjectTypeService>();

builder.Services.AddScoped<IRoleQuery, RoleQuery>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IApprovalStatusQuery, ApprovalStatusQuery>();
builder.Services.AddScoped<IApprovalStatusService, ApprovalStatusService>();


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
