using Application.Interfaces.IRole;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class RoleService(IRoleQuery query) : IRoleService
    {
        private readonly IRoleQuery _query = query;

        public async Task<List<ApproverRole>> GetAllAsync()
        {
            return await _query.GetAllRoles();
        }
    }
}
