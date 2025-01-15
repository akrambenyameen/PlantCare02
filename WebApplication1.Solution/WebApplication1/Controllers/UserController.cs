using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Api.DTOs;
using WebApplication1.Api.Errors;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry.Data;
using WebApplication1.Service;
using WebApplication1.Service.WebApplication1.Repositry.WebApplication1.Core.services;

namespace WebApplication1.Api.Controllers
{

    public class UserController : ApiBaseController
    {
        private readonly IGenericRepositry<User> UserRepo;
        private readonly AuthService auth;
        private readonly PlantCareContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signinManager;
        private readonly IEmailService emailService;
        private readonly ILogger<UserController> _logger;

        public UserController(IGenericRepositry<User> userRepo,
                               AuthService auth,
                               PlantCareContext dbContext,
                               UserManager<User> userManager,
                               SignInManager<User> signInManager,
                               IEmailService emailService, ILogger<UserController> _logger
                               )
        {
            UserRepo = userRepo;
            this.auth = auth;
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.signinManager = signInManager;
            this.emailService = emailService;
            this._logger = _logger;
        }

        //[HttpGet("error/{id}")]
        //public async Task getServerError(int id)
        //{
        //    var user = await UserRepo.GetByIdAsync(200);
        //    var userString = user.ToString();//throw server error
        //}

        #region get-all-users 
        [HttpGet("getall")]

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {

            var users = await UserRepo.GetAllAsync();

            if (!users.Any())
            {
                return NotFound(new ApiErrorResponse(404));
            }
            return Ok(users);
        }
        #endregion

        #region login
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.email);
            if (user == null)
                return Unauthorized(new ApiErrorResponse(401, "this email dosent exist"));

            var result = await signinManager.CheckPasswordSignInAsync(user, request.password, false);

           if(!await userManager.IsEmailConfirmedAsync(user))
            return Unauthorized(new ApiErrorResponse(401,"Verify your email first , Check yor email messages"));

            if (!result.Succeeded) return Unauthorized(new ApiErrorResponse(401, "invalid password"));

            var token = await auth.RegisterOrLogin(user);
            
            var response = new LoginResponseDto()
            {
                message = "Login Successful",
                token = token
            };
            return Ok(response);

        }
        #endregion

        #region register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
         
                var isExist = await userManager.FindByEmailAsync(request.Email);
                if (isExist is not null)
                    return BadRequest(new ApiErrorResponse(400, "This email address is already in use"));
                
                var user = new User()
                {
                    name = request.name,
                    Email = request.Email,
                    PhoneNumber = request.Phone,
                    UserName = request.Email
                };
                var result = await userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest(new ApiErrorResponse(400, $"Something went wrong: {errors}"));
                }
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var param = new Dictionary<string, string?>{
                    { "token" ,token },
                    {"email",user.Email }
                };
                var callback = QueryHelpers.AddQueryString("https://plantcarehub.netlify.app/emailConfirmation", param);

                await emailService.sendEmailAsync(user.Email, "Email Confirmation", $"Click the link below to confirm your email:\r\n{callback}\r\n\r\n");
                
                return Ok("Registration successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");

            }
        }
        #endregion

        #region logout

        [HttpPost("logout")]
        public async Task<IActionResult> logout([FromHeader] string Authorization)
        {


            if (string.IsNullOrEmpty(Authorization))
            {
                return BadRequest(new { error = "Invalid or missing header" });
            }
            var token = await dbContext.Tokens.FirstOrDefaultAsync(t => t.token == Authorization && t.isValid);
            if (token == null)
                return BadRequest(new ApiErrorResponse(401, "An error occured"));
            token.isValid = false;
            await dbContext.SaveChangesAsync();
            return Ok(new { message = "Logout successful" });

        }
        #endregion

        #region forget-password
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new ApiErrorResponse(400, "Email is not found"));


            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);


            var resetLink = Url.Action(
                "reset-password",
                "User",
                new { token = resetToken, email = request.Email },
                Request.Scheme
                );
            resetLink = $"https://plantcarehub.netlify.app/NewPassword?token={Uri.EscapeDataString(resetToken)}&email={request.Email}";



            await emailService.sendEmailAsync(request.Email,
                                             "Reset Password",
                                             $"Click the link below to securely reset your password:\r\n{$"{resetLink}"}\r\n\r\n" +
                                             $"If you did not request a password reset, please ignore this message.\r\n\r\nBest regards," +
                                             $"\r\nThe PlantCare Team");

            return Ok(new { Message = "you will receive reset instructions,check your email.", resetLink });
        }
        #endregion

        #region reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> resetPassword([FromQuery] string token, [FromQuery] string email, [FromBody] resetPasswordRequestDto request)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest(new ApiErrorResponse(400, "Token or Email is missing"));
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiErrorResponse(401, "Email address is required"));
            var isSamePassword = await userManager.CheckPasswordAsync(user, request.newPassword);
            if (isSamePassword)
            {
                return BadRequest("The new password cannot be the same as the old password.");
            }
            var result = await userManager.ResetPasswordAsync(user, token, request.newPassword);
            if (!result.Succeeded)
                return BadRequest(new ApiErrorResponse(400, "Something Went Wrong"));
            return Ok(new { Message = "Password updated!" });
        }
        #endregion
        [HttpGet("emailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email , [FromQuery] string token)
        {
            Console.WriteLine($"Received Token: {token}");

            var user = await userManager.FindByEmailAsync (email);
            if (user == null)
                return BadRequest(new ApiErrorResponse(404,"User not found"));
            var isconfirmed = await userManager.ConfirmEmailAsync(user, token);
            if (!isconfirmed.Succeeded)
            {
                Console.WriteLine($"Email confirmation failed. Errors: {string.Join(", ", isconfirmed.Errors.Select(e => e.Description))}");
                return BadRequest(new ApiErrorResponse(400, "An error occurred while confirming your email"));
            }

            return Ok(new { message = "Email verified successfully" });
        }
    }
}
