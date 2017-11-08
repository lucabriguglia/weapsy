﻿using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;

namespace Weapsy.Tests.Factories
{
    public static class SiteFactory
    {
        public static Site CreateNew()
        {
            return CreateNew(Guid.NewGuid(), "My Site");
        }

        public static Site CreateNew(Guid id, string name)
        {
            var command = new CreateSiteCommand
            {
                Id = id,
                Name = name
            };

            var validatorMock = new Mock<IValidator<CreateSiteCommand>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            return Site.CreateNew(command, validatorMock.Object);
        }

        public static Site Update(this Site site, Guid homePageId)
        {
            var command = new UpdateSiteDetailsCommand
            {
                SiteId = site.Id,
                HomePageId = homePageId
            };

            var validatorMock = new Mock<IValidator<UpdateSiteDetailsCommand>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            site.UpdateDetails(command, validatorMock.Object);

            return site;
        }
    }
}
