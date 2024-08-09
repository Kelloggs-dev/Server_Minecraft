using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_SECU
{
    public class C_COMPTE
    {

        public Guid Id_User { get; set; }
        public string Nom_Uilisateur { get; set; }
        public int Role { get; set; }

        public override string ToString()
        {
            return $"{Id_User}, {Nom_Uilisateur}, {Role}";
        }
    }

    
}
