using Project3_Morton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Morton.Controllers
{
    public class StateController : Controller
    {
        // GET: State
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<State> states;
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            states = context.States.OrderByDescending(c => c.StateName).ToList();
                        else
                            states = context.States.OrderBy(c => c.StateName).ToList();
                        break;
                    }
                default:
                    if (isDesc)
                        states = context.States.OrderByDescending(c => c.StateCode).ToList();
                    else
                        states = context.States.OrderBy(c => c.StateCode).ToList();
                    break;
            }
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                states = states.Where(c =>
                         c.StateName.ToLower().Contains(id) ||
                         c.StateCode.ToLower().Contains(id)
                         
                    ).ToList();
            }
            return View(states);
        }


        [HttpGet]
        public ActionResult Upsert(string id)
        {
            BooksEntities context = new BooksEntities();
            State states = context.States.Where(c => c.StateCode == id).FirstOrDefault();

            if (states == null)
            {
                states = new State();
            }
            return View(states);
        }


        [HttpPost]
        public ActionResult Upsert(State newState)
        {
            BooksEntities context = new BooksEntities();


            if (context.States.Where(c => c.StateCode == newState.StateCode).Count() > 0)
            {
                var stateToSave = context.States.Where(c => c.StateCode == newState.StateCode).FirstOrDefault();

                stateToSave.StateName = newState.StateName;
               
            }
            else
            {
                context.States.Add(newState);
            }

            context.SaveChanges();
            return RedirectToAction("All");
        }
    }
}