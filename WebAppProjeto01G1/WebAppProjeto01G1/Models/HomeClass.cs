using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelo.Cadastros;

namespace WebAppProjeto01G1.Models
{
    public class HomeClass
    {
        public IQueryable<Produto> listaProdutoLancamento;
        public IQueryable<Produto> listaprodutoDestaques;
    }
}