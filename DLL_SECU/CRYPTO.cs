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

        public static (byte[] Sel, byte[] Hash, byte[] Hash_2, byte[] Hash_3, byte[] Hash_4, byte[] Hash_5) Genere_Sel_Hash(string P_Nom, string P_Prenom, string P_Date_Naissance, string P_Mail, string P_Mdp)
        {
            var Generateur = RandomNumberGenerator.Create();
            byte[] Sel = new byte[64];
            Generateur.GetBytes(Sel);

            Rfc2898DeriveBytes Derivation = new Rfc2898DeriveBytes(P_Nom, Sel, Iterations, HashAlgorithmName.SHA512);
            Rfc2898DeriveBytes Derivation_2 = new Rfc2898DeriveBytes(P_Prenom, Sel, Iterations, HashAlgorithmName.SHA512);
            Rfc2898DeriveBytes Derivation_3 = new Rfc2898DeriveBytes(P_Mail, Sel, Iterations, HashAlgorithmName.SHA512);
            Rfc2898DeriveBytes Derivation_4 = new Rfc2898DeriveBytes(P_Date_Naissance, Sel, Iterations, HashAlgorithmName.SHA512);
            Rfc2898DeriveBytes Derivation_5 = new Rfc2898DeriveBytes(P_Mdp, Sel, Iterations, HashAlgorithmName.SHA512);

            var Le_Hash_Nom = Derivation.GetBytes(64);
            var Le_Hash_Prenom = Derivation_2.GetBytes(64);
            var Le_Hash_Mail = Derivation_3.GetBytes(64);
            var Le_Hash_Date_Naissance = Derivation_4.GetBytes(64);
            var Le_Hash_Mdp = Derivation_5.GetBytes(64);

            return (Sel,Le_Hash_Nom,Le_Hash_Prenom,Le_Hash_Mail,Le_Hash_Date_Naissance, Le_Hash_Mdp);
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
