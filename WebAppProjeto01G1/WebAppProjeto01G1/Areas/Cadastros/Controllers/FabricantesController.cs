using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.Data.Entity;
using Modelo.Cadastros;
using Servico.Cadastros;

namespace WebAppProjeto01G1.Areas.Cadastros.Controllers
{
    public class FabricantesController : Controller
    {
        private FabricanteServico fabricanteServico = new FabricanteServico();

        // GET: Fabricantes
        public ActionResult Index()
        {
            //return View(fabricantes);
            return View(fabricanteServico.ObterFabricantesClassificadosPorNome());
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
            //context.Fabricantes.Add(fabricante);
            //context.SaveChanges();
            fabricanteServico.GravarFabricante(fabricante);

            return RedirectToAction("Index");
        }

        // GET: Fabricantes/Edit/5
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                //return RedirectToAction("PaginaErro");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Fabricante fabricante = context.Fabricantes.Find(id);
            Fabricante fabricante = fabricanteServico.ObterFabricantePorId((long)id);

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
                //context.Entry(fabricante).State = EntityState.Modified;
                //context.SaveChanges();
                fabricanteServico.GravarFabricante(fabricante);
                return RedirectToAction("Index");
            }
            return View(fabricante);
        }

        // GET: Fabricantes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Fabricante fabricante = context.Fabricantes.Where(f =>f.FabricanteId == id).Include("Produtos.Categoria").First();
            Fabricante fabricante = fabricanteServico.ObterFabricantePorId((long)id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }


        // GET: Fabricantes/Delete/5
        [HttpGet]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Fabricante fabricante = context.Fabricantes.Find(id);
            Fabricante fabricante = fabricanteServico.ObterFabricantePorId((long)id);
            if (fabricante == null)
            {
                return HttpNotFound();
            }
            return View(fabricante);
        }

        // POST: Fabricantes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            //Fabricante fabricante = context.Fabricantes.Find(id);
            //context.Fabricantes.Remove(fabricante);
            //context.SaveChanges();
            Fabricante fabricante = fabricanteServico.ObterFabricantePorId(id);
            fabricanteServico.EliminarFabricantePorId(id);
            TempData["Message"] = "Fabricante " + fabricante.Nome.ToUpper() + " foi removido";
            return RedirectToAction("Index");
        }


    }
}