using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DLL_SECU
{
    static class CRYPTO
    {
        public static int Iterations = 10000;

        public static (byte[] Sel, byte[] Hash) Genere_Sel_Hash(string P_Message)
        {
            var Generateur = RandomNumberGenerator.Create();
            byte[] Sel = new byte[64];
            Generateur.GetBytes(Sel);

            Rfc2898DeriveBytes Derivation = new Rfc2898DeriveBytes(P_Message, Sel, Iterations, HashAlgorithmName.SHA512);

            var Le_Hash = Derivation.GetBytes(64);

            return (Sel, Le_Hash);
        }

        //--------------------------------------------------
        public static byte[] Calcule_Hash(string P_Message, byte[] P_Sel)
        {
            Rfc2898DeriveBytes Derivation = new Rfc2898DeriveBytes(P_Message, P_Sel, Iterations, HashAlgorithmName.SHA512);
            return Derivation.GetBytes(64);
        }

        //--------------------------------------------------
        public static bool Atteste_Identite(string P_Message, byte[] P_Sel, byte[] P_Hash)
        {
            var Hash_Calcule = Calcule_Hash(P_Message, P_Sel);
            return Hash_Calcule.SequenceEqual(P_Hash);
        }

        //--------------------------------------------------
        public static bool Verifie_Force_Password(string P_Mot_De_Passe)
        {
            string Modele = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(P_Mot_De_Passe, Modele);
        }
    }
}
