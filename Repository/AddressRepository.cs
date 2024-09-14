using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MemetProject.Repository
{
    public class AddressRepository : IAddressRepository 
    {
        private readonly DataContext _context;
        public AddressRepository(DataContext context)
        {
            _context = context;
        }

        public bool AddressExists(int UserId, string addressTitle)
        {
            return _context.Users.Where(u => u.UserId == UserId).Any(u => u.UserAddress.Any(a => a.AddressTitle == addressTitle));
        }

        public bool CreateAddress(int UserId, Address address)
        {
            var user = _context.Users.Where(u => u.UserId == UserId).SingleOrDefault();
            if(user == null)
                return false;

            address.User = user;

            _context.Add(address);
            
            return Save();
        }

        public Address GetAddressById(int addressId)
        {
            return _context.Addresses.Where(a => a.AddressId == addressId).SingleOrDefault();
        }

        public ICollection<Address> GetAddressesOfAUser(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId).Select(u => u.UserAddress).SingleOrDefault();
        }
        public bool AddressDelete(int AddressId)
        {
            var Address = _context.Addresses.Where(o => o.AddressId == AddressId).SingleOrDefault();

            if(Address == null)
                return false;

            _context.Addresses.Remove(Address);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool AddressExists(int addressId)
        {
            return _context.Addresses.Any(a => a.AddressId == addressId);
        }
        public bool AddressUpdate(Address Address)
        {
            var existingAddress = _context.Addresses.FirstOrDefault(u => u.AddressId == Address.AddressId);

            if (!AddressExists(Address.AddressId))
                return false;


            _context.Entry(existingAddress).CurrentValues.SetValues(Address);

            _context.Addresses.Update(existingAddress);
            return Save();
        }
    }
}