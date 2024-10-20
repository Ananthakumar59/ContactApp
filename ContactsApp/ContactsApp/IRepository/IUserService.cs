using ContactsApp.Models;

namespace ContactsApp.IRepository
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User AddUser(User newUser);
        User UpdateUser(int id, User updatedUser);
        bool DeleteUser(int id);
    }
}
