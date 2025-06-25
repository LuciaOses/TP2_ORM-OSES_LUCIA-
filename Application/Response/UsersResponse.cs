using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class UsersResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public GenericResponse? Role { get; set; }
    }
}
