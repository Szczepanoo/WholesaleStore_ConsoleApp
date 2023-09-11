using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleStore_ConsoleApp
{

    enum ProductCategory
    {
        Electronics,
        Sport,
        Beauty,
        Automotive,
        Appliances,
        Clothing,
        Garden,
        Home,

    }
    internal class Product
    {
        static readonly string PathProducts = AppDomain.CurrentDomain.BaseDirectory + "Products.csv";
        static readonly string Dash = "==================================================================================================";
        static readonly string CartDash = "===============================================================================================================";
        public int ID { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
        public int AvailableAmount { get; set; }
        public ProductCategory Category { get; set; }

        public Product(int id, ProductCategory category, string name, double price, int amount)
        {
            ID = id;
            Category = category;
            Name = name;
            UnitPrice = price;
            AvailableAmount = amount;
        }

        public override string ToString()
        {
            return $"{ID,-10} {Category,-14} {Name,-50} {UnitPrice,10:0.00} {AvailableAmount,10} ";
        }

        public string ToStringCart()
        {
            return ToString() + $"{UnitPrice * AvailableAmount,12:0.00}";
        }

        public string ToStringCSV()
        {
            return $"{ID};{Category};{Name};{UnitPrice};{AvailableAmount}";
        }

        public static void GetProducts(List<Product> Products)
        {

            using StreamReader sr = new(PathProducts);

            while (!sr.EndOfStream)
            {

                string? line = sr.ReadLine();
                if (line != null)
                {
                    string[] dane = line.Split(';');
                    Product p = new(int.Parse(dane[0]), (ProductCategory)Enum.Parse(typeof(ProductCategory), dane[1]), dane[2], double.Parse(dane[3]), int.Parse(dane[4]));
                    Products.Add(p);
                }
                else
                {
                    Console.WriteLine("Unknown error.");
                    Program.Exit();
                    return;
                }
            }
        }

        public static void ShowProductsCart(List<Product> products, string name)
        {

            Console.WriteLine(name);
            Console.WriteLine($"{"ID",-10} {"CATEGORY",-14} {"NAME",-50} {"PRICE",10} {"AMOUNT",10} {"TOTAL",12}");
            Console.WriteLine(CartDash);
            foreach (Product product in products)
            {
                Console.WriteLine(product.ToStringCart());
            }
            Console.WriteLine(CartDash);

        }
        public static void ShowProducts(List<Product> products, string name)
        {

            var sequence = products.OrderBy(obj => obj.ID).GroupBy(obj => obj.Category);

            Console.WriteLine(name);
            Console.WriteLine($"{"ID",-10} {"CATEGORY",-14} {"NAME",-50} {"PRICE",10} {"AMOUNT",10} ");
            Console.WriteLine(Dash);
            foreach (var group in sequence)
            {

                Console.WriteLine($"[{group.Key}]");

                foreach (Product product in group)
                {
                    Console.WriteLine(product.ToString());
                }
                Console.WriteLine(Dash);
            }
            Console.WriteLine("Curernt list presents pre-tax prices.");

        }
        public static double GetTotalPrice(List<Product> products)
        {
            double Total = 0;

            foreach (Product p in products)
            {
                Total += p.UnitPrice * p.AvailableAmount;
            }
            return Total;
        }

        public static void SaveProducts(List<Product> products)
        {
            using StreamWriter sw = new(PathProducts);
            foreach (Product p in products)
            {
                sw.WriteLine(p.ToStringCSV());
            }
        }

    }
}
