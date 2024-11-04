using Bookify.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookify.Application.Users.LoginUser
{
    public sealed record LogInUserCommand(String Email, String Password): ICommand<AccessTokenResponse>;
    
}
