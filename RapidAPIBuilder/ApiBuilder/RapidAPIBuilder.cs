using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RapidAPIBuilder.Models;
using RapidAPIBuilder.Models.Entties;

namespace RapidAPIBuilder.ApiBuilder;

public class RapidAPIBuilder<TEntity> where TEntity : class, IEntity
{
    private readonly RouteGroupBuilder _group;

    public RapidAPIBuilder(IEndpointRouteBuilder app)
    {
        _group = app.MapGroup($"/{typeof(TEntity).Name.ToLower()}");
    }

    public static RapidAPIBuilder<TEntity> Create(IEndpointRouteBuilder app)
    {
        return new RapidAPIBuilder<TEntity>(app);
    }

    public RapidAPIBuilder<TEntity> WithCreateApi<TRequest, TResponse, TValidator>() 
        where TValidator : AbstractValidator<TRequest>, new()
    {
        _group.MapPost("/", 
            async (TRequest requestDto, ApplicationDbContext _context, IMapper _mapper) =>
            {
                var validator = new TValidator();
                var validationResult = await validator.ValidateAsync(requestDto);

                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors.Select(_ => new
                    {
                        PropertyName = _.PropertyName,
                        ErrorMessage = _.ErrorMessage,
                    }));

                var entity = _mapper.Map<TEntity>(requestDto);
                _context.Add(entity);

                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<TResponse>(entity);

                return Results.Ok(responseDto);
            });

        return this;
    }

    public RapidAPIBuilder<TEntity> WithGetByIdApi<TGetDetailReponse>()
    {
        _group.MapGet("/{id}",
            async (Guid id, ApplicationDbContext _context, IMapper _mapper) =>
            {
                var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(_ => _.Id == id);

                if (entity is null)
                    return Results.NotFound();

                var responseDto = _mapper.Map<TGetDetailReponse>(entity);

                return Results.Ok(responseDto);
            });
        
        return this;
    }

    public RapidAPIBuilder<TEntity> WithGetAllApi<TGetAllResponse>()
    {
        _group.MapGet("/",
            async (ApplicationDbContext _context, IMapper _mapper) =>
            {
                var entities = await _context.Set<TEntity>().ToListAsync();

                if (entities.Count == 0)
                    return Results.NoContent();

                var responseDtos = _mapper.Map<List<TGetAllResponse>>(entities);

                return Results.Ok(responseDtos);
            });

        return this;
    }

    public RapidAPIBuilder<TEntity> WithDeleteApi()
    {
        _group.MapDelete("/{id}",
            async (Guid id, ApplicationDbContext _context) =>
            {
                var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(_ => _.Id == id);

                if (entity is null)
                    return Results.NotFound();

                if (entity is IDeletable deletableEntity)
                {
                    if (deletableEntity.IsDeleted)
                        return Results.NotFound();

                    deletableEntity.IsDeleted = true;

                    await _context.SaveChangesAsync();
                }

                return Results.NoContent();
            });
        
        return this;
    }

    public RapidAPIBuilder<TEntity> WithUpdateApi<TUpdateRequest>()
    {
        _group.MapPut("/{id}",
            async (Guid id, TUpdateRequest updateRequest, ApplicationDbContext _context, IMapper _mapper) =>
            {
                var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(_ => _.Id == id);

                if (entity is null)
                    return Results.NotFound();

                if (entity is IDeletable deletableEntity && deletableEntity.IsDeleted)
                    return Results.NotFound();

                _mapper.Map(source: updateRequest, destination: entity);

                await _context.SaveChangesAsync();

                return Results.Ok("Updated!");
            });

        return this;
    }
}
