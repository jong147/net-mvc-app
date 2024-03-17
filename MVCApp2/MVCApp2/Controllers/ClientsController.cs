using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCApp2.Models;
using System.Collections;

namespace MVCApp2.Controllers
{
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private DatabaseContext database;
        public ClientsController(DatabaseContext _database)
        {
            database = _database;
        }

        [HttpGet]
        [Route("/Clients/Index")]
        public async Task<ActionResult> Index()
        {
            // boxing y unboxing

            float my_float = 1.5f;

            // int my_implicit_int = my_float; no se puede

            int my_int = (int) my_float;

            object my_object = my_int;

            ArrayList my_array_list = new ArrayList();

            my_array_list.Add(my_object);

            int my_int2 = (int)my_object;

            ViewData["object"] = my_object;

            ViewData["int2"] = my_int2;

            // reflection

            int i = 42;
            Type type = i.GetType();
            Console.WriteLine(type);

            // exception handling

            divide(2.0f, 0.0f);

            return View(await database.Clients.ToListAsync());
        }

        float divide(float num1, float num2) {
            try
            {
                ViewData["divideResult"] = (num1/num2);

                if (num2 == 0)
                {
                    ViewData["divideResult"] = "Cannot divide by zero";
                }
                return num1 / num2;
            }
            catch (Exception ex)
            {
                ViewData["divideResult"] = "Error";
                return 0;
                //throw;
            }
            
        }

        [HttpGet]
        [Route("/Clients/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/Clients/Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Clients item)
        //public ActionResult Create(Clients item)
        {
            try
            {
                database.Add(item);
                await database.SaveChangesAsync();
                //database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e);
                return View(item);
            }
        }

        [HttpGet]
        [Route("/Clients/Edit")]
        public ActionResult Edit(string Id)
        {
            Clients item = (from client in database.Clients where client.Id == Id select client).FirstOrDefault()!;

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [Route("/Clients/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Clients item)
        //public ActionResult Edit(Clients item)
        {
            try
            {
                database.Update(item);
                await database.SaveChangesAsync();
                //database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e);
                return View(item);
            }
        }

        [HttpGet]
        [Route("/Clients/Delete")]
        public ActionResult Delete(string Id)
        {
            Clients item = (from client in database.Clients where client.Id == Id select client).FirstOrDefault()!;

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [Route("/Clients/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Clients item)
        {
            try
            {
                database.Remove(item);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e);
                return View(item);
            }
        }

        [HttpPost]
        [Route("/Clients/ClientDelete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ClientDelete(string id)
        {
            try
            {
                Clients item = (from client in database.Clients where client.Id == id select client).FirstOrDefault()!;

                //if (item == null)
                //{
                //    return NotFound();
                //}

                database.Remove(item);
                await database.SaveChangesAsync();
                //return RedirectToAction("Index");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return Content($"Error: " + e);
            }
        }

    }
}
