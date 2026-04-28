using DevelopTest.Data;
using DevelopTest.Models;
using DevelopTest.Responses;

namespace DevelopTest.Services;

public class UserService
{
    private readonly MySqlDbContext _context;
    
    public UserService(MySqlDbContext context)
    { 
        _context = context;
    }

    public ServiceResponse<IEnumerable<User>> GetAllUsers()
    {
        var users = _context.Users.ToList();
        return new ServiceResponse<IEnumerable<User>>()
        {
            Success = true,
            Data = users
        };
    }

    public ServiceResponse<User> SaveUser(User user)
    {
        try
        {
            if (user == null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "User cannot be null"
                };
            }

            if (string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Document) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Data = user,
                    Message = "Name, Document and Email are required"
                };
            }

            var existsDocument = _context.Users
                .FirstOrDefault(u => u.Document == user.Document);

            if (existsDocument != null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Data = user,
                    Message = "User already exists with this document"
                };
            }

            var existsEmaill = _context.Users
                .FirstOrDefault(u => u.Email == user.Email);

            if (existsEmaill != null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Data = user,
                    Message = "User already exists with this email"
                };
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return new ServiceResponse<User>()
            {
                Success = true,
                Data = user,
                Message = "User created successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<User>()
            {
                Success = false,
                Message = "Error creating user"
            };
        }
    }

    public ServiceResponse<User> GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            return new ServiceResponse<User>()
            {
                Success = true,
                Data = user,
                Message = "User found"
            };
        }

        return new ServiceResponse<User>()
        {
            Success = false,
            Data = user,
            Message = "User not found"
        };
    }

    public ServiceResponse<User> UpdateUser(User user)
    {
        try
        {
            if (user == null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "User cannot be null"
                };
            }

            var userDb = _context.Users.Find(user.Id);

            if (userDb == null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Document) ||
                string.IsNullOrWhiteSpace(user.Email))
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Data = user,
                    Message = "Name, Document and Email are required"
                };
            }

            var existsDocument = _context.Users
                .FirstOrDefault(u => u.Document == user.Document && u.Id != user.Id);

            if (existsDocument != null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Data = user,
                    Message = "Another user already has this document"
                };
            }

            var existsEmail = _context.Users
                .FirstOrDefault(u => u.Email == user.Email && u.Id != user.Id);

            if (existsEmail != null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Data = user,
                    Message = "Another user already has this email"
                };
            }

            userDb.Name = user.Name;
            userDb.Document = user.Document;
            userDb.Email = user.Email;
            userDb.Phone = user.Phone;

            _context.SaveChanges();

            return new ServiceResponse<User>()
            {
                Success = true,
                Data = user,
                Message = "User updated successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<User>()
            {
                Success = false,
                Message = "Error updating user"
            };
        }
    }
    
    public ServiceResponse<User> DeleteUser(User user)
    {
        try
        {
            if (user == null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "User cannot be null"
                };
            }

            var userDb = _context.Users.Find(user.Id);

            if (userDb == null)
            {
                return new ServiceResponse<User>()
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            _context.Users.Remove(userDb);
            _context.SaveChanges();

            return new ServiceResponse<User>()
            {
                Success = true,
                Data = user,
                Message = "User removed successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<User>()
            {
                Success = false,
                Message = "Error removing user"
            };
        }
    }
    
}