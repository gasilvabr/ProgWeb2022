using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppProjeto01G2.Models;

namespace WebAppProjeto01G2.Controllers
{
    public class FabricantesController : Controller
    {
        private EFContext context = new EFContext();

        private static IList<Fabricante> fabricantes = new List<Fabricante>()
        {
            new Fabricante() { FabricanteId = 1, Nome = "LG"},
            new Fabricante() { FabricanteId = 2, Nome = "Microsoft"}
        };

        // GET: Fabricantes
        public ActionResult Index()
        {
            return View(
                fabricantes
                //context.Fabricantes.OrderBy(c => c.Nome)
                );
        }

        // GET: Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fabricante fabricante)
        {
            fabricantes.Add(fabricante);
            fabricante.FabricanteId = fabricantes.Select(m => m.FabricanteId).Max() + 1;

            //context.Fabricantes.Add(fabricante);
            //context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Fabricantes/Edit/5
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricante fabricante = fabricantes.Where(m => m.FabricanteId == id).First();
            //Fabricante fabricante = context.Fabricantes.Find(id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // POST: Fabricantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                fabricantes.Remove(
                  fabricantes.Where(c => c.FabricanteId == fabricante.FabricanteId).First());
                fabricantes.Add(fabricante);
                //context.Entry(fabricante).State = EntityState.Modified;
                //context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fabricante);
        }

    }
}