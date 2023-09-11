namespace WholesaleStore_ConsoleApp
{
    internal class Program
    {
        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            return password;
        }

        public static string ReadString(string name)
        {
            string? str = Console.ReadLine();

            while (str == null || str == "")
            {
                Console.WriteLine($"{name} cannot be empty. Please try again: ");
                Console.Write($"{name}: ");
                str = Console.ReadLine();
            }

            return str;
        }

        public static int ReadInt()
        {
            string? input = Console.ReadLine();
            bool convSuccess = false;
            int parsedValue = 0;

            while (!convSuccess)
            {
                if (int.TryParse(input, out parsedValue))
                {
                    convSuccess = true;
                }
                else
                {
                    Console.Write("Wrong value. Please try again: ");
                    input = Console.ReadLine();
                }
            }
            return parsedValue;

        }


        public static bool IsPasswordSecure(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            if (!password.Any(IsSpecialCharacter))
            {
                return false;
            }

            return true;
        }

        private static bool IsSpecialCharacter(char c)
        {
            return !char.IsLetterOrDigit(c);
        }

        public static void Exit()
        {
            Console.WriteLine("Exiting...");
            Thread.Sleep(2000);
        }

        static void Main()
        {
            Console.Title = "Jacob Digital Enterprise";

            DateTime currentDate = DateTime.Now;
            string currentDateString = currentDate.ToString("dd-MM-yyyy");

            List<Product> Products = new();
            List<Product> Cart = new();
            List<Client> Clients = new();
            List<Invoice> Invoices = new();

            Product.GetProducts(Products);
            Client.GetClients(Clients);
            Invoice.GetInvoices(Invoices);

            string? CurrentName = "";
            string? CurrentSurname = "";
            string? CurrentPassword = "";

            string ActiveInvoiceID = Invoice.GetInvoiceID(Invoices);

            bool IsLoginOnList = false;
            bool IsShopListCorrect = false;

            double TotalPrice = 0;


            Console.WriteLine("Welcome to Jacob Digital Enterprise!");
            Console.WriteLine("The administrator of personal data is the designated representative of the J.D. Enterprise.");
            Console.WriteLine("By logging in, the user consents to the processing of his personal data by the Board of \nJacob Digital Enterprise and designated persons.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Enter your login to proceed. If you don't have an account you will be able to create one or continue as guest.");
            Console.Write("Login: ");
            string? EnteredLogin = Console.ReadLine();

            while (EnteredLogin == null || EnteredLogin == "" || EnteredLogin == "guest")
            {
                if (EnteredLogin == "guest")
                {
                    Console.WriteLine("You cannot use guest login. Please use another one.");

                }
                else
                {
                    Console.WriteLine("Login cannot be empty. Please try again.");
                }

                Console.Write("Login: ");
                EnteredLogin = Console.ReadLine();
            }

            foreach (Client c in Clients)
            {
                if (c.Login == EnteredLogin)
                {
                    IsLoginOnList = true;
                    CurrentName = c.Name;
                    CurrentPassword = c.Password;
                    CurrentSurname = c.Surname;
                    break;

                }
            }

            if (!IsLoginOnList)
            {
                Console.WriteLine("We don't have that login in clients database. Do you want to create an account with this login now?");

                string? ans = ReadString("Answer");
                bool IsAnswer1Correct = false; bool IsAnswer2Correct = false;
                while (!IsAnswer1Correct)
                {
                    if (ans.ToLower() == "yes" || ans.ToLower() == "y")
                    {
                        Client.AddNewClient(Clients, EnteredLogin);

                        foreach (Client c in Clients)
                        {
                            if (c.Login == EnteredLogin)
                            {
                                CurrentName = c.Name;
                                CurrentPassword = c.Password;
                                CurrentSurname = c.Surname;
                                break;
                            }
                        }
                        IsAnswer1Correct = true;

                    }
                    else if (ans.ToLower() == "no" || ans.ToLower() == "n")
                    {
                        Console.WriteLine("Want to proceed as guest?");
                        string? ans2 = ReadString("Answer");

                        while (!IsAnswer2Correct)
                        {
                            if (ans2.ToLower() == "no" || ans.ToLower() == "n")
                            {
                                Console.WriteLine("Thanks for visiting us.");
                                Exit();
                                IsAnswer2Correct = true;
                                return;

                            }
                            else if (ans2.ToLower() == "y" || ans2.ToLower() == "yes")
                            {
                                Console.WriteLine("Proceeding as guest...");
                                Thread.Sleep(1000);
                                EnteredLogin = "guest";
                                IsAnswer2Correct = true;
                            }
                            else
                            {
                                Console.WriteLine("Please type 'yes' or 'no'.");
                                ans2 = ReadString("Answer");
                                IsAnswer2Correct = false;
                            }
                        }
                        IsAnswer1Correct = true;
                    }
                    else
                    {
                        Console.WriteLine("Please type 'yes' or 'no'.");
                        ans = ReadString("Answer");
                        IsAnswer1Correct = false;
                    }
                }

                Console.WriteLine($"Login: {EnteredLogin}");
            }

            if (EnteredLogin != "guest")
            {

                Console.Write("Password: ");
                string EnteredPassword = ReadPassword();

                for (int i = 3; i > 0; i--)
                {
                    if (i == 1)
                    {
                        Console.WriteLine("\nEntered wrong password 3 times.");
                        Exit();
                        return;
                    }

                    if (EnteredPassword != CurrentPassword)
                    {
                        Console.Write($"\nEnter your password ({i - 1} trial(s) left): ");
                        EnteredPassword = ReadPassword();
                    }

                    if (EnteredPassword == CurrentPassword)
                    {
                        break;
                    }
                }
            }
            Console.Clear();
            Console.WriteLine($"\nHi {CurrentName},\nPress any key to check our product list. ");
            Console.ReadKey();

            while (!IsShopListCorrect)
            {
                Product.ShowProducts(Products, "[  PRODUCT LIST  ]");
                Console.WriteLine("To add product to cart type proper ID number (0 to finish): ");
                int? SelectedProductId = ReadInt();

                while (SelectedProductId != 0)
                {
#pragma warning disable CS8600
                    Product selectedProduct = Products.Find(product => product.ID == SelectedProductId);
#pragma warning restore CS8600

                    if (selectedProduct != null)
                    {
                        Console.WriteLine($"Selected product: {selectedProduct.Name}");
                        Console.Write("Enter amount: ");
                        int wantToAddAmount = ReadInt();

                        if (wantToAddAmount <= selectedProduct.AvailableAmount)
                        {

                            if (wantToAddAmount != 0)
                            {

#pragma warning disable CS8600
                                Product ItemInCart = Cart.Find(item => item.ID == selectedProduct.ID);
#pragma warning restore CS8600

                                if (ItemInCart != null)
                                {
                                    ItemInCart.AvailableAmount += wantToAddAmount;
                                }
                                else
                                {

                                    Cart.Add(new Product(selectedProduct.ID, selectedProduct.Category, selectedProduct.Name, selectedProduct.UnitPrice, wantToAddAmount));
                                }

                                selectedProduct.AvailableAmount -= wantToAddAmount;

                                Console.WriteLine($"{wantToAddAmount} pieces of {selectedProduct.Name} has been added to cart successful.");
                            }
                            else
                            {
                                Console.WriteLine("Wrong value.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("There are not enough available products.");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Wrong ID.");
                    }

                    Console.Write("Enter ID (0 to finish):");

                    SelectedProductId = ReadInt();

                }

                if (Cart.Count != 0)
                {
                    TotalPrice = Product.GetTotalPrice(Cart);

                    Product.ShowProductsCart(Cart, "[  YOUR CART  ]");
                    Console.WriteLine($"Check your cart list. Is everything correct? Total (pre-tax) price: {TotalPrice:0.00}");
                    Console.WriteLine();
                    Console.Write("Type 'yes' to confirm and generate invoice or 'no' to clear your cart and start again: ");

                    string? ans = ReadString("Answer");

                    bool IsAnswerCorrect = false;
                    while (!IsAnswerCorrect)
                    {
                        if (ans == "yes" || ans == "y")
                        {

                            Console.WriteLine("Proceeding...");
                            Thread.Sleep(2000);
                            IsShopListCorrect = true;
                            IsAnswerCorrect = true;

                        }
                        else if (ans == "no" || ans == "n")
                        {

                            Console.WriteLine("Clearing your cart...");
                            Thread.Sleep(2000);
                            Products.Clear();
                            Product.GetProducts(Products);
                            Cart.Clear();
                            IsShopListCorrect = false;
                            IsAnswerCorrect = true;
                        }
                        else
                        {
                            Console.WriteLine("Please type 'yes' or 'no'.");
                            ans = ReadString("Answer");
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Your cart is empty. You cannot proceed with no products selected. \nPress any key to check our products list.");
                    IsShopListCorrect = false;
                    Console.ReadKey();
                }

            }

            if (EnteredLogin == "guest")
            {
                Console.WriteLine("Currently you are logged in as guest.");
                Console.WriteLine("If you want to finish shopping and generate invoice you have to enter your data.");
                Console.WriteLine("Do you want to do it now?");
                string ans = ReadString("Answer");
                bool IsAnswerCorrect = false;
                while (!IsAnswerCorrect)
                {
                    if (ans == "yes" || ans == "y")
                    {
                        Console.Write("Enter your name: ");
                        CurrentName = ReadString("Name");

                        Console.Write("Enter your surname: ");
                        CurrentSurname = ReadString("Surname");

                    }
                    else if (ans == "no" || ans == "n")
                    {
                        Console.WriteLine("Thank you for visiting us.");
                        Exit();
                        return;

                    }
                    else
                    {
                        Console.WriteLine("Please typy 'yes' or 'no'.");
                        ans = ReadString("Answer");
                    }
                }

            }

            Client CurrentClient = new(EnteredLogin, CurrentName, CurrentSurname, CurrentPassword);
            Console.WriteLine($"We are generating invoice now...");
            Thread.Sleep(2000);


            string InvName = "INV_" + currentDateString + "_" + EnteredLogin + "_" + ActiveInvoiceID;

            string PathInvoice = AppDomain.CurrentDomain.BaseDirectory + @$"\Invoices\{InvName}.txt";

            FileStream fs = File.Create(PathInvoice);
            fs.Close();

            using (StreamWriter sw = new(PathInvoice))
            {

                sw.WriteLine($"Invoice: {InvName}");
                sw.WriteLine($"Date: {currentDateString}");
                sw.WriteLine($"Name: {CurrentName}");
                sw.WriteLine($"Surname: {CurrentSurname}");
                sw.WriteLine($"Login: {EnteredLogin}");
                sw.WriteLine();
                sw.WriteLine($"[PRODUCT LIST]");
                sw.WriteLine($"{"ID",-10} {"CATEGORY",-14} {"NAME",-50} {"PRICE",10} {"AMOUNT",10} {"TOTAL",12}");
                sw.WriteLine("===============================================================================================================");
                foreach (Product product in Cart)
                {
                    sw.WriteLine(product.ToStringCart());
                }
                sw.WriteLine();
                sw.WriteLine($"Subtotal: {TotalPrice:0.00} USD");
                sw.WriteLine($"Tax 9.0%: {TotalPrice * 0.09:0.00} USD");
                sw.WriteLine($"Total:    {TotalPrice + TotalPrice * 0.09:0.00} USD");
                sw.WriteLine();
                sw.WriteLine("The administrator of personal data is the designated representative of the Jacob Digital Enterprise.");
            }

            Invoices.Add(new Invoice(int.Parse(ActiveInvoiceID), EnteredLogin, TotalPrice));

            Console.WriteLine();
            Console.WriteLine($"Saved to file : {InvName}.txt");

            Console.WriteLine("If everything is correct, please go to see one of our assistant who will help you finish the transaction.");
            Console.WriteLine("Thanks for visiting Jacob Digital Enterprise.");

            Client.SaveClients(Clients);
            Product.SaveProducts(Products);
            Invoice.SaveInvoices(Invoices);

        }
    }
}