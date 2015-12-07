using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD.Models
{
    public class Uzytkownik
    {
        [Key]
        public Guid UzytkownikId { get; set; }
        public String Login { get; set; }
        public String Haslo { get; set; }
        //[Required(ErrorMessage = "Adres email jest wymagany")] 
        //[EmailAddress(ErrorMessage = "Adres email jest niepoprawny")]
        //public String Email { get; set; }
        //Uprawnienia
        //kontrola dostępu przy logowaniu do systemu
        //public Boolean podglad { get; set; }
        //public Boolean edycjaWizyt { get; set; }
        //public Boolean dodawanieWizyt { get; set; } 
    }
}
