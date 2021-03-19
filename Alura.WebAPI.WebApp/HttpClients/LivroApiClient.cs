using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _accessor;

        public LivroApiClient(HttpClient httpClient, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _accessor = accessor;
        }

        private void AddBearerToken()
        {
            var Token = _accessor.HttpContext.User.Claims.First(x => x.Type == "Token").Value;
            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        public async Task<byte[]> GetCapaLivroAsync(int id)
        {
            AddBearerToken();
            HttpResponseMessage Resposta = await _httpClient.GetAsync($"Livros/Capas/{id}");
            Resposta.EnsureSuccessStatusCode();

            return await Resposta.Content.ReadAsByteArrayAsync();
        }
        public async Task<LivroApi> GetLivroAsync(int id)
        {
            AddBearerToken();
            HttpResponseMessage Resposta = await _httpClient.GetAsync($"Livros/{id}");
            Resposta.EnsureSuccessStatusCode();

            return await Resposta.Content.ReadAsAsync<LivroApi>();
        }

        public async Task DeleteLivroAsync(int id)
        {
            AddBearerToken();
            var Resposta = await _httpClient.DeleteAsync($"Livros/{id}");
            Resposta.EnsureSuccessStatusCode();
        }

        public async Task<Lista> GetListaLeituraAsync(TipoListaLeitura Tipo)
        {
            AddBearerToken();
            var Resposta = await _httpClient.GetAsync($"ListasLeitura/{Tipo}");
            Resposta.EnsureSuccessStatusCode();
            return await Resposta.Content.ReadAsAsync<Lista>();
        }

        public async Task PostLivroAsync(LivroUpload Livro)
        {
            AddBearerToken();
            HttpContent Conteudo = CriarMultipartFormDataContent(Livro);
            var Resposta = await _httpClient.PostAsync("Livros", Conteudo);
            Resposta.EnsureSuccessStatusCode();
        }

        public async Task PutLivroAsync(LivroUpload Livro)
        {
            AddBearerToken();
            HttpContent Conteudo = CriarMultipartFormDataContent(Livro);
            var Resposta = await _httpClient.PutAsync("Livros/", Conteudo);
            Resposta.EnsureSuccessStatusCode();
        }

        private string EnvolveComAspasDuplas(string Valor)
        {
            return $"\"{Valor}\"";
        }

        private HttpContent CriarMultipartFormDataContent(LivroUpload Livro)
        {
            var Content = new MultipartFormDataContent();

            Content.Add(new StringContent(Livro.Titulo), EnvolveComAspasDuplas("Titulo"));
            Content.Add(new StringContent(Livro.Lista.ParaString()), EnvolveComAspasDuplas("Lista"));

            if(!string.IsNullOrEmpty(Livro.Subtitulo))
                Content.Add(new StringContent(Livro.Subtitulo), EnvolveComAspasDuplas("Subtitulo"));

            if (!string.IsNullOrEmpty(Livro.Resumo))
                Content.Add(new StringContent(Livro.Resumo), EnvolveComAspasDuplas("Resumo"));
            
            if (!string.IsNullOrEmpty(Livro.Autor))
                Content.Add(new StringContent(Livro.Autor), EnvolveComAspasDuplas("Autor"));

            if (Livro.Id > 0)
                Content.Add(new StringContent(Livro.Id.ToString()), EnvolveComAspasDuplas("id"));

            if(Livro.Capa != null)
            {
                var ImagemContent = new ByteArrayContent(Livro.Capa.ConvertToBytes());
                ImagemContent.Headers.Add("content-type", "image/png");
                Content.Add(ImagemContent, EnvolveComAspasDuplas("Capa"), EnvolveComAspasDuplas("capa.png"));
            }

            return Content;
        }
    }
}
