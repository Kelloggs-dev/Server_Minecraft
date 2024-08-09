using DLL_SECU;

namespace Test_Lib
{
    internal class Program
    {
        static void Main(string[] args)
        {
            C_BASE_SECU La_Base = new C_BASE_SECU();

            var (Result,Nouvel_Utilisateur) = La_Base.Ajoute_User("Geslin", "Remi", "remigeslin23@gmail.com","16/03/2004","Kelloggs1", "Test123@");

            if (Result == RESULAT_BDD.Ok)
            {
                Console.WriteLine(Result);
            } else
            {
                Console.WriteLine(Result);
            }

            var (Result_2,Utilisateur_Connecter) = La_Base.Connexion_User("Kelloggs1", "Test123@");

            if (Result_2 == RESULAT_BDD.Ok)
            {
                Console.WriteLine(Utilisateur_Connecter.ToString());
            } else {
                Console.WriteLine(Result_2);
            }
        }
    }
}
