using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

using MisteriosDaNaturaleza.Modelos;

using MisteriosDaNatureza.Utilidades;

namespace MisteriosDaNaturaleza.Areas.Identity.Pages.Account
{
    [Authorize]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _rolSupervisor;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> rolSupervisor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _rolSupervisor = rolSupervisor;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //Copiamos os campos de rexistro de usuario de AplicacionUsuario.cs
            public string Nome { get; set; }
            public string Rua { get; set; }
            public string Poboacion { get; set; }
            public string Provincia { get; set; }
            public string CodigoPostal { get; set; }
            public string NumeroTelefono { get; set; }//engadimos telefono como extra
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var usuario = new AplicacionUsuario
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nome = Input.Nome,
                    Rua = Input.Rua,
                    Poboacion = Input.Poboacion,
                    Provincia = Input.Provincia,
                    CodigoPostal = Input.CodigoPostal,
                    PhoneNumber = Input.NumeroTelefono

                };
                var result = await _userManager.CreateAsync(usuario, Input.Password);
                if (result.Succeeded)
                {
                    ////comprobamos solo un rol, porque cando creamos un rol xa os creamos todos aproveitando
                    //if(!await _rolSupervisor.RoleExistsAsync(DE.Admin)) //comprobamos se non existe usuario Admin 
                    //{
                    //    //nese caso, creamos a IdentityRole de Admin dentro das taboas da BD, na taboa AspNetUserRoles
                    //    await _rolSupervisor.CreateAsync(new IdentityRole(DE.Admin));
                    //    await _rolSupervisor.CreateAsync(new IdentityRole(DE.Supervisor));
                    //}

                    //unha vez creadas as reglas, comprobamos que tipo de usuario seleccionou no rexistro co boton de radio a persona k elixeu rol
                    //en Register.cshtml.cs temos o nome identificador que lle puxemos no boton radio, neste caso rdRolUsr
                    string rol = Request.Form["rdRolUsr"].ToString();

                    if (rol == DE.Admin)
                    {
                        await _userManager.AddToRoleAsync(usuario, DE.Admin);
                    }
                    else
                    {
                        if (rol == DE.Supervisor)
                        {
                            await _userManager.AddToRoleAsync(usuario, DE.Supervisor);
                        }
                    }

                    _logger.LogInformation("O usuario creou unha nova conta con password");

                    //Dado que SOLO o Admin -e non o usuario final- pode rexistrar novos empregados, non necesitamos ningun tipo de confirmacion de email (a parte comentada abaixo).

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = usuario.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //comentamos aqui xk se rexistramos un novo usuario, non queremos que saia da conta de Admin e entre na do usuario creado
                        //await _signInManager.SignInAsync(usuario, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
