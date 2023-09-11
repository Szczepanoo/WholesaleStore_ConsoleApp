using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholesaleStore_ConsoleApp
{


    internal class Invoice
    {
        static readonly string PathInvoices = AppDomain.CurrentDomain.BaseDirectory + @"\Invoices\Invoices.csv";
        public int InvoiceID { get; set; }
        public string ClientLogin { get; set; }
        public double TotalPrice { get; set; }

        public Invoice(int invoiceID, string clientLogin, double totalPrice)
        {
            InvoiceID = invoiceID;
            ClientLogin = clientLogin;
            TotalPrice = totalPrice;
        }

        public string ToStringCSV()
        {
            return $"{InvoiceID};{ClientLogin};{TotalPrice}";
        }

        public static void GetInvoices(List<Invoice> invoices)
        {

            using StreamReader sr = new(PathInvoices);

            while (!sr.EndOfStream)
            {

                string? line = sr.ReadLine();
                if (line != null)
                {
                    string[] dane = line.Split(';');
                    Invoice i = new(int.Parse(dane[0]), dane[1], double.Parse(dane[2]));
                    invoices.Add(i);
                }
                else
                {
                    Console.WriteLine("Unknown error.");
                    Program.Exit();
                    return;
                }
            }
        }
        public static void SaveInvoices(List<Invoice> invoices)
        {
            using StreamWriter sw = new(PathInvoices);
            foreach (Invoice i in invoices)
            {
                sw.WriteLine(i.ToStringCSV());
            }
        }

        public static string GetInvoiceID(List<Invoice> invoices)
        {
            int max = 0;

            foreach (Invoice i in invoices)
            {
                if (i.InvoiceID > max)
                {
                    max = i.InvoiceID;
                }
            }
            max += 1;

            return max.ToString();
        }

    }
}
