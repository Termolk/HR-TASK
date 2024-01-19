using HRCRUD.BD;
using HRCRUD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCRUD.Service
{
    public class AuthService
    {
        private readonly HRSystemContext _context;

        public AuthService(HRSystemContext context)
        {
            _context = context;
        }

        public User Login(string username, string password)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == username);
            if (user != null && VerifyPasswordHash(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return password == storedHash;
        }
    }

}
