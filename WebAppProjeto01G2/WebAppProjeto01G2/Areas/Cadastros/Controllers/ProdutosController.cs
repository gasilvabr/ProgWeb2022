using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Modelo.Cadastros;
using Servico.Cadastros;
using Servico.Tabelas;
using System.IO;

namespace WebAppProjeto01G2.Areas.Cadastros.Controllers
{
    public class ProdutosController : Controller
    {
        //private EFContext context = new EFContext();
        private ProdutoServico produtoServico = new ProdutoServico();
        private CategoriaServico categoriaServico = new CategoriaServico();
        private FabricanteServico fabricanteServico = new FabricanteServico();

        // GET: Produtos
        public ActionResult Index()
        {
            //var produtos = context.Produtos.Include(c => c.Categoria).Include(f => f.Fabricante).OrderBy(c => c.Nome);
            var produtos = produtoServico.ObterProdutosClassificadosPorNome();

            return View(
                produtos
                );
        }

        // GET: Produtos/Details/5
        public ActionResult Details(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Produto produto = context.Produtos.Where(p => p.ProdutoId == id).Include(c => c.Categoria).Include(f => f.Fabricante).First();
            Produto produto = produtoServico.ObterProdutoPorId((long)id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
            //ViewBag.CategoriaId = new SelectList(context.Categorias.OrderBy(b => b.Nome), "CategoriaId", "Nome");
            //ViewBag.FabricanteId = new SelectList(context.Fabricantes.OrderBy(b => b.Nome), "FabricanteId", "Nome");
            ViewBag.CategoriaId = new SelectList(categoriaServico.ObterCategoriasClassificadasPorNome(), "CategoriaId", "Nome");
            ViewBag.FabricanteId = new SelectList(fabricanteServico.ObterFabricantesClassificadosPorNome(), "FabricanteId", "Nome");
            return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        public ActionResult Create(Produto produto)
        {
            try
            {
                // TODO: Add insert logic here
                //context.Produtos.Add(produto);
                //context.SaveChanges();
                produtoServico.GravarProduto(produto);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.CategoriaId = new SelectList(categoriaServico.ObterCategoriasClassificadasPorNome(), "CategoriaId", "Nome");
                ViewBag.FabricanteId = new SelectList(fabricanteServico.ObterFabricantesClassificadosPorNome(), "FabricanteId", "Nome");
                return View();
            }
        }

        // GET: Produtos/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Produto produto = context.Produtos.Find(id);
            Produto produto = produtoServico.ObterProdutoPorId((long)id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CategoriaId = new SelectList(context.Categorias.OrderBy(b => b.Nome), "CategoriaId", "Nome", produto.CategoriaId);
            //ViewBag.FabricanteId = new SelectList(context.Fabricantes.OrderBy(b => b.Nome), "FabricanteId", "Nome", produto.FabricanteId);
            ViewBag.CategoriaId = new SelectList(categoriaServico.ObterCategoriasClassificadasPorNome(), "CategoriaId","Nome", produto.CategoriaId);
            ViewBag.FabricanteId = new SelectList(fabricanteServico.ObterFabricantesClassificadosPorNome(), "FabricanteId","Nome", produto.FabricanteId);

            return View(produto);
        }
        // Metodo Privado
        private void PopularViewBag(Produto produto = null)
        {
            if (produto == null)
            {
                ViewBag.CategoriaId = new SelectList(categoriaServico.ObterCategoriasClassificadasPorNome(),
                "CategoriaId", "Nome");
                ViewBag.FabricanteId = new SelectList(fabricanteServico.ObterFabricantesClassificadosPorNome(),
                "FabricanteId", "Nome");
            }
            else
            {
                ViewBag.CategoriaId = new SelectList(categoriaServico.ObterCategoriasClassificadasPorNome(),
                "CategoriaId", "Nome", produto.CategoriaId);
                ViewBag.FabricanteId = new SelectList(fabricanteServico.ObterFabricantesClassificadosPorNome(),
                "FabricanteId", "Nome", produto.FabricanteId);
            }
        }

        private byte[] SetLogotipo(HttpPostedFileBase logotipo)
        {
            var bytesLogotipo = new byte[logotipo.ContentLength];
            logotipo.InputStream.Read(bytesLogotipo, 0, logotipo.ContentLength);
            return bytesLogotipo;
        }

        private ActionResult GravarProduto(Produto produto, 
            HttpPostedFileBase logotipo, string chkRemoverImagem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (chkRemoverImagem != null)
                    {
                        produto.Logotipo = null;
                    }
                    if (logotipo != null)
                    {
                        produto.LogotipoMimeType = logotipo.ContentType;
                        produto.Logotipo = SetLogotipo(logotipo); // Faz upload no banco de dados
                        produto.NomeArquivo = logotipo.FileName;
                        produto.TamanhoArquivo = logotipo.ContentLength;

                        // Fazendo o upload na pasta do servidor
                        string strFilename = Server.MapPath("~/App_Data/" + produto.NomeArquivo);
                        logotipo.SaveAs(strFilename);
                    }
                    produtoServico.GravarProduto(produto);
                    return RedirectToAction("Index");
                }
                PopularViewBag(produto);
                return View(produto);
            }
            catch
            {
                PopularViewBag(produto);
                return View(produto);
            }
        }

