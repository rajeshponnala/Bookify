using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.Users
{
    public sealed class User : Entity
    {
        private User(Guid id, FirstName firstName, LastName lastName, Email email): base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }

        public Email Email { get; private set; }
        public string IdentityId { get; private set; } = String.Empty;

        public static User Create(FirstName firstName, LastName lastName, Email email)
        {
            var user = new User(Guid.NewGuid(), firstName, lastName, email);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            return user;
        }

        public void SetIdentity(string identityId)
        {
            IdentityId = identityId;
        }
    }
}
