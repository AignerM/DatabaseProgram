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
            using (NorthwindContext db = new NorthwindContext())
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
                                Console.ReadKey();
                            }
                            break;

                        case "2":
                            Console.WriteLine("Please insert the customer ID you are looking for.");
                            try
                            {
                                
                                string customerID = Console.ReadLine();
                                do
                                {
                                    Console.Clear();
                                    var searchID = db.Orders
                                    .Where(x => x.CustomerID == customerID);
                                    StringBuilder result = new StringBuilder();
                                    foreach (var item in searchID)
                                    {
                                        double costall = 0.0, cost = 0.0, quantity = 0.0, discount = 0.0, temp = 0.0;
                                        var price = db.Order_Details
                                            .Where(x => x.OrderID == item.OrderID);
                                        foreach (var id in price)
                                        {
                                            cost = Convert.ToDouble(id.UnitPrice);
                                            quantity = Convert.ToDouble(id.Quantity);
                                            discount = Convert.ToDouble(id.Discount);
                                            if (discount > 0)
                                            {
                                                temp = cost * quantity;
                                                temp = temp - (temp * discount);
                                                costall += temp;
                                                temp = 0;
                                            }
                                            else
                                            {
                                                costall += cost * quantity;
                                            }
                                        }
                                        result.AppendFormat("{0}\t{1}\t{2}\t{3:0.00}\n", item.OrderID, item.ShipName, item.ShipAddress, costall);
                                        Console.WriteLine(result);
                                        result.Clear();
                                    }

                                    Console.WriteLine("Enter the orderID, to see the order details:");
                                    var orderID = Convert.ToInt32(Console.ReadLine());
                                    var order = db.Order_Details
                                        .Where(x => x.OrderID == orderID);
                                    string product = "";
                                    foreach (var item in order)
                                    {
                                        var productID = db.Products
                                            .Where(x => x.ProductID == item.ProductID);
                                        foreach (var id in productID)
                                        {
                                            product = id.ProductName;
                                        }
                                        result.AppendFormat("{0}\t{1}\t{2:0.00}", product, item.Quantity, item.UnitPrice);
                                        Console.WriteLine(result);
                                        result.Clear();
                                    }
                                    Console.WriteLine("If you don't want to search for an other order press ENTER, else press J");
                                } while (Console.ReadLine() != "");
                            }
                            catch
                            {
                                Console.WriteLine("The ID you entert is not valid!");
                                Console.ReadKey();
                            }
                            break;

                        default:
                            Console.WriteLine("Please enter a valid menu option!");
                            Console.ReadKey();
                            break;
                    }
                    Console.Clear();
                } while (true);

            }
        }
    }
}
