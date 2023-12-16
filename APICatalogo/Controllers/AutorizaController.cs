﻿using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace APICatalogo.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class AutorizaController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _singInManager;

    public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _singInManager = signInManager;
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
        return Ok();
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
            return Ok();
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Login invalido!");
            return BadRequest(ModelState);
        }
    }

}