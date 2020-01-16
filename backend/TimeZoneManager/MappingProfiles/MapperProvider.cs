using AutoMapper;
using System.Collections.Generic;
using System.Reflection;

namespace TimeZoneManager.MappingProfiles
{
    public interface IMapperProvider
    {
        ICollection<Assembly> Assemblies { get; }
    }

    public class MapperProvider : IMapperProvider
    {
        public ICollection<Assembly> Assemblies { get; }

        public MapperProvider()
        {
            Assemblies = new List<Assembly> { GetType().Assembly };
        }

        public MapperConfiguration CreateConfiguration()
        {
            return new MapperConfiguration(conf => conf.AddMaps(Assemblies));
        }

        public Mapper GetMapper()
        {
            return new Mapper(CreateConfiguration());
        }
    }
}
