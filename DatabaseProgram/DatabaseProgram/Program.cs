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
            Menu(0);
        }

        static void Menu(int step)
        {
            switch (step)
            {
                case 1:
                    Console.WriteLine("Enter the Customer ID to search for Orders or just press enter to return to Main Menu");
                    string id = Console.ReadLine();
                    if (id != "")
                    {
                        SearchOrders(id);
                    }
                    else
                    {
                        Console.Clear();
                        Menu(0);
                    }
                    break;

                case 2:
                    Console.WriteLine("Enter the Order ID to search for Detailed Orders or just press enter to return to Main Menu");
                    id = Console.ReadLine();
                    if (id != "")
                    {
                        try
                        {
                            int orderID = Convert.ToInt32(id);
                            SearchOrderDetails(orderID);
                        }
                        catch
                        {
                            Console.WriteLine("The ID you entered is not valid! \nReturning to Menu");
                            Console.ReadKey();
                            Console.Clear();
                            Menu(0);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Menu(0);
                    }
                    break;

                case 3:

                    break;

                default:
                    Console.WriteLine("Please instert the customer you are searching for.");
                    string customer = Console.ReadLine();
                    SearchCustomer(customer);
                    break;
            }
        }

        static void SearchCustomer(string customer)
        {
            using (NorthwindContext db = new NorthwindContext())
            {
                var searchcustomer = db.Customers
                                .Where(x => x.CompanyName.Contains(customer) || x.ContactName.Contains(customer));
                StringBuilder result = new StringBuilder();

                foreach (var custom in searchcustomer)
                {
                    result.AppendFormat("{0}\t{1}\t{2}\n", custom.CustomerID, custom.CompanyName, custom.ContactName);
                }
                ConsoleOutput(result,1);
            }
        }

        static void SearchOrders(string id)
        {
            using (NorthwindContext db = new NorthwindContext())
            {
                var foundOrders = db.Orders
                                .Where(x => x.CustomerID == id);
                StringBuilder result = new StringBuilder();
                foreach (var item in foundOrders)
                {
                    double costall = 0.0, cost = 0.0, quantity = 0.0, discount = 0.0, temp = 0.0;
                    var price = db.Order_Details
                        .Where(x => x.OrderID == item.OrderID);
                    foreach (var order in price)
                    {
                        cost = Convert.ToDouble(order.UnitPrice);
                        quantity = Convert.ToDouble(order.Quantity);
                        discount = Convert.ToDouble(order.Discount);
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
                }
                ConsoleOutput(result, 2);
            }
        }

        static void SearchOrderDetails(int id)
        {
            using(NorthwindContext db=new NorthwindContext())
            {
                var order = db.Order_Details
                                    .Where(x => x.OrderID == id);
                string product = "";
                StringBuilder result = new StringBuilder();
                foreach (var item in order)
                {
                    var productID = db.Products
                        .Where(x => x.ProductID == item.ProductID);
                    foreach (var vProduct in productID)
                    {
                        product = vProduct.ProductName;
                    }
                    result.AppendFormat("{0}\t{1}\t{2:0.00}\n", product, item.Quantity, item.UnitPrice);
                }
                ConsoleOutput(result, 3);
            }
        }

        static void ConsoleOutput(StringBuilder result, int step)
        {
            Console.WriteLine(result);
            Menu(step);
        }
    }
}
