﻿using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICatalogo.Controllers;

[Produces("application/json")]
[Route("api/[Controller]")]
[ApiController]
public class AutorizaController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _singInManager;
    private readonly IConfiguration _configuration;

    public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _singInManager = signInManager;
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return "AutorizaController :: Acessado em : " + DateTime.Now.ToLongDateString();
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        }

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await _singInManager.SignInAsync(user, false);
        return Ok(GeraToken(model));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        }

        var result = await _singInManager.PasswordSignInAsync(userInfo.Email,
            userInfo.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok(GeraToken(userInfo));
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Login invalido!");
            return BadRequest(ModelState);
        }
    }

    private UsuarioToken GeraToken(UsuarioDTO userInfo)
    {
        // Define declarações do usuário (opcional)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
            new Claim("claim1", "claim2"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // Gera uma chave com base em um algoritmo simetrico
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        // Gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
        var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        // Tempo de expiração do token
        var expiracao = _configuration["TokenConfiguration:ExpireHours"];
        var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

        // Classe que representa um token jwt e gera o token
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["TokenConfiguration:Issuer"],
            audience: _configuration["TokenConfiguration:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credenciais);

        // Retorna os dados com o token e informações
        return new UsuarioToken()
        {
            Authenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Message = "Token JWT OK"
        };


    }



}
