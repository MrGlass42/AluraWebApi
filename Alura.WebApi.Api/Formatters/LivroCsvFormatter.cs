using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.Api.Formatters
{
    public class LivroCsvFormatter : TextOutputFormatter
    {
        public LivroCsvFormatter()
        {
            var TextCsvMediaType = MediaTypeHeaderValue.Parse("text/csv");
            var ApplicationCsvMediaType = MediaTypeHeaderValue.Parse("application/csv");

            SupportedMediaTypes.Add(TextCsvMediaType);
            SupportedMediaTypes.Add(ApplicationCsvMediaType);

            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(LivroApi);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var LivroEmCsv = "";

            if (context.Object is LivroApi)
            {
                var Livro = context.Object as LivroApi;
                LivroEmCsv = $"{Livro.Titulo};{Livro.Subtitulo};{Livro.Autor};{Livro.ListaComNome()}";
            }

            using (var Escritor = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                return Escritor.WriteAsync(LivroEmCsv);
            }
        }
    }
}
