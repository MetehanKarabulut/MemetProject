using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;
using Microsoft.AspNetCore.SignalR;

namespace MemetProject.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(int UserId);
        ICollection<User> GetAllUsers();
        User GetUserById(int UserId);
        ICollection<Order> GetOrdersOfAUser(int UserId);
        ICollection<Address> GetAddressesOfAUser(int UserId);
        bool CreateUser(User user);
        User userLogin(string userMail, string userPassword);
        bool UserDelete(int userId);
        bool UserUpdate(User user, int userId);
        bool Save();
    }
}