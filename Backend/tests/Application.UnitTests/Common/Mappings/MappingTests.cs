using System.Runtime.Serialization;
using AutoMapper;
using Backend.Application.Common.Mappings;
using Backend.Application.Common.Models;
using Backend.Domain.Entities;
using NUnit.Framework;

namespace Backend.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
