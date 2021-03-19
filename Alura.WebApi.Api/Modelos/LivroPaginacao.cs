
using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.WebApi.Api.Modelos
{
    public static class LivroPaginadoExtensions
    {
        public static LivroPaginado ToLivroPaginado(this IQueryable<LivroApi> Query, LivroPaginacao Paginacao)
        {
            var TotalItens = Query.Count();
            var TotalPaginas = (int) Math.Ceiling((TotalItens / (double) Paginacao.Tamanho));
            return new LivroPaginado
            {
                Total = TotalItens,
                TotalPaginas = TotalPaginas,
                NumeroPagina = Paginacao.Pagina,
                TamanhoPagina = Paginacao.Tamanho,
                Resultado = Query
                    .Skip(Query.Count() * (Paginacao.Pagina - 1))
                    .Take(Paginacao.Tamanho).ToList(),
                Anterior = Paginacao.Pagina > 1 ? $"Livros?pagina={Paginacao.Pagina - 1}&tamanho={Paginacao.Tamanho}" : "" ,
                Proximo = Paginacao.Pagina < TotalPaginas ? $"Livros?pagina={Paginacao.Pagina + 1}&tamanho={Paginacao.Tamanho}" : "" ,
            };
        }
    }

    public class LivroPaginado
    {
        public int Total { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroPagina { get; set; }
        public IList<LivroApi> Resultado { get; set; }
        public string Anterior { get; set; }
        public string Proximo { get; set; }
    }
    public class LivroPaginacao
    {
        public int Pagina { get; set; } = 1;
        public int Tamanho { get; set; } = 25;
    }
}
