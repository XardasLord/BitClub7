﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BC7.Domain;
using BC7.Infrastructure.CustomExceptions;
using BC7.Repository;
using BC7.Security;
using MediatR;

namespace BC7.Business.Implementation.Articles.Commands
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand>
    {
        private readonly IUserAccountDataRepository _userAccountDataRepository;
        private readonly IArticleRepository _articleRepository;

        public CreateArticleCommandHandler(IUserAccountDataRepository userAccountDataRepository, IArticleRepository articleRepository)
        {
            _userAccountDataRepository = userAccountDataRepository;
            _articleRepository = articleRepository;
        }

        public async Task<Unit> Handle(CreateArticleCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = await _userAccountDataRepository.GetAsync(command.UserAccountDataId);
            if (user.Role != UserRolesHelper.Root)
            {
                throw new ValidationException("Only root user can add an article");
            }

            var article = new Article(
                id: Guid.NewGuid(),
                creatorId: user.Id,
                title: command.Title,
                text: command.Text
            );

            await _articleRepository.CreateAsync(article);

            return Unit.Value;
        }
    }
}