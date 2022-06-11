using Project3_Morton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Morton.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        /// <summary>
        /// get list of all invoices that are not deleted and sort
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<Invoice> invoice = context.Invoices.Where(c => c.IsDeleted == false).ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            invoice = invoice.OrderByDescending(i => i.CustomerID).ToList();
                        else
                            invoice = invoice.OrderBy(i => i.CustomerID).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            invoice = invoice.OrderByDescending(i => i.InvoiceDate).ToList();
                        else
                            invoice = invoice.OrderBy(i => i.InvoiceDate).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            invoice = invoice.OrderByDescending(i => i.InvoiceTotal).ToList();
                        else
                            invoice = invoice.OrderBy(i => i.InvoiceTotal).ToList();
                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                            invoice = invoice.OrderByDescending(i => i.InvoiceTotal).ToList();
                        else
                            invoice = invoice.OrderBy(i => i.InvoiceTotal).ToList();
                        break;
                    }
                case 5:
                    {
                        if (isDesc)
                            invoice = invoice.OrderByDescending(i => i.ProductTotal).ToList();
                        else
                            invoice = invoice.OrderBy(i => i.ProductTotal).ToList();
                        break;
                    }
                case 6:
                    {
                        if (isDesc)
                            invoice = invoice.OrderByDescending(i => i.SalesTax).ToList();
                        else
                            invoice = invoice.OrderBy(i => i.SalesTax).ToList();
                        break;
                    }
                case 7:
                    {
                        if (isDesc)
                            invoice = invoice.OrderByDescending(i => i.Shipping).ToList();
                        else
                            invoice = invoice.OrderBy(i => i.Shipping).ToList();
                        break;
                    }

                case 0:
                default:
                    if (isDesc)
                        invoice = invoice.OrderByDescending(i => i.InvoiceID).ToList();
                    else
                        invoice = invoice.OrderBy(i => i.InvoiceID).ToList();
                    break;
            }
            return View(invoice);
        }

        /// <summary>
        /// add or update invoice get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            BooksEntities context = new BooksEntities();
            Invoice invoice = context.Invoices.Where(i => i.InvoiceID == id).FirstOrDefault();
            
            List<Customer> customers = context.Customers.ToList();
            UpsertInvoiceModel viewModel = new UpsertInvoiceModel()
            {
                Customers = customers,
                Invoice = invoice
            };

            return View(viewModel);
        }

        /// <summary>
        /// add or update invoice post
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(UpsertInvoiceModel model, string customerID)
        {
            Invoice newInvoice = model.Invoice;
            customerID = customerID.Split(',')[0];
            newInvoice.CustomerID = Int32.Parse(customerID);
            BooksEntities context = new BooksEntities();


            if (context.Invoices.Where(i => i.InvoiceID == newInvoice.InvoiceID).Count() > 0)
            {
                var invoiceToSave = context.Invoices.Where(i => i.InvoiceID == newInvoice.InvoiceID).FirstOrDefault();

                invoiceToSave.InvoiceDate = newInvoice.InvoiceDate;
                invoiceToSave.InvoiceTotal = newInvoice.InvoiceTotal;
                invoiceToSave.ProductTotal = newInvoice.ProductTotal;
                invoiceToSave.SalesTax = newInvoice.SalesTax;
                invoiceToSave.Shipping = newInvoice.Shipping;
                invoiceToSave.CustomerID = newInvoice.CustomerID;
            }
            else
            {
                context.Invoices.Add(newInvoice);
            }

            context.SaveChanges();
            return RedirectToAction("All");
        }

        /// <summary>
        /// delete invoice get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            BooksEntities context = new BooksEntities();
            Invoice invoice = context.Invoices.Where(c => c.InvoiceID == id).FirstOrDefault();
            return View(invoice);
        }


        /// <summary>
        /// delete invoice post
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Invoice invoice)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Invoices.Where(c => c.InvoiceID == invoice.InvoiceID).Count() > 0)
                {
                    Invoice invoiceDelete = context.Invoices.Where(c => c.InvoiceID == invoice.InvoiceID).FirstOrDefault();
                    invoiceDelete.IsDeleted = true;
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