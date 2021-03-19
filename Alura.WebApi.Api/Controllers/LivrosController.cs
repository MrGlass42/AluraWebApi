using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Imagem = Alura.ListaLeitura.Modelos.Livro;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Alura.ListaLeitura.Api.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public LivrosController(IRepository<Livro> Repository)
        {
            _repo = Repository;
        }

        [HttpGet]
        public IActionResult ListaDeLivros()
        {
            var Lista = _repo.All.Select(x => x.ToApi()).ToList();
            return Ok(Lista);
        }

        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _repo.Excluir(model);

            return NoContent();
        }

        [HttpPut]
        public IActionResult Alterar([FromForm] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if (model.Capa == null)
                {
                    livro.ImagemCapa = _repo.All
                        .Where(l => l.Id == livro.Id)
                        .Select(l => l.ImagemCapa)
                        .FirstOrDefault();
                }
                _repo.Alterar(livro);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Incluir([FromForm] LivroUpload Livro)
        {
            if (ModelState.IsValid)
            {
                _repo.Incluir(Livro.ToLivro());
                return Created(Url.Action("Recuperar", new { id = Livro.Id }), Livro);
            }

            return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult Recuperar(int id)
        {
            var Livro = _repo.Find(id);
            if (Livro == null)
                return NotFound();

            return Ok(Livro.ToApi());
        }

        [HttpGet("Capas/{id}")]
        public IActionResult ImagemCapa(int id)
        {
            byte[] img = _repo.All
                .Where(l => l.Id == id)
                .Select(l => l.ImagemCapa)
                .FirstOrDefault();
                
            if (img == null)
                return null;

            return File(img, "image/png");
        }
    }
}
