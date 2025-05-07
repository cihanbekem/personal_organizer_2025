using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace po
{
    public class User
    {
        public int ID { get; set; }
        // Kullanıcının e-posta adresi
        public string Email { get; set; }

        // Kullanıcının şifresi
        public string Password { get; set; }

        // Kullanıcının rolü (Admin, User vb.)
        public string Role { get; set; }

        // Kullanıcının adı
        public string Name { get; set; }

        // Kullanıcının soyadı
        public string Surname { get; set; }

        // Kullanıcının telefon numarası
        public string Phone { get; set; }

        // Kullanıcının adresi
        public string Address { get; set; }

        // Kullanıcının fotoğrafı (Base64 formatında)
        public string Photo { get; set; }
    }
}
