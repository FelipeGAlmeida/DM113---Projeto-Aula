﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ProductsClient.ProductsService;


namespace ProductsClient {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Press ENTER when the service has started");
            Console.ReadLine();

            Console.WriteLine("Iniciando....");

            //ProductsServiceClient proxy = new ProductsServiceClient("BasicHttpBinding_IProductsService");
            ProductsServiceClient proxy = new ProductsServiceClient("WS2007HttpBinding_IProductsService");


            // Test the operations in the service
            // Obtain a list of all products
            Console.WriteLine("Test 1: List all products");
            List<ProductData> products = proxy.ListMatchingProducts("Prod").ToList();
            foreach (ProductData p in products) {
                Console.WriteLine("Name: {0}", p.Name);
                Console.WriteLine("Code: {0}", p.Code);
                Console.WriteLine("Price: {0}", p.Price);
                Console.WriteLine();
            }
            Console.WriteLine();

            // Get details of this product
            Console.WriteLine("Test 2: Display the details of a product");
            ProductData product = proxy.GetProduct("0001");
            Console.WriteLine("Name: {0}", product.Name);
            Console.WriteLine("Code: {0}", product.Code);
            Console.WriteLine("Price: {0}", product.Price);
            Console.WriteLine();

            // Get details of this product
            Console.WriteLine("Test 3: Display the details of a product that does not exists");
            ProductData product1 = proxy.GetProduct("123");
            if (product1 != null) {
                Console.WriteLine("Name: {0}", product1.Name);
                Console.WriteLine("Code: {0}", product1.Code);
                Console.WriteLine("Price: {0}", product1.Price);
                Console.WriteLine();
            } else {
                Console.WriteLine("No such product");
                Console.WriteLine();
            }

            // Query the stock of this product
            Console.WriteLine("Test 4: Display stock of a product");
            int quantity = proxy.CurrentStock("0001");
            Console.WriteLine("Current stock: {0}", quantity);
            Console.WriteLine();

            // Add stock of this product
            Console.WriteLine("Test 5: Add stock for a product");
            if (proxy.AddStock("0001", 100)) {
                quantity = proxy.CurrentStock("0001");
                Console.WriteLine("Stock changed. Current stock: {0}", quantity);
            } else {
                Console.WriteLine("Stock update failed");
            }
            Console.WriteLine();

            // Disconnect from the service
            proxy.Close();
            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();

        }
    }
}
