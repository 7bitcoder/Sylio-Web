using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Games.Sheduler;
internal class ServerCurrentUser : ICurrentUserService
{
    public string? UserId => "Server";

    public string? UserName => "Server";
}
