using Project3_Morton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Morton.Controllers
{
    public class InvoiceLineItemController : Controller
    {
        // GET: InvoiceLineItem'
        /// <summary>
        /// get all invoice line items and sort
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<InvoiceLineItem> invoiceLineItems;
           

            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.ProductCode).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.ProductCode).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.Quantity).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.Quantity).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.ItemTotal).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.ItemTotal).ToList();
                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.UnitPrice).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.UnitPrice).ToList();
                        break;
                    }         
                case 0:
                default:
                    if (isDesc)
                        invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.InvoiceID).ToList();
                    else
                        invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.InvoiceID).ToList();
                    break;
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                invoiceLineItems = invoiceLineItems.Where(c =>
                         c.ProductCode.ToLower().Contains(id) 
                    ).ToList();
            }
            return View(invoiceLineItems);
        }

        /// <summary>
        /// add or delete lineitem get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            BooksEntities context = new BooksEntities();
            InvoiceLineItem invoiceLineItems = context.InvoiceLineItems.Where(i => i.InvoiceID == id).FirstOrDefault();
            List<Product> products = context.Products.ToList();

            UpsertItemsModel viewModel = new UpsertItemsModel()
            {
                InvoiceLineItem = invoiceLineItems,
                Products = products
            };

            return View(viewModel);
        }

        /// <summary>
        /// add or delete line items post
        /// </summary>
        /// <param name="model"></param>
        /// <param name="productcode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(UpsertItemsModel model, string productcode)
        {
            InvoiceLineItem newInvoiceLineItem = model.InvoiceLineItem;
            newInvoiceLineItem.ProductCode = productcode;
            BooksEntities context = new BooksEntities();


            if (context.InvoiceLineItems.Where(i => i.InvoiceID == newInvoiceLineItem.InvoiceID).Count() > 0)
            {
                var invoiceLineItemToSave = context.InvoiceLineItems.Where(i => i.InvoiceID == newInvoiceLineItem.InvoiceID).FirstOrDefault();

                invoiceLineItemToSave.ItemTotal = newInvoiceLineItem.ItemTotal;
                invoiceLineItemToSave.ProductCode = newInvoiceLineItem.ProductCode;
                invoiceLineItemToSave.Quantity = newInvoiceLineItem.Quantity;
                invoiceLineItemToSave.UnitPrice = newInvoiceLineItem.UnitPrice;
            }
            else
            {
                context.InvoiceLineItems.Add(newInvoiceLineItem);
            }

            context.SaveChanges();
            return RedirectToAction("All");
        }
    }
}