using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.Api.Formatters
{
    public class ListasLeituraCsvFormatter : TextOutputFormatter
    {
        public ListasLeituraCsvFormatter()
        {
            var ApplicationCsvMediaType = MediaTypeHeaderValue.Parse("application/csv");
            var TextCsvMediaType = MediaTypeHeaderValue.Parse("text/csv");
            SupportedMediaTypes.Add(ApplicationCsvMediaType);
            SupportedMediaTypes.Add(TextCsvMediaType);

            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(Lista);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var ListasLeituraCsv = "";
            if (context.Object is Lista)
            {
                var ListasLeitura = context.Object as Lista;
                ListasLeitura.Livros.ToList().ForEach(x =>
                {
                    ListasLeituraCsv += $"{x.Titulo};{x.Subtitulo};{x.Autor};{x.ListaComNome()}\n";
                });
            }

            using (var Escritor = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                return Escritor.WriteAsync(ListasLeituraCsv);
            }
        }
    }
}
