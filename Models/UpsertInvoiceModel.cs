using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3_Morton.Models
{
    /// <summary>
    /// class to upsert invoices with a list of customers to use to grab customer id
    /// </summary>
    public class UpsertInvoiceModel
    {
        public List<Customer> Customers { get; set; }

        public Invoice Invoice { get; set; }
    }
}