using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Authorize;
using Authorize.Core.H;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Manager.RegisterUserClass(typeof(User), typeof(User).GetProperty(nameof(User.Permissions)));
            Manager.RegisterPermissionClass(typeof(Permission[]), typeof(Permission).GetProperty("Read"), typeof(Permission).GetProperty("Write"),
                typeof(Permission).GetProperty("PermissionCode"));

            User currentUser = new User()
            {
                Name = "Ciao",
                Surname = "AA",
                Permissions = new[]
                {
                    new Permission()
                    {
                        // if empty, permit everything withount a permission code
                        PermissionCode = "",

                        Read = PermissionRead.ReadBasic,
                        Write = PermissionWrite.WriteBasic
                    },
                    new Permission()
                    {
                        // if empty, permit everything withount a permission code
                        PermissionCode = "Nna",

                        Read = PermissionRead.ReadAdvanced,
                        Write = PermissionWrite.WriteAdvanced
                    },
                },
                Friends = new[] {"Hello", "World" }
            };
            Manager.RegisterCurrentUser(currentUser);

            var l = new Authorize.Core.Linq.Linq();
            l.WhereQueries.Add(new Authorize.Core.Linq.LinqPermissionQueryRule((int)PermissionRead.ReadAdvanced, x => x != "Hello"));
            l.WhereQueries.Add(new Authorize.Core.Linq.LinqPermissionQueryRule((int)PermissionRead.ReadBasic, x => x != "World"));

            foreach (var item in Authorize.Core.Class.Query<User>(l, typeof(User).GetProperties().Where(p => p.Name == "Friends").ToList()[0], currentUser.Friends))
            {
                Console.WriteLine("               " + (string)item);
            }

            using (var perm = Permitter.Instance<User>(currentUser))
            {
                Console.WriteLine((string)perm.Get("Name"));
                Console.WriteLine((string)perm.Get("Surname"));
                perm.Set("Name", "Alessandro");
                Console.WriteLine((string)perm.Get("Name"));
            }

            Console.ReadKey();
        }
    }
}
