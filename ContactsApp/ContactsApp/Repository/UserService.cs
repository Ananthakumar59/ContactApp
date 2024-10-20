using ContactsApp.IRepository;
using ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContactsApp.Repository
{
    public class UserService:IUserService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "mockDatabase.json");
        private static readonly object _fileLock = new object();

        // Read all users from the JSON file
        public List<User> GetAllUsers()
        {
            var json = File.ReadAllText(_filePath);
            var users = JsonConvert.DeserializeObject<DatabaseModel>(json)?.Users ?? new List<User>();
            return users;
        }

        // Add a new user to the JSON file
        public User AddUser(User newUser)
        {
            var users = GetAllUsers();
            newUser.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            users.Add(newUser);

            // Update the JSON file
            var updatedData = new DatabaseModel { Users = users };
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(updatedData, Formatting.Indented));

            return newUser;
        }

        public User UpdateUser(int id, User updatedUser)
        {
            var users = GetAllUsers();

            // Find the user with the specified ID
            var existingUser = users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return null; // Return null if the user is not found (you can handle this differently)
            }

            // Update user details
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;

            // Save the updated user list back to the file
            var updatedData = new DatabaseModel { Users = users };
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(updatedData, Formatting.Indented));

            return existingUser;
        }

        public bool DeleteUser(int id)
        {
            var users = GetAllUsers();

            // Find the user with the specified ID
            var userToRemove = users.FirstOrDefault(u => u.Id == id);
            if (userToRemove == null)
            {
                return false; // Return false if the user is not found
            }

            // Remove the user from the list
            users.Remove(userToRemove);

            // Save the updated user list back to the file
            var updatedData = new DatabaseModel { Users = users };
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(updatedData, Formatting.Indented));
          
            return true; // Return true to indicate the user was successfully deleted
        }
        private void SaveChanges(List<User> users)
        {
            lock (_fileLock)
            {
                var updatedData = new DatabaseModel { Users = users };
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(updatedData, Formatting.Indented));
            }
        }

    }
    // Database model to map the entire JSON structure
    public class DatabaseModel
    {
        public List<User> Users { get; set; }
    }
   

}
