using Application.Interfaces;
using Domain.Entities;


namespace Application.UseCases
{
    public class UserService(IUserRepository repo)
    {
        private readonly IUserRepository _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public async Task ListarUsuarios()
        {
            var users = await _repo.GetAllAsync();
            foreach (var u in users)
            {
                Console.WriteLine($"ID: {u.Id}, Nombre: {u.Name}, Email: {u.Email}, Rol: {u.ApproverRole?.Name}");
            }
        }

        public async Task CrearUsuario()
        {
            Console.Write("Nombre: ");
            var name = Console.ReadLine();
            Console.Write("Email: ");
            var email = Console.ReadLine();
            Console.Write("ID Rol: ");
            var roleId = int.Parse(Console.ReadLine() ?? "0");

            var user = new User { Name = name!, Email = email!, Role = roleId };
            await _repo.AddAsync(user);
            Console.WriteLine("Usuario creado" + user.Name);
        }

        public async Task EditarUsuario()
        {
            Console.Write("ID del usuario: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                Console.WriteLine("Usuario no encontrado");
                return;
            }

            Console.Write("Nuevo Nombre: ");
            user.Name = Console.ReadLine()!;
            Console.Write("Nuevo Email: ");
            user.Email = Console.ReadLine()!;
            Console.Write("Nuevo Rol ID: ");
            user.Role = int.Parse(Console.ReadLine() ?? "0");

            await _repo.UpdateAsync(user);
            Console.WriteLine("Usuario actualizado" + user.Name);
        }

        public async Task EliminarUsuario()
        {
            Console.Write("ID del usuario a eliminar: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            await _repo.DeleteAsync(id);
            Console.WriteLine("Usuario eliminado");
        }
    }
}

