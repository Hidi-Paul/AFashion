using OCS.WebApi.Security;
using OCS.WebApi.SecurityModels;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Claims;

namespace OCS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager userManager;

        public AccountController()
        {
            userManager = new ApplicationUserManager(new ApplicationUserStore(new SecurityDbContext()));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();

            }
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = model.Email,
                Email = model.Email
            };
            try
            {
                var result = await userManager.CreateAsync(user, model.Password);
                if (model.Email.Contains("admin"))
                {
                    var claimResult=await userManager.AddClaimAsync(user.Id, new Claim("Role", "Admin"));
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }


            return Ok();
        }
    }
}
