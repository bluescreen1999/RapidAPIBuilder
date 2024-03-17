using RapidAPIBuilder.Models;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RapidAPIBuilder.ApiBuilder;
using RapidAPIBuilder.Models.Entties;
using RapidAPIBuilder.Models.Dtos.Tags;
using RapidAPIBuilder.Models.Validators.Tags;
using RapidAPIBuilder.Models.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("minimal"));
});

builder.Services
.AddFluentValidationAutoValidation()
.AddFluentValidationClientsideAdapters();

builder.Services.AddAutoMapper(typeof(TagMappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

RapidAPIBuilder<Tag>.Create(app)
    .WithCreateApi<CreateTagRequest, CreateTagResponse, CreateTagRequestValidator>()
    .WithGetByIdApi<GetTagDetailsResponse>()
    .WithDeleteApi()
    .WithGetAllApi<GetAllTagsResponse>()
    .WithUpdateApi<UpdateTagReqest>();

app.Run();