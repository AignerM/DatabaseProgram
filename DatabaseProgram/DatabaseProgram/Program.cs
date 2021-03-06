﻿using System;
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

                default:
                    Console.WriteLine("Please instert the customer you are searching for.");
                    string customer = Console.ReadLine();
                    SearchCustomer(customer);
                    break;
            }
        }

        static void SearchCustomer(string customer)
        {
            bool found = false;
            using (NorthwindContext db = new NorthwindContext())
            {
                var searchcustomer = db.Customers
                                .Where(x => x.CompanyName.Contains(customer) || x.ContactName.Contains(customer));
                StringBuilder result = new StringBuilder();

                foreach (var custom in searchcustomer)
                {
                    result.AppendFormat("{0}\t{1}\t{2}\n", custom.CustomerID, custom.CompanyName, custom.ContactName);
                    found = true;
                }
                if (found)
                {
                    ConsoleOutput(result, 1);
                }
                else
                {
                    Console.WriteLine("No customer was found. Returning to Menu");
                    Menu(0);
                }
            }
        }

        static void SearchOrders(string id)
        {
            bool found = false;
            using (NorthwindContext db = new NorthwindContext())
            {
                var foundOrders = db.Orders
                                .Where(x => x.CustomerID == id);
                StringBuilder result = new StringBuilder();
                foreach (var item in foundOrders)
                {
                    double costall = 0.0, cost = 0.0, quantity = 0.0, discount = 0.0, temp = 0.0;
                    foreach (var order in item.Order_Details)
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
                    found = true;
                }
                if (found)
                {
                    ConsoleOutput(result, 2);
                }
                else
                {
                    Console.WriteLine("No customer was found. Enter a valid Customer ID");
                    Menu(1);
                }
            }
        }

        static void SearchOrderDetails(int id)
        {
            bool found = false;
            using(NorthwindContext db=new NorthwindContext())
            {
                var order = db.Order_Details
                                    .Where(x => x.OrderID == id && x.Product.ProductID == x.ProductID);
                                    
                                                                    
                string productName = "";
                StringBuilder result = new StringBuilder();
                foreach (var item in order)
                {
                    productName = item.Product.ProductName;
                    result.AppendFormat("{0}\t{1}\t{2:0.00}\n", productName, item.Quantity, item.UnitPrice);
                    found = true;
                }
                if (found)
                {
                    ConsoleOutput(result, 2);
                }
                else
                {
                    Console.WriteLine("No customer was found. Enter a valid Order ID");
                    Menu(2);
                }
            }
        }

        static void ConsoleOutput(StringBuilder result, int step)
        {
            Console.WriteLine(result);
            Menu(step);
        }
    }
}
