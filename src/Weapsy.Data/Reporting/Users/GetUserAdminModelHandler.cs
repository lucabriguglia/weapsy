﻿using AutoMapper;
using Weapsy.Infrastructure.Queries;
using System.Threading.Tasks;
using Weapsy.Reporting.Users.Queries;
using Weapsy.Reporting.Users;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Users;
using System.Linq;
using System.Collections.Generic;

namespace Weapsy.Data.Reporting.Users
{
    public class GetUserAdminModelHandler : IQueryHandlerAsync<GetUserAdminModel, UserAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetUserAdminModelHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<UserAdminModel> RetrieveAsync(GetUserAdminModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.Users
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Id == query.Id && x.Status != UserStatus.Deleted);

                if (entity == null)
                    return null;

                var model = _mapper.Map<UserAdminModel>(entity);

                var allRoles = await context.Roles.ToListAsync();

                model.AllRoles = allRoles.Select(x => x.Name);

                var userRoleNames = new List<string>();
                foreach (var role in entity.Roles)
                {
                    var userRole = allRoles.FirstOrDefault(x => x.Id == role.RoleId);
                    if (userRole != null)
                    {
                        userRoleNames.Add(userRole.Name);
                    }
                }
                model.UserRoles = userRoleNames;

                return model;
            }
        }
    }
}