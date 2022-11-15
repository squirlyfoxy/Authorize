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
                    new Permission()
                    {
                        PermissionCode = "City",

                        Read = PermissionRead.ReadAdvanced,
                        Write = PermissionWrite.WriteBasic
                    }
                },
                Friends = new[] {"Andrea", "Luigi" }
            };
            Manager.RegisterClass(typeof(City));
            Manager.RegisterCurrentUser(currentUser);

            var l = new Authorize.Core.Linq.Linq();
            l.WhereQueries.Add(new Authorize.Core.Linq.LinqPermissionQueryRule((int)PermissionRead.ReadAdvanced, x => x != "Andrea"));
            l.WhereQueries.Add(new Authorize.Core.Linq.LinqPermissionQueryRule((int)PermissionRead.ReadBasic, x => x != "Luigi"));

            using (var perm = Permitter.Instance<User>(currentUser))
            {
                Console.WriteLine((string)perm.Get("Name"));
                Console.WriteLine((string)perm.Get("Surname"));
                perm.Set("Name", "Alessandro");
                Console.WriteLine((string)perm.Get("Name"));

                foreach (var item in perm.Where("Friends", l, currentUser.Friends))
                {
                    Console.WriteLine("Friend: " + (string)item);
                }
            }

            var cesena = new City()
            {
                Name = "Cesena",
                Country = "IT",
                UsersThatLiveHere = new User[]
                {
                    currentUser,
                    new User()
                    {
                        Name = "Andrea",
                        Surname = "Adreoni"
                    },
                    new User()
                    {
                        Name = "Federico",
                        Surname = "Armanni"
                    },
                }
            };

            Console.WriteLine(" -- City -- ");

            var cityL = new Authorize.Core.Linq.Linq();
            cityL.WhereQueries.Add(new Authorize.Core.Linq.LinqPermissionQueryRule((int)PermissionRead.ReadAdvanced, x => x.Name != currentUser.Name));
            cityL.WhereQueries.Add(new Authorize.Core.Linq.LinqPermissionQueryRule((int)PermissionRead.ReadBasic, x => x.Name == currentUser.Name));
            using (var perm = Permitter.Instance(cesena))
            {
                Console.WriteLine("Name: " + (string)perm.Get("Name"));

                foreach (var item in perm.Where("UsersThatLiveHere", cityL, cesena.UsersThatLiveHere))
                {
                    Console.WriteLine("User: " + (item as User).Name);
                }
            }

            Console.ReadKey();
        }
    }
}
