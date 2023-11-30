﻿using HC.Foundation.Data.Base;
using HC.Service.Authentication.Data;
using HC.Service.Authentication.Entities;
using HC.Service.Authentication.Repositories.IRepositories;

namespace HC.Service.Authentication.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthenticationDbContext context) : base(context)
        {
        }
    }
}