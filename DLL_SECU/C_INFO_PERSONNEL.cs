using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace DLL_SECU
{
    public class C_INFO_PERSONNEL
    {
        public Guid Id_User { get; set; }
        public byte[] Nom { get; set; }
        public byte[] Prenom { get; set; }
        public byte[] Mail { get; set; }
        public byte[] Date_Naissance { get; set; }
        public byte[] Sel { get; set; }
        public byte[] Mdp { get; set; }
    }
}
