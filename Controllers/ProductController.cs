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
        /// <summary>
        /// make list of all products that are not deleted and sort
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<Product> products = context.Products.Where(c => c.IsDeleted == false).ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            products = products.OrderByDescending(c => c.UnitPrice).ToList();
                        else
                            products = products.OrderBy(c => c.UnitPrice).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            products = products.OrderByDescending(c => c.OnHandQuantity).ToList();
                        else
                            products = products.OrderBy(c => c.OnHandQuantity).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            products = products.OrderByDescending(c => c.Description).ToList();
                        else
                            products = products.OrderBy(c => c.Description).ToList();
                        break;
                    }
                case 0:
                default:
                    if (isDesc)
                        products = products.OrderByDescending(c => c.ProductCode).ToList();
                    else
                        products = products.OrderBy(c => c.ProductCode).ToList();
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

        /// <summary>
        /// add or update product get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// add or update product post
        /// </summary>
        /// <param name="newProduct"></param>
        /// <returns></returns>
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

        /// <summary>
        /// delete product get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();
            Product product = context.Products.Where(c => c.ProductCode == id).FirstOrDefault();
            return View(product);
        }


        /// <summary>
        /// delete product post
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Product product)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Products.Where(c => c.ProductCode == product.ProductCode).Count() > 0)
                {
                    Product productDelete = context.Products.Where(c => c.ProductCode == product.ProductCode).FirstOrDefault();
                    productDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("All");
        }
    }
}