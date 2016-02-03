using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD.Models {
    public enum Czas {
        min15 = 15,
        min30 = 30,
        min45 = 45,
        h1 = 60,
        h2 = 120
    }

    public static class CzasWizyty{   
        public static TimeSpan getTimeSpan(Czas czas) {
            return new TimeSpan(0, (int)czas, 0);
        }

    }
}
