using DLL_SECU;

namespace Test_Lib
{
    internal class Program
    {
        static void Main(string[] args)
        {
            C_BASE_SECU La_Base = new C_BASE_SECU();

            var result = La_Base.Ajoute_User("Geslin", "Remi", "remigeslin23@gmail.com","16/03/2004","Kelloggs", "Test123@");

            if (result == RESULAT_BDD.Ok)
            {
                Console.WriteLine("ok");
            } else
            {
                Console.WriteLine("erreur");
            }
        }
    }
}
