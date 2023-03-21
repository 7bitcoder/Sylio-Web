using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.IntegrationTests;
internal class FakeCurrentUserService : ICurrentUserService
{
    public string? UserId { get; set; }

    public string? UserName { get; set; }
}
