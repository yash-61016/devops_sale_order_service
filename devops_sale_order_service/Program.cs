using AutoMapper;
using devops_sale_order_service;
using devops_sale_order_service.Data;
using devops_sale_order_service.Endpoints;
using devops_sale_order_service.Filters;
using devops_sale_order_service.Models.Dto.Create;
using devops_sale_order_service.Repository;
using devops_sale_order_service.Repository.IRepository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISaleOrderRepository, SaleOrderRepository>();
builder.Services.AddScoped<ISaleOrderLineRepository, SaleOrderLineRepository>();

var connectionString = builder.Configuration.GetConnectionString("SaleOrderDbConnectionString");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// add dependency injection for validators for the sale order endpoints
builder.Services.AddScoped<IValidator<SaleOrderCreateDto>, CreateSaleOrderValidator<SaleOrderCreateDto>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

ApplyMigration();
app.ConfigureSaleOrderEndpoints();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}