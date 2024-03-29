﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ProductsEntityModel;
using System.ServiceModel.Activation;

namespace Products {

    // WCF service that implements the service contract
    // This implementation performs minimal error checking and exception handling
    [AspNetCompatibilityRequirements(
    RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // OBSERVAÇÃO: Você pode usar o comando "Renomear" no menu "Refatorar" para alterar o nome da classe "Service1" no arquivo de código, svc e configuração ao mesmo tempo.
    // OBSERVAÇÃO: Para iniciar o cliente de teste do WCF para testar esse serviço, selecione Service1.svc ou Service1.svc.cs no Gerenciador de Soluções e inicie a depuração.
    public class ProductsService : IProductsService, IProductsServiceV2 {

        public List<ProductData> ListProducts() {
            return ListMatchingProducts("");
        }

        public List<ProductData> ListMatchingProducts(string match) {
            // Create a list of products
            List<ProductData> productsList = new List<ProductData>();
            try {
                // Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel()) {
                    // Fetch the products in the database
                    List<Product> products = (from product in database.Products
                                              where product.Name.Contains(match)
                                              select product).ToList();
                    foreach (Product product in products) {
                        ProductData productData = new ProductData() {
                            Name = product.Name,
                            Code = product.Code,
                            Price = product.Price
                        };
                        productsList.Add(productData);
                    }
                }
            } catch {
                // Ignore exceptions in this implementation
            }
            // Return the list of products
            return productsList;
        }

        public ProductData GetProduct(string productCode) {
            ProductData productData = null;
            try {
                // Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel()) {
                    // Find the first product that matches the specified product code
                    if (ProductExists(productCode, database)) {
                        Product matchingProduct = database.Products.First(p =>
                        String.Compare(p.Code, productCode) == 0);
                        productData = new ProductData() {
                            Name = matchingProduct.Name,
                            Code = matchingProduct.Code,
                            Price = matchingProduct.Price
                        };
                    }
                }
            } catch {
                // Ignore exceptions in this implementation
            }
            // Return the product
            return productData;
        }

        public bool ProductExists(string productCode, ProductsModel database) {
            // Check to see whether the specified product exists in the database
            int numProducts = (from p in database.Products
                               where string.Equals(p.Code, productCode)
                               select p).Count();
            return numProducts > 0;
        }


        public int CurrentStock(string productCode) {
            int quantityTotal = 0;
            try {
                // Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel()) {
                    if (ProductExists(productCode, database)) {
                        // Calculate the sum of all quantities for the specified product
                         quantityTotal = (from s in database.Stocks
                                          join p in database.Products on s.Product.Id
                                          equals p.Id
                                          where String.Compare(p.Code, productCode) == 0
                                          select (int)s.Quantity).Sum();
                    }
                }
            } catch {
                // Ignore exceptions in this implementation
            }
            // Return the stock level
            return quantityTotal;
        }

        public bool AddStock(string productCode, decimal quantity) {
            try {
                // Connect to the ProductsModel database
                using (ProductsModel database = new ProductsModel()) {
                    if (!ProductExists(productCode, database))
                        return false;
                    else {
                        // Find the ProductID for the specified product
                        int productID = (from p in database.Products
                                         where String.Compare(p.Code, productCode) == 0
                                         select p.Id).First();
                        // Find the Stock object that matches the parameters passed
                        // in to the operation
                        Stock stock = database.Stocks.First(pi => pi.Id == productID);
                        stock.Quantity = quantity;
                        stock.LastUpdate = DateTime.Now;
                        database.Stocks.Add(stock);
                        database.SaveChanges();
                    }

                }
            } catch {
                // If an exception occurs, return false to indicate failure
                return false;
            }
            // Return true to indicate success
            return true;
        }


    }
}
