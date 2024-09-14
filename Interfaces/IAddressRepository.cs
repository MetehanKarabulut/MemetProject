using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface IAddressRepository
    {
        bool AddressExists(int UserId, string addressTitle);
        bool AddressExists(int addressId);
        bool AddressDelete(int addressId);
        ICollection<Address> GetAddressesOfAUser(int UserId);
        Address GetAddressById(int AddressId);
        bool CreateAddress(int userId, Address address);
        bool Save();
        bool AddressUpdate(Address address);
    }
}