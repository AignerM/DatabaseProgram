using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Database_NorthwindContext db = new Database_NorthwindContext())
            {
                do
                {
                    Console.WriteLine("1- Search by customer\n2- Search by customer ID");
                    string choise = Console.ReadLine();

                    switch (choise)
                    {
                        case "1":
                            Console.WriteLine("Please instert the customer you are searching for.");
                            try
                            {
                                string customer = Console.ReadLine();

                                var searchcustomer = db.Customers
                                    .Where(x => x.CompanyName.Contains(customer) || x.ContactName.Contains(customer));
                                StringBuilder result = new StringBuilder();

                                foreach (var item in searchcustomer)
                                {
                                    result.AppendFormat("{0}\t{1}\t{2}\n", item.CustomerID, item.CompanyName, item.ContactName);
                                    Console.WriteLine(result);
                                    result.Clear();
                                }
                            }
                            catch
                            {
                                Console.WriteLine("The customer you searched for doesn't exist!");
                            }
                            break;

                        case "2":
                            Console.WriteLine("Please insert the customer ID you are looking for.");
                            try
                            {
                                
                                string customerID = Console.ReadLine();
                                var searchID = db.Orders
                                    .Where(x => x.CustomerID == customerID);
                                StringBuilder result = new StringBuilder();
                                foreach (var item in searchID)
                                {
                                    double costall=0.0;
                                    var price = db.Order_Details
                                        .Where(x => x.OrderID == item.OrderID);
                                    foreach (var id in price)
                                    {
                                        var cost = Convert.ToDouble(id.UnitPrice);
                                        var quantity = Convert.ToDouble(id.Quantity);
                                        var discount = Convert.ToDouble(id.Discount);
                                        costall += cost * quantity - discount;
                                    }
                                    result.AppendFormat("{0}\t{1}\t{2}\t{3:0.00}\n", item.OrderID, item.ShipName, item.ShipAddress,costall);
                                    Console.WriteLine(result);
                                    result.Clear();
                                }

                                Console.WriteLine("Enter the orderID, to see the order details:");
                                string oderID = Console.ReadLine();

                            }
                            catch
                            {
                                Console.WriteLine("The ID you entert is not valid!");
                            }
                            break;

                        default:
                            Console.WriteLine("Please enter a valid menu option!");
                            break;
                    }

                    Console.ReadKey();
                    Console.Clear();
                } while (true);

            }
        }
    }
}
