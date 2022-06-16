
using ExamenProject3.Data;
using ExamenProject3.Forms;
using ExamenProject3.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ExamenProject3.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(SignUpForm form);

        Task<IEnumerable<UserModel>> GetAllAsync();

        Task<UserModel> GetUserByIdAsync(int id);

        Task<bool> DeleteAsync(int id); 

        Task<UserUpdate> UpdateAsync(int id, UserUpdate userUpdate);

    }
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAsync(SignUpForm form)
        {
            var created = false;
            var newUser = await _context.Users.FirstOrDefaultAsync(u=> u.Email == form.Email);
            var UserAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.PostalCode == form.PostalCode);
            int addressId;

            
            if(newUser == null)
            {
                if (UserAddress != null && UserAddress.StreetName == form.StreetName && UserAddress.City == form.City)
                    addressId = UserAddress.Id;
                else
                {
                    var newAddress = new UserAddressEntity { StreetName = form.StreetName, City = form.City, PostalCode = form.PostalCode };
                    await _context.Addresses.AddAsync(newAddress);
                    await _context.SaveChangesAsync();
                    addressId = newAddress.Id;
                }
                newUser = new UserEntity { FirstName = form.FirstName, 
                    LastName = form.LastName,
                    Email = form.Email,
                    PhoneNumber = form.PhoneNumber,
                    UserAddressId = addressId,  
                };
               newUser.CreateSecurityPassword(form.Password);
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                created = true;
               
            }
            return created;
        }


        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            List<UserModel> users = new List<UserModel>();
            foreach(UserEntity user in  await _context.Users.Include(u => u.UserAddress).ToListAsync())
            {
                users.Add(new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserAddressId = user.UserAddressId,
                    StreetName = user.UserAddress.StreetName,
                    ZipCode = user.UserAddress.PostalCode,
                    City= user.UserAddress.City
                });
            }
            return users;
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                var address = await _context.Addresses.FindAsync(user.UserAddressId);
                return new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserAddressId = user.UserAddressId,
                    StreetName = address.StreetName,
                    ZipCode = address.PostalCode,
                    City = address.City,
                };
            }  
            return null;    
        }


        public async Task<bool> DeleteAsync(int id)
        {
           var deleted = false;
           var user = await _context.Users.FindAsync(id);
            if( user != null)
            {
                _context.Users.Remove(user);    
                await _context.SaveChangesAsync();
                deleted = true;
            }
            return deleted;
        }

        public async Task<UserUpdate> UpdateAsync(int id, UserUpdate userUpdate)
        {
            var user = await _context.Users.FindAsync(id);
            UserAddressEntity newaddress;
            if(user != null)
            {
              
                 var address = await _context.Addresses.FindAsync(user.UserAddressId);
                if(address.StreetName != userUpdate.StreetName || address.PostalCode != userUpdate.PostalCode || address.City != userUpdate.City)
                {
                    newaddress = new UserAddressEntity
                    {
                        StreetName = userUpdate.StreetName,
                        PostalCode = userUpdate.PostalCode,
                        City = userUpdate.City
                    };
                    await _context.Addresses.AddAsync(newaddress);
                    await _context.SaveChangesAsync();
                    user.UserAddressId = newaddress.Id;
                    await _context.SaveChangesAsync();
                }
                user.PhoneNumber = userUpdate.PhoneNumber;
                
                await _context.SaveChangesAsync();
                newaddress = await _context.Addresses.FindAsync(user.UserAddressId);
                return new UserUpdate
                {
                    PhoneNumber = user.PhoneNumber,
                    UserAddressId = user.UserAddressId,
                    StreetName = newaddress.StreetName,
                    PostalCode = newaddress.PostalCode,
                    City = newaddress.City

                };
            }
            return null; 

        }
    }
}
