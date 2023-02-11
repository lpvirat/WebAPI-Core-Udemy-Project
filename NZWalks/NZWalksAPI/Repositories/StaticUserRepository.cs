using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class StaticUserRepository : IUserRepository
    {

        private List<User> users = new List<User>()
        {
            //new User()
            //{
            //    Id= Guid.NewGuid(),FirstName = "Read Only",LastName="User",
            //    Username="readonly@user.com",Password="Readonly@user",EmailAddress="readonly@user.com",
            //    Roles=new List<string>{"reader"}
            //},
            //new User()
            //{
            //    Id= Guid.NewGuid(),FirstName = "Read Write",LastName="User",
            //    Username="readwrite@user.com",Password="Readwrite@user",EmailAddress="readwrite@user.com",
            //    Roles=new List<string>{"reader","writer"}
            //}
        };

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = users.Find(x => x.Username.ToLower() == username.ToLower() && x.Password.ToLower() == password.ToLower());
            return user;

        }
    }
}
