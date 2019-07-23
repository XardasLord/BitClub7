using System;
using MediatR;

namespace BC7.Business.Implementation.Users.Commands.ChangeAvatar
{
    public class ChangeAvatarCommand : IRequest
    {
        public Guid UserAccountDataId { get; set; }
        public string AvatarPath { get; set; }
    }
}