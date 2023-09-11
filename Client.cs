using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleStore_ConsoleApp
{

    internal class Client
    {
        static readonly string PathClients = AppDomain.CurrentDomain.BaseDirectory + "Clients.csv";
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }

        public Client(string login, string name, string surname, string password)
        {
            Login = login;
            Name = name;
            Surname = surname;
            Password = password;
        }

        public string ToStringCSV()
        {
            return $"{Login};{Name};{Surname};{Password}";
        }


        public static void AddNewClient(List<Client> clients, string login)
        {
            Console.Write("Enter your name: ");
            string? name = Program.ReadString("Name");


            Console.Write("Enter your surname: ");
            string? surname = Program.ReadString("Surname");


            string password = "1";
            string passwordConfirm = "";
            Console.Write("Enter your password: ");
            while (password != passwordConfirm)
            {
                password = Program.ReadPassword();

                while (!Program.IsPasswordSecure(password))
                {
                    Console.WriteLine("\nPassword must include: \n1.Lowercase letter. \n2.Uppercase letter. \n3.Special character. \n4.Number. \n5.Be at least 8 characters.");
                    Console.Write("Please try again. \nEnter your password:");
                    password = Program.ReadPassword();
                }

                Console.Write("\nConfirm your password: ");
                passwordConfirm = Program.ReadPassword();
                if (passwordConfirm == password)
                {
                    break;
                }
                Console.WriteLine($"\nWrong password.");
                Console.Write("Enter your password: ");

            }

            Client client = new(login, name, surname, password);

            clients.Add(client);

            Console.WriteLine("\nAccount has been created. Log in to confirm.");

        }

        public static void GetClients(List<Client> clients)
        {


            using StreamReader sr = new(PathClients);

            while (!sr.EndOfStream)
            {

                string? line = sr.ReadLine();
                if (line != null)
                {
                    string[] dane = line.Split(';');
                    Client c = new(dane[0], dane[1], dane[2], dane[3]);
                    clients.Add(c);
                }
                else
                {
                    Console.WriteLine("Unknown error.");
                    Program.Exit();
                    return;
                }
            }
        }

        public static void SaveClients(List<Client> clients)
        {
            using StreamWriter sw = new(PathClients);
            foreach (Client c in clients)
            {
                sw.WriteLine(c.ToStringCSV());
            }
        }

    }
}
