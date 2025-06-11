﻿using Microsoft.AspNetCore.Mvc;

namespace CadastroProdutos.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Produto()
        {
            return View();
        }
    }
}