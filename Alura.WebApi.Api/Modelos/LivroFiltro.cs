using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.WebApi.Api.Modelos
{
    public static class LivroFiltroExtensions
    {
        public static IQueryable<Livro> AplicaFiltro (this IQueryable<Livro> Query, LivroFiltro Filtro)
        {
            if(Filtro != null)
            {
                if (!string.IsNullOrEmpty(Filtro.Titulo))
                    Query = Query.Where(x => x.Titulo.Contains(Filtro.Titulo));

                if (!string.IsNullOrEmpty(Filtro.Subtitulo))
                    Query = Query.Where(x => x.Subtitulo.Contains(Filtro.Subtitulo));

                if (!string.IsNullOrEmpty(Filtro.Autor))
                    Query = Query.Where(x => x.Autor.Contains(Filtro.Autor));

                if (!string.IsNullOrEmpty(Filtro.Lista))
                    Query = Query.Where(x => x.Lista == Filtro.Lista.ParaTipo());
            }

            return Query;
        }
    }

    public class LivroFiltro
    {
        public string Autor { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Lista { get; set; }
    }
}
