using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;

namespace MemetProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public ICollection<Address> GetAddressesOfAUser(int UserId)
        {
            return _context.Addresses.Where(a => a.User.UserId == UserId).ToList();
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderBy(u => u.UserId).ToList();
        }

        public ICollection<Order> GetOrdersOfAUser(int UserId)
        {
            return _context.Orders.Where(o => o.User.UserId == UserId).ToList();
        }

        public User GetUserById(int UserId)
        {
            return _context.Users.Where(u => u.UserId == UserId).SingleOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UserDelete(int userId)
        {
            var addresses = _context.Addresses.Where(a => a.User.UserId == userId).ToList();
            _context.Addresses.RemoveRange(addresses);
            
            var user = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();

            if(user == null)
                return false;
            
            _context.Users.Remove(user);

            return Save();
        }

        public bool UserExists(int UserId)
        {
            return _context.Users.Any(u => u.UserId == UserId);
        }

        public User userLogin(string userMail, string userPassword){
            try {
                var user = _context.Users.Where(u => u.EMail == userMail && u.Password == userPassword).SingleOrDefault();
                if(user == null) {
                    Console.WriteLine("Kullanıcı bulunamadı: " + userMail);
                }
                return user;
            } catch (Exception ex) {
                Console.WriteLine("Hata: " + ex.Message);
                return null;
            }
        }

        public bool UserUpdate(User user, int userId)
        {
            var existingUser = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();

            if (!UserExists(userId))
                return false;


            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.EMail = user.EMail;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;

            _context.Users.Update(existingUser);
            return Save();
        }
    }
}