using System.Text.Json;

namespace DLL_SECU
{
    public class C_BASE_SECU
    {
        const string Fichier_Info_Joueur = "Ino_Joueur.json";

        List<C_INFO_JOUEUR> Info_Joueur;

        public C_BASE_SECU()
        {
            if (File.Exists(Fichier_Info_Joueur))
            {
                Charge_Data();
            } else
            {
                Info_Joueur = new List<C_INFO_JOUEUR>();
                Save();
            }
        }

        public void Charge_Data()
        {
            string Data_Info_Perso = File.ReadAllText(Fichier_Info_Joueur);

            Info_Joueur = JsonSerializer.Deserialize<List<C_INFO_JOUEUR>>(Data_Info_Perso);
        }

        public void Save()
        {
            var Data_Info_Joueur = JsonSerializer.Serialize(Info_Joueur);

            File.WriteAllText(Fichier_Info_Joueur, Data_Info_Joueur);
        }

        public Guid Generation_Guid()
        {
            Guid guid = Guid.NewGuid();

            return guid;
        }

        public (RESULAT_BDD, C_INFO_JOUEUR) Ajoute_User(string P_Nom,string P_Prenom,string P_Mail,string P_Date_Naissance, string P_Nom_Utilisateur, string P_Mdp)
        {
            var Trouver = Info_Joueur.Where(U => U.Nom_Uilisateur == P_Nom_Utilisateur).FirstOrDefault();

            if (Trouver == null)
            {

                if (CRYPTO.Verifie_Force_Password(P_Mdp) != true) return (RESULAT_BDD.MDP_FAIBLE, null);
                if (CRYPTO.Verifie_Valide_Mail(P_Mail) != true) return (RESULAT_BDD.MAIL_NON_VALIDE, null);

                var Guid = Generation_Guid();
                var Encode_Les_Donnees = CRYPTO.Genere_Sel_Hash(P_Nom, P_Prenom, P_Mail, P_Date_Naissance, P_Mdp);
                var Nouvel_Utilisateur_Info_Perso = new C_INFO_JOUEUR()
                {
                    Id_User = Guid, Nom = Encode_Les_Donnees.Hash, Prenom = Encode_Les_Donnees.Hash_2,
                    Mail = Encode_Les_Donnees.Hash_3, Date_Naissance = Encode_Les_Donnees.Hash_4, Mdp = Encode_Les_Donnees.Hash_5,
                    Sel = Encode_Les_Donnees.Sel, Nom_Uilisateur = P_Nom_Utilisateur, Role = (int)ROLE.Joueur
                };

                Info_Joueur.Add(Nouvel_Utilisateur_Info_Perso);

                Save();

                return (RESULAT_BDD.Ok, Nouvel_Utilisateur_Info_Perso);

            }
            return (RESULAT_BDD.EXISTE_DEJA, null);
        }

        public (RESULAT_BDD, C_COMPTE) Connexion_User(string P_Nom_Uilisateur, string P_Mdp)
        {
            var Trouver = Info_Joueur.Where(U => U.Nom_Uilisateur == P_Nom_Uilisateur).FirstOrDefault();

            if (Trouver == null) return (RESULAT_BDD.EXISTE_PAS, null);
            if (CRYPTO.Verifie_Force_Password(P_Mdp) != true) return (RESULAT_BDD.MDP_FAIBLE, null);

            bool Ok = CRYPTO.Atteste_Identite(P_Mdp, Trouver.Sel, Trouver.Mdp);

            if (Ok) return (RESULAT_BDD.Ok, new C_COMPTE()
            {
                Id_User = Trouver.Id_User, Nom_Uilisateur = Trouver.Nom_Uilisateur,
                Role = Trouver.Role
            });
            return (RESULAT_BDD.ERREUR_MDP, null);
        }

    }
}
