using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppProjeto01G1.Infraestrutura;

namespace WebAppProjeto01G1.Areas.Seguranca.Controllers
{
    public class AdminController : Controller
    {

        // Definição da Propriedade GerenciadorUsuario
        private GerenciadorUsuario GerenciadorUsuario00
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<GerenciadorUsuario>();
            }
        }

        // GET: Seguranca/Admin
        public ActionResult Index()
        {
            return View(GerenciadorUsuario00.Users);
        }
    }
}