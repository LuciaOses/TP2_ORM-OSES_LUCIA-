using Application.Interfaces;
using Application.Interfaces.IApprovalStatus;
using Application.Interfaces.IArea;
using Application.Interfaces.IProjectProporsal;
using Application.Interfaces.IProjectType;
using Application.Interfaces.IRole;
using Application.Interfaces.IUser;
using Application.Interfaces.IValidator;
using Application.UseCases;
using Infraestructura.Validations;
using Infrastructure.Query;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    c.IncludeXmlComments(xmlPath);
});

var connectionString = builder.Configuration["ConnectionString"];

builder.Services.AddDbContext<AprobacionDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAreaQuery, AreaQuery>();
builder.Services.AddScoped<IAreaCommands, AreaCommands>();
builder.Services.AddScoped<IAreaService, AreaService>();

builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCommand, UserCommand>();

builder.Services.AddScoped<IProjectTypeQuery, ProjectTypeQuery>();
builder.Services.AddScoped<IProjectTypeService, ProjectTypeService>();

builder.Services.AddScoped<IRoleQuery, RoleQuery>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IApprovalStatusQuery, ApprovalStatusQuery>();
builder.Services.AddScoped<IApprovalStatusService, ApprovalStatusService>();

builder.Services.AddScoped<IProjectProposalService, ProjectProposalService>();
builder.Services.AddScoped<IProjectProposalCommand, ProjectProposalCommand>();
builder.Services.AddScoped<GetProjectById>();
builder.Services.AddScoped<UpdateProjectProposal>();

builder.Services.AddScoped<IApprovalRuleQuery, ApprovalRuleQuery>();
builder.Services.AddScoped<IProjectApprovalStepCommand, ProjectApprovalStepCommand>();
builder.Services.AddScoped<ApprovalService>();


builder.Services.AddScoped<IDatabaseValidator, DatabaseValidator>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend"); 

app.UseAuthorization();

app.MapControllers();

app.Run();