using Project3_Morton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Morton.Controllers
{
    public class OrderOptionController : Controller
    {
        // GET: OrderOption
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<OrderOption> orders = context.OrderOptions.Where(c => c.IsDeleted == false).ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            orders = orders.OrderByDescending(c => c.SalesTaxRate).ToList();
                        else
                            orders = orders.OrderBy(c => c.SalesTaxRate).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            orders = orders.OrderByDescending(c => c.FirstBookShipCharge).ToList();
                        else
                            orders = orders.OrderBy(c => c.FirstBookShipCharge).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            orders =orders.OrderByDescending(c => c.FirstBookShipCharge).ToList();
                        else
                            orders = orders.OrderBy(c => c.FirstBookShipCharge).ToList();
                        break;
                    }
                case 0:
                default:
                    if (isDesc)
                        orders = orders.OrderByDescending(c => c.Id).ToList();
                    else
                        orders = orders.OrderBy(c => c.Id).ToList();
                    break;
            }
            return View(orders);
        }

        [HttpGet]
        public ActionResult Upsert(int id)
        {
            BooksEntities context = new BooksEntities();
            OrderOption orderOption = context.OrderOptions.Where(c => c.Id == id).FirstOrDefault();

            if (orderOption == null)
            {
                orderOption = new OrderOption();
            }
            return View(orderOption);
        }


        [HttpPost]
        public ActionResult Upsert(OrderOption neworderOption)
        {
            BooksEntities context = new BooksEntities();


            if (context.OrderOptions.Where(c => c.Id == neworderOption.Id).Count() > 0)
            {
                var customerToSave = context.OrderOptions.Where(c => c.Id == neworderOption.Id).FirstOrDefault();

                customerToSave.AdditionalBookShipCharge = neworderOption.AdditionalBookShipCharge;
                customerToSave.SalesTaxRate = neworderOption.SalesTaxRate;
                customerToSave.FirstBookShipCharge = neworderOption.FirstBookShipCharge;
            }
            else
            {
                context.OrderOptions.Add(neworderOption);
            }

            context.SaveChanges();
            return RedirectToAction("All");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            BooksEntities context = new BooksEntities();
            OrderOption order = context.OrderOptions.Where(c => c.Id == id).FirstOrDefault();
            return View(order);
        }


        
        [HttpPost]
        public ActionResult Delete(OrderOption order)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.OrderOptions.Where(c => c.Id == order.Id).Count() > 0)
                {
                    OrderOption orderDelete = context.OrderOptions.Where(c => c.Id == order.Id).FirstOrDefault();
                    orderDelete.IsDeleted = true;
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