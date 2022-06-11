using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3_Morton.Models
{
    public class UpsertInvoiceModel
    {
        public List<Customer> Customers { get; set; }

        public Invoice Invoice { get; set; }
    }
}