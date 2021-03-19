using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Alura.WebApi.Api.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Alura.ListaLeitura.Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Authorize]
    [ApiExplorerSettings(GroupName ="v2")]
    [Route("api/v{version:apiVersion}/Livros")]
    //[Route("api/Livros")]
    public class Livros2Controller : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public Livros2Controller(IRepository<Livro> Repository)
        {
            _repo = Repository;
        }

        [HttpGet]
        public IActionResult ListaDeLivros([FromQuery] LivroFiltro Livro, [FromQuery] LivroOrdem Ordem, [FromQuery] LivroPaginacao Paginacao)
        {
            var LivroPaginado = _repo.All
                .AplicaFiltro(Livro)
                .AplicaOrdem(Ordem)
                .Select(x => x.ToApi())
                .ToLivroPaginado(Paginacao);
            return Ok(LivroPaginado);
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

            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }

        [HttpGet("{id}")]
        public IActionResult Recuperar(int id)
        {
            var Livro = _repo.Find(id);
            if (Livro == null)
                return NotFound();

            return Ok(Livro);
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
