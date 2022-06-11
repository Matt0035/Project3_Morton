using Project3_Morton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Morton.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<Product> products;
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            products = context.Products.OrderByDescending(c => c.UnitPrice).ToList();
                        else
                            products = context.Products.OrderBy(c => c.UnitPrice).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            products = context.Products.OrderByDescending(c => c.OnHandQuantity).ToList();
                        else
                            products = context.Products.OrderBy(c => c.OnHandQuantity).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            products = context.Products.OrderByDescending(c => c.Description).ToList();
                        else
                            products = context.Products.OrderBy(c => c.Description).ToList();
                        break;
                    }
                case 0:
                default:
                    if (isDesc)
                        products = context.Products.OrderByDescending(c => c.ProductCode).ToList();
                    else
                        products = context.Products.OrderBy(c => c.ProductCode).ToList();
                    break;
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                products = products.Where(c =>
                         c.ProductCode.ToLower().Contains(id) ||
                         c.Description.ToLower().Contains(id)

                    ).ToList();
            }
            return View(products);
        }


        [HttpGet]
        public ActionResult Upsert(string id)
        {
            BooksEntities context = new BooksEntities();
            Product product = context.Products.Where(c => c.ProductCode == id).FirstOrDefault();

            if (product == null)
            {
                product = new Product();
            }
            return View(product);
        }


        [HttpPost]
        public ActionResult Upsert(Product newProduct)
        {
            BooksEntities context = new BooksEntities();


            if (context.Products.Where(c => c.ProductCode == newProduct.ProductCode).Count() > 0)
            {
                var productToSave = context.Products.Where(c => c.ProductCode == newProduct.ProductCode).FirstOrDefault();

                productToSave.Description = newProduct.Description;
                productToSave.OnHandQuantity = newProduct.OnHandQuantity;
                productToSave.UnitPrice = newProduct.UnitPrice;
            }
            else
            {
                context.Products.Add(newProduct);
            }

            context.SaveChanges();
            return RedirectToAction("All");
        }


    }
}