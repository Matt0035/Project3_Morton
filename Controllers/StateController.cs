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
        /// <summary>
        /// make list of all states that are no deleted and sort
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities context = new BooksEntities();
            List<State> states = context.States.Where(c => c.IsDeleted == false).ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            states = states.OrderByDescending(c => c.StateName).ToList();
                        else
                            states = states.OrderBy(c => c.StateName).ToList();
                        break;
                    }
                default:
                    if (isDesc)
                        states = states.OrderByDescending(c => c.StateCode).ToList();
                    else
                        states = states.OrderBy(c => c.StateCode).ToList();
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

        /// <summary>
        /// update or delete state get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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


        /// <summary>
        /// update or add state post
        /// </summary>
        /// <param name="newState"></param>
        /// <returns></returns>
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


        /// <summary>
        /// delete state get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();
            State state = context.States.Where(c => c.StateCode == id).FirstOrDefault();
            return View(state);
        }


        /// <summary>
        /// delete state post
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(State state)
        {
            BooksEntities context = new BooksEntities();
            try
            {
                if (context.States.Where(c => c.StateCode == state.StateCode).Count() > 0)
                {
                    State stateDelete = context.States.Where(c => c.StateCode == state.StateCode).FirstOrDefault();
                    stateDelete.IsDeleted = true;
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