using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project3_Morton.Models
{
    public class UpsertItemsModel
    {

        public InvoiceLineItem InvoiceLineItem { get; set; }

        public List<Product> Products { get; set; }
    }
}