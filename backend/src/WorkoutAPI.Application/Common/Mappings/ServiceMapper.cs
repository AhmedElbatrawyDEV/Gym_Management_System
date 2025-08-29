using Mapster;
using WorkoutAPI.Application.Common.Interfaces;

namespace WorkoutAPI.Application.Common.Mappings;
public class ServiceMapper : IMapper
{
    private readonly TypeAdapterConfig _config;

    public ServiceMapper(TypeAdapterConfig config)
    {
        _config = config;
    }

    public TDestination Map<TDestination>(object source)
    {
        return source.Adapt<TDestination>(_config);
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TDestination>(_config);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return source.Adapt(destination, _config);
    }
}

