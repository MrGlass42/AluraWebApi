using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Alura.ListaLeitura.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class ListasLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public ListasLeituraController(IRepository<Livro> Repository)
        {
            _repo = Repository;
        }

        private Lista CriarLista(TipoListaLeitura TipoLista)
        {
            return new Lista
            {
                Tipo = TipoLista.ParaString(),
                Livros = _repo.All.Where(x => x.Lista == TipoLista).Select(x => x.ToApi()).ToList()
            };
        }

        [HttpGet]
        public IActionResult TodasListas()
        {
            Lista ParaLer = CriarLista(TipoListaLeitura.ParaLer);
            Lista Lendo = CriarLista(TipoListaLeitura.Lendo);
            Lista Lidos = CriarLista(TipoListaLeitura.Lidos);
            var Colecao = new List<Lista> { ParaLer, Lendo, Lidos };

            return Ok(Colecao);
        }

        [HttpGet("{Tipo}")]
        public IActionResult ListarPorTipo(TipoListaLeitura Tipo)
        {
            var Lista = CriarLista(Tipo);
            return Ok(Lista);
        }
    }
}
