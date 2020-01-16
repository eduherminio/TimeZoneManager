using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TimeZoneManager.MappingProfiles;
using Xunit;

namespace TimeZoneManager.Test
{
    public class AutoMapperConfigurationValidation
    {
        [Fact]
        public void ValidateConfiguration()
        {
            IMapper mapper = new MapperProvider().GetMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
