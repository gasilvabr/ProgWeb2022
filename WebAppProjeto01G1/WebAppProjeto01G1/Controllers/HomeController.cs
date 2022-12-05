using Servico.Cadastros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProjeto01G1.Models;

namespace WebAppProjeto01G1.Controllers
{
    public class HomeController : Controller
    {
        private ProdutoServico produtoServico = new ProdutoServico();

        // GET: Home
        public ActionResult Index()
        {
            HomeClass home = new HomeClass();

            home.listaprodutoDestaques = produtoServico.ObterProdutosClassificadosPorNome();
            home.listaProdutoLancamento = produtoServico.ObterProdutosClassificadosPorNome();
            return View(home);
        }
    }
}