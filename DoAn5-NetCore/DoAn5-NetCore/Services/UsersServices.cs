using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn5_NetCore.Models;

namespace DoAn5_NetCore.Service
{
    public interface IUserServices
    {
        Users Authenticate(string username, string password);
        IEnumerable<Users> GetAll();
        Users GetById(Guid id);
        Users Create(Users user, string password);
        void Update(Users user, string password = null);
        void Delete(Guid id);
    }

    public class UsersServices : IUserServices
    {
        private ChatingContext _context;

        public UsersServices(ChatingContext context)
        {
            _context = context;
        }

        public IEnumerable<Users> GetAll()
        {
            return _context.Users;
        }

        // lấy user theo mã

        public Users GetById(Guid id)
        {
            return _context.Users.Find(id);
        }

        public Users Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public Users Create(Users user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Vui lòng nhập mật khẩu");

            if (_context.Users.Any(x => x.Email == user.Email))
                throw new Exception("Email \"" + user.Email + "\" đã được đăng ký bởi tài khoản khác");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(Users userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.UsersId);

            if (user == null)
                throw new Exception("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Email == userParam.Email))
                    throw new Exception("Email " + userParam.Email + " is already taken");

                user.Email = userParam.Email;
            }

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }


        public void Delete(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] storedHash, out byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                storedSalt = hmac.Key;
                storedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Value cannot be empty or whitespace only string.");
            if (storedHash.Length != 64) throw new Exception("Invalid length of password hash (64 bytes expected) = " + storedHash.Length);
            if (storedSalt.Length != 128) throw new Exception("Invalid length of password salt (128 bytes expected) = " + storedSalt.Length);

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}
