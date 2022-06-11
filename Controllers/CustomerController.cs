using Project3_Morton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Morton.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer

        /// <summary>
        /// get all customers that are not deleted and sort
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<Customer> customers = context.Customers.Where(c => c.IsDeleted == false).ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            customers = customers.OrderByDescending(c => c.Name).ToList();
                        else
                            customers = customers.OrderBy(c => c.Name).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            customers = customers.OrderByDescending(c => c.Address).ToList();
                        else
                            customers = customers.OrderBy(c => c.Address).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            customers = customers.OrderByDescending(c => c.City).ToList();
                        else
                            customers = customers.OrderBy(c => c.City).ToList();
                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                            customers = customers.OrderByDescending(c => c.State).ToList();
                        else
                            customers = customers.OrderBy(c => c.State).ToList();
                        break;
                    }
                case 5:
                    {
                        if (isDesc)
                            customers = customers.OrderByDescending(c => c.ZipCode).ToList();
                        else
                            customers = customers.OrderBy(c => c.ZipCode).ToList();
                        break;
                    }

                case 0:
                default:
                    if (isDesc)
                        customers = customers.OrderByDescending(c => c.CustomerID).ToList();
                    else
                        customers = customers.OrderBy(c => c.CustomerID).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                customers = customers.Where(c =>
                         c.Name.ToLower().Contains(id) ||
                         c.Address.ToLower().Contains(id) ||
                         c.City.ToLower().Contains(id) ||
                         c.State.ToLower().Contains(id) ||
                         c.ZipCode.ToLower().Contains(id)
                    ).ToList();
            }
            return View(customers);
        }

        /// <summary>
        /// adding or updating a customer get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            BooksEntities context = new BooksEntities();
            Customer customer = context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();

            if(customer == null)
            {
                customer = new Customer();
            }
            return View(customer);
        }


        /// <summary>
        /// adding or updating a customer post
        /// </summary>
        /// <param name="newCustomer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(Customer newCustomer)
        {
            BooksEntities context = new BooksEntities();


            if (context.Customers.Where(c => c.CustomerID == newCustomer.CustomerID).Count() > 0)
                {
                    var customerToSave = context.Customers.Where(c => c.CustomerID == newCustomer.CustomerID).FirstOrDefault();

                    customerToSave.Address = newCustomer.Address;
                    customerToSave.Name = newCustomer.Name;
                    customerToSave.City = newCustomer.City;
                    customerToSave.State = newCustomer.State;
                    customerToSave.ZipCode = newCustomer.ZipCode;
                    customerToSave.IsDeleted = newCustomer.IsDeleted;
            }
                else
                {
                    context.Customers.Add(newCustomer);
                }

            context.SaveChanges();
            return RedirectToAction("All");
        }

        /// <summary>
        /// delete customer get 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            BooksEntities context = new BooksEntities();
            Customer customer = context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();
            return View(customer);
        }


        /// <summary>
        /// delete customer post
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Customer customer)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.Customers.Where(c => c.CustomerID == customer.CustomerID).Count() > 0)
                {
                    Customer customerDelete = context.Customers.Where(c => c.CustomerID == customer.CustomerID).FirstOrDefault();
                    customerDelete.IsDeleted = true;
                }
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("All");
        }
    }
}