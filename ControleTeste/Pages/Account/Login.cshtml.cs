using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ControleTeste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ControleTeste.Services;

namespace ControleTeste.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public LoginModel(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Token { get; set; }

        public class InputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            // permitir mostrar mensagem, etc. returnUrl será usado após login
            ViewData["ReturnUrl"] = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(Input?.Username) || string.IsNullOrWhiteSpace(Input?.Password))
            {
                ModelState.AddModelError(string.Empty, "Credenciais inválidas");
                return Page();
            }

            var user = await _userService.ValidateCredentialsAsync(Input.Username, Input.Password);
            if (user == null)
            {
                // identificar se usuário existe para mostrar mensagem específica
                var exists = await _userService.GetByUsernameAsync(Input.Username);
                if (exists == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuário não encontrado");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Senha incorreta");
                }
                return Page();
            }

            // Gerar token
            var jwtSection = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSection["Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("isAdmin", user.IsAdmin ? "true" : "false")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpireMinutes"] ?? "60")),
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Salvar token em cookie para uso simples no browser (nota: para produção, reavaliar segurança)
            Response.Cookies.Append("AuthToken", tokenString, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
            });

            // Redirecionar para returnUrl se seguro, senão para /Alteracoes
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToPage("/Alteracoes/Index");
        }
    }
}
