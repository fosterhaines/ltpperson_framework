using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LTPPerson_Framework.Models;

namespace LTPPerson_Framework.Controllers
{
    public class peopleController : Controller
    {
        private LTPEntities db = new LTPEntities();

        // GET: people
        //public async Task<ActionResult> Index()
        //{
        //    var people = db.people.Include(p => p.state);
        //    return View(await people.ToListAsync());
        //}

        // GET: people
        public async Task<ActionResult> Index(string searchString)
        {
            var people = db.people.Include(p => p.state);
            if (!String.IsNullOrEmpty(searchString))
            {
                
                var searchResult = db.uspPersonSearch(null, null, searchString, null, null, null);
                people = people.Where(s => searchResult.Select(u => u.person_id).Contains(s.person_id));
            }
            return View(await people.ToListAsync());
        }

        // GET: people/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = await db.people.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: people/Create
        public ActionResult Create()
        {
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code");
            return View();
        }

        // POST: people/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "person_id,first_name,last_name,state_id,gender,dob")] person person)
        {
            if (ModelState.IsValid)
            {
                db.people.Add(person);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code", person.state_id);
            return View(person);
        }

        // GET: people/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = await db.people.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code", person.state_id);
            return View(person);
        }

        // POST: people/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "person_id,first_name,last_name,state_id,gender,dob")] person person)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(person).State = EntityState.Modified;
                db.uspPersonUpsert(person.person_id, person.first_name, person.last_name, person.state_id, person.gender, person.dob);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code", person.state_id);
            return View(person);
        }

        // GET: people/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = await db.people.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: people/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            person person = await db.people.FindAsync(id);
            db.people.Remove(person);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // GET: people/Create
        public ActionResult _Create()
        {
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code");
            return PartialView("_Create");
        }

        // POST: people/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Create([Bind(Include = "person_id,first_name,last_name,state_id,gender,dob")] person person)
        {
            if (ModelState.IsValid)
            {
                db.uspPersonUpsert(person.person_id, person.first_name, person.last_name, person.state_id, person.gender, person.dob);
                //db.people.Add(person);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code", person.state_id);
            return PartialView(person);
        }

        // GET: people/Edit/5
        public async Task<ActionResult> _Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person person = await db.people.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code", person.state_id);
            return PartialView("_Edit",person);
        }

        // POST: people/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _Edit([Bind(Include = "person_id,first_name,last_name,state_id,gender,dob")] person person)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(person).State = EntityState.Modified;
                db.uspPersonUpsert(person.person_id, person.first_name, person.last_name, person.state_id, person.gender, person.dob);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.state_id = new SelectList(db.states, "state_id", "state_code", person.state_id);
            return PartialView(person);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
