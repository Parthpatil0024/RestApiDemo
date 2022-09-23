using RestApiBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.IO;
using System.Security.Cryptography;

namespace RestApiBL.Repo
{
    public class UserRepo
    {
        public IEnumerable<tblUser> getUsersList()  
        {
            using (DBTContext context = new DBTContext())
            {
                return context.tblUsers.ToList();
            }


        }

        public tblUser getEmployeeById(int Id)   
        {
            using (DBTContext context = new DBTContext())
            {
                return context.tblUsers.FirstOrDefault(e => e.Id == Id);
            }
        }

        public bool saveUser(tblUser user)
        {
            bool result = false;
            using (DBTContext context = new DBTContext())
            {
                
                context.tblUsers.AddOrUpdate(user);
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool deleteUser(int Id)
        {
            bool result = false;
            using (DBTContext context = new DBTContext())
            {
                var user=context.tblUsers.FirstOrDefault(e => e.Id == Id);
                context.tblUsers.Remove(user);
                context.SaveChanges();
                result = true;
            }
            return result;
        }

        public tblUser authenticateUser(string emailId, string password)
        {
            tblUser user;
            password = encrypt(password);

            using (DBTContext context = new DBTContext())
            {
                user = context.tblUsers.Where(x => x.EmailId == emailId && x.Password == password).FirstOrDefault();
            }
            return user;
        }

        public string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
    }
}
