using BugTrackingDAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BugTrackingBL
{
    public class LoginManager : ILoginManager
    {
        private readonly UserManager<GeneralUser> _userManager;
        private readonly IConfiguration _config;
        public LoginManager(UserManager<GeneralUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        async public Task<TokenDto> UserLogin(LoginDto credentials )
        {
            #region Get user Data
            GeneralUser? user = await _userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
            {
                return null;
            }
            #endregion

            #region Check user password
            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, credentials.Password);
            if (!isPasswordCorrect)
            {
                return null;
            }
            #endregion

            return await GenerateToken(await _userManager.GetClaimsAsync(user),credentials.Email);
        }

        #region Functions
      async private Task<TokenDto> GenerateToken(IList<Claim> claimList , string email)
        {
            #region Prepare secretKey
            var stringKey = _config.GetSection("secretkey").Value ?? string.Empty;
            Console.WriteLine(stringKey);
            var bytesKey = Encoding.ASCII.GetBytes(stringKey);
            var key = new SymmetricSecurityKey(bytesKey);
            #endregion

            #region Generate JWT Token object 
            var signincredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expireDate = DateTime.Now.AddMinutes(60);
          
            var jwt = new JwtSecurityToken(
                expires: expireDate,
                claims: claimList,
                signingCredentials: signincredentials
                );
            #endregion

            #region Convert token to string
            var handler = new JwtSecurityTokenHandler();
            var jwtString = handler.WriteToken(jwt);
            #endregion
            var user = await _userManager.FindByEmailAsync(email);
            
            return new TokenDto { Token = jwtString, Expiry = expireDate , Role = user.Type};
        }
        #endregion
    }
}
