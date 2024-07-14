using System.Text.Json;

namespace DLL_SECU
{
    public class C_BASE_SECU
    {
        const string Fichier_Info_Personnel = "Ino_Perso.json";
        const string Fichier_Nom_Utilisateur = "Nom_Utilisateur.json";

        List<C_INFO_PERSONNEL> Info_Perso;
        List<C_NOM_UTILISATEUR> Nom_Uilisateur;

        public C_BASE_SECU()
        {
            if (File.Exists(Fichier_Nom_Utilisateur))
            {
                Charge_Data();
            } else
            {
                Info_Perso = new List<C_INFO_PERSONNEL>();
                Nom_Uilisateur = new List<C_NOM_UTILISATEUR>();
                Save();
            }
        }

        public void Charge_Data()
        {
            string Data_Info_Perso = File.ReadAllText(Fichier_Info_Personnel);
            string Data_Nom_Utilisateur = File.ReadAllText(Fichier_Nom_Utilisateur);

            Info_Perso = JsonSerializer.Deserialize<List<C_INFO_PERSONNEL>>(Data_Info_Perso);
            Nom_Uilisateur = JsonSerializer.Deserialize<List<C_NOM_UTILISATEUR>>(Data_Nom_Utilisateur);
        }

        public void Save()
        {
            var Data_Info_Perso = JsonSerializer.Serialize(Info_Perso);
            var Data_Nom_Utilisateur = JsonSerializer.Serialize(Nom_Uilisateur);

            File.WriteAllText(Fichier_Info_Personnel, Data_Info_Perso);
            File.WriteAllText(Fichier_Nom_Utilisateur, Data_Nom_Utilisateur);
        }

        public Guid Generation_Guid()
        {
            Guid guid = Guid.NewGuid();

            return guid;
        }

        public RESULAT_BDD Ajoute_User(string P_Nom, string P_Prenom, string P_Mail, string P_Date_Naissance, string P_Nom_Utilisateur, string P_Mdp)
        {
            var Trouver = Nom_Uilisateur.Where(U => U.Nom_Utilisateur == P_Nom_Utilisateur).FirstOrDefault();

            if (Trouver == null)
            {
                if (CRYPTO.Verifie_Force_Password(P_Mdp) != true) return RESULAT_BDD.MDP_FAIBLE;
                var Guid = Generation_Guid();
                var Encode_Les_Donnees = CRYPTO.Genere_Sel_Hash(P_Nom, P_Prenom, P_Mail, P_Date_Naissance, P_Mdp);
                var Nouvel_Utilisateur_Info_Perso = new C_INFO_PERSONNEL()
                {
                    Id_User = Guid, Nom = Encode_Les_Donnees.Hash, Prenom = Encode_Les_Donnees.Hash_2,
                    Mail = Encode_Les_Donnees.Hash_3, Date_Naissance = Encode_Les_Donnees.Hash_4, Mdp = Encode_Les_Donnees.Hash_5,
                    Sel = Encode_Les_Donnees.Sel
                };
                var Nouvel_Utilisateur = new C_NOM_UTILISATEUR() { Id_User = Guid,Nom_Utilisateur = P_Nom_Utilisateur };

                Info_Perso.Add(Nouvel_Utilisateur_Info_Perso);
                Nom_Uilisateur.Add(Nouvel_Utilisateur);

                Save();

                return RESULAT_BDD.Ok;

            }
            return RESULAT_BDD.EXISTE_DEJA;
        }
    }
}
