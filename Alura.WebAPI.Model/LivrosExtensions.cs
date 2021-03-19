using System.IO;
using Microsoft.AspNetCore.Http;

namespace Alura.ListaLeitura.Modelos
{
    public static class LivrosExtensions
    {
        public static byte[] ConvertToBytes(this IFormFile image)
        {
            if (image == null)
            {
                return null;
            }
            using (var inputStream = image.OpenReadStream())
            using (var stream = new MemoryStream())
            {
                inputStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public static LivroUpload ToUpload(this LivroApi Livro)
        {
            return new LivroUpload
            {
                Id = Livro.Id,
                Titulo = Livro.Titulo,
                Subtitulo = Livro.Subtitulo,
                Resumo = Livro.Resumo,
                Autor = Livro.Autor,
                Lista = Livro.Lista.ParaTipo()
            };
        }

        public static Livro ToLivro(this LivroUpload model)
        {
            return new Livro
            {
                Id = model.Id,
                Titulo = model.Titulo,
                Subtitulo = model.Subtitulo,
                Resumo = model.Resumo,
                Autor = model.Autor,
                ImagemCapa = model.Capa.ConvertToBytes(),
                Lista = model.Lista
            };
        }

        public static LivroApi ToApi(this Livro livro)
        {
            return new LivroApi
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Subtitulo = livro.Subtitulo,
                Resumo = livro.Resumo,
                Autor = livro.Autor,
                Capa = $"api/Livros/Capas/{livro.Id}",
                //Lista = $"api/ListasLeitura/{livro.Lista.ParaString()}"
                Lista = livro.Lista.ParaString()
            };
        }

        public static Livro ToLivro(this LivroApi livro)
        {
            return new Livro
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Subtitulo = livro.Subtitulo,
                Resumo = livro.Resumo,
                Autor = livro.Autor,
                ImagemCapa = livro.ToUpload().Capa.ConvertToBytes(),
                //Lista = $"api/ListasLeitura/{livro.Lista.ParaString()}"
                Lista = livro.ToUpload().Lista
            };
        }

        public static string ListaComNome(this LivroApi Livro)
        {
            var RotaQuebrada = Livro.Lista.Split("/");
            return RotaQuebrada[RotaQuebrada.Length - 1].ToString();
        }

        public static LivroUpload ToModel(this Livro livro)
        {
            return new LivroUpload
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Subtitulo = livro.Subtitulo,
                Resumo = livro.Resumo,
                Autor = livro.Autor,
                Lista = livro.Lista
            };
        }
    }
}
