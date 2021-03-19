using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Alura.ListaLeitura.Service
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;

        public LoginController (SignInManager<Usuario> SignInManager)
        {
            _signInManager = SignInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel Model) 
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Model.Login, Model.Password, true, true);
                if (result.Succeeded)
                {
                    var Token = "";
                    //Criar Token (Header + Payload >> Claims + Signature)

                    var Direitos = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, Model.Login),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var Chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("alura-webapi-authentication-valid"));

                    var Credencias = new SigningCredentials(Chave, SecurityAlgorithms.HmacSha256);

                    var JwtToken = new JwtSecurityToken(
                        issuer: "Alura.WebApp",
                        audience: "Postman",
                        claims: Direitos,
                        signingCredentials: Credencias,
                        expires: DateTime.Now.AddMinutes(30)
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(JwtToken));
                }
                else
                    return Unauthorized();
            }
            else
                return BadRequest();
        }
    }
}