        public ActionResult DownloadArquivo(long id)
        {
            Produto produto = produtoServico.ObterProdutoPorId(id);
            FileStream fileStream = new FileStream(Server.MapPath("~/App_Data/" + produto.NomeArquivo), FileMode.Open, FileAccess.Read);
            return File(fileStream.Name, produto.LogotipoMimeType, produto.NomeArquivo);
        }
/*        public ActionResult DownloadArquivo(long id)
        {
            Produto produto = produtoServico.ObterProdutoPorId(id);
            FileStream fileStream = new FileStream(Server.MapPath("~/App_Data/" + produto.NomeArquivo), FileMode.Create,FileAccess.Write);
            fileStream.Write(produto.Logotipo, 0, Convert.ToInt32(produto.TamanhoArquivo));
            fileStream.Close();
            return File(fileStream.Name, produto.LogotipoMimeType, produto.NomeArquivo);
        }*/

        [HttpPost]
        public ActionResult Edit(Produto produto, 
            HttpPostedFileBase logotipo = null, string chkRemoverImagem = null)
        {
            return GravarProduto(produto, logotipo, chkRemoverImagem);
        }

        public FileContentResult GetLogotipo(long id)
        {
            Produto produto = produtoServico.ObterProdutoPorId(id);
            if (produto != null)
            {
                return File(produto.Logotipo, produto.LogotipoMimeType);
            }
            return null;
        }

        public string GetLogotipoName(long id)
        {
            Produto produto = produtoServico.ObterProdutoPorId(id);
            if (produto != null)
            {
                return Server.MapPath("~/App_Data/" + produto.NomeArquivo);
            }
            return null;
        }
        // POST: Produtos/Edit/5
        /*[HttpPost]
        public ActionResult Edit(Produto produto)
        {
            try
            {
                // TODO: Add update logic here
                //context.Entry(produto).State = EntityState.Modified;
                //context.SaveChanges();
                produtoServico.GravarProduto(produto);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/

        // GET: Produtos/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Produto produto = context.Produtos.Where(p => p.ProdutoId == id).Include(c => c.Categoria).Include(f => f.Fabricante).First();
            Produto produto = produtoServico.ObterProdutoPorId((long)id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {
                // TODO: Add delete logic here
                //Produto produto = context.Produtos.Find(id);
                //context.Produtos.Remove(produto);
                //context.SaveChanges();
                Produto produto = produtoServico.ObterProdutoPorId(id);
                produtoServico.EliminarProdutoPorId(id);
                TempData["Message"] = "Produto " + produto.Nome.ToUpper() + " foi removido";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
