using Alura.ListaLeitura.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.HttpClients
{
    public class AuthApiClient
    {
        public class LoginResult
        {
            public bool Succeeded { get; set; }
            public string Token { get; set;}
        }

        private readonly HttpClient _httpClient;
        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> PostLoginAsync(LoginModel model)
        {
            var Resposta = await _httpClient.PostAsJsonAsync("Login", model);
            return new LoginResult { Succeeded = Resposta.IsSuccessStatusCode, Token = await Resposta.Content.ReadAsStringAsync() };
        }
    }
}
