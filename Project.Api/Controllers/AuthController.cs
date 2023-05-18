using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Project.Core.Converters;
using Project.Core.Entities;
using Project.Core.Enum;
using Project.Core.Exceptions;
using Project.Core.Models.Auth;
using Project.Core.Models.Dto;
using Project.Core.Models.Request;
using Project.Core.Models.Response;
using Project.Core.OperationInterfaces;
using Project.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v{v:apiVersion}/auth")]
    [ApiController, ApiVersion("1")]
    public class AuthController : Controller
    {
        private readonly TokenProviderOptions _tokenProviderOptions;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IOptions<TokenProviderOptions> options, IMapper mapper, IUserService userService)
        {
            _tokenProviderOptions = options.Value;
            ThrowIfInvalidOptions(_tokenProviderOptions);
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ResultRequest<DtoTokenResponse>> Login([FromBody] InboundRequest<DtoAuthLoginPair> request)
        {
            try
            {
                var dto = request?.Data;
                if (dto == null)
                {
                    return ResultRequest<DtoTokenResponse>.Error("Access Token request fail", "Invalid request data");
                }
                var token = await GenerateToken(HttpContext, dto.UserName, dto.Password);
                User user = _userService.GetByUserName(dto.UserName);
                token.User = _mapper.Map<DtoUser>(user);
                return ResultRequest<DtoTokenResponse>.Ok(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return ResultRequest<DtoTokenResponse>.Error("Access Token request error", e.Message);
            }
        }

        [HttpPost("login-api-key")]
        public async Task<ResultRequest<DtoTokenResponse>> LoginWithApiKey([FromBody] InboundRequest<DtoAuthApiKey> request)
        {
            try
            {
                var dto = request?.Data;
                if (dto == null)
                {
                    return ResultRequest<DtoTokenResponse>.Error("Access Token request fail", "Invalid request data");
                }

                var token = await GenerateToken(HttpContext, dto.ApiKey);

                return ResultRequest<DtoTokenResponse>.Ok(token);

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return ResultRequest<DtoTokenResponse>.Error("Access Token request error", e.Message);
            }
        }

        [HttpPost("refresh-token")]
        public ResultRequest<DtoTokenResponse> RefreshToken([FromBody] InboundRequest<DtoRefreshToken> request)
        {
            try
            {
                var dto = request?.Data;
                if (dto == null)
                {
                    return ResultRequest<DtoTokenResponse>.Error("Access Token request fail", "Invalid request data");
                }

                var encryptedRefreshToken = dto.RefreshToken;
                if (string.IsNullOrEmpty(encryptedRefreshToken))
                    throw new LogicException("Refresh Token is required for grantType RefreshToken");

                var tokenFromRefreshToken = GetNewTokenFromRefreshToken(encryptedRefreshToken);



                return ResultRequest<DtoTokenResponse>.Ok(tokenFromRefreshToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return ResultRequest<DtoTokenResponse>.Error("Access Token request error", e.Message);
            }
        }

        private async Task<DtoTokenResponse> GenerateToken(
            HttpContext context,
            string userName,
            string password)
        {
            var identity = await _tokenProviderOptions.IdentityResolver(context, userName, password);
            if (identity == null)
            {
                throw new LogicException(ExceptionMessage.INVALID_CREDENTIALS);
            }

            var now = DateTime.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };
            claims.AddRange(identity.Claims);

            var encodedJwt = GetJwt(claims, now);

            var encryptedRefreshToken = GetRefreshToken(claims, identity.Name);

            var response = new DtoTokenResponse
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_tokenProviderOptions.Expiration.TotalSeconds,
                RefreshToken = encryptedRefreshToken
            };

            return response;
        }

        private async Task<DtoTokenResponse> GenerateToken(
            HttpContext context,
            string apiKey)
        {
            var identity = await _tokenProviderOptions.ApiKeyIdentityResolver(context, apiKey);
            if (identity == null)
            {
                throw new LogicException(ExceptionMessage.INVALID_CREDENTIALS);
            }

            var now = DateTime.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };
            claims.AddRange(identity.Claims);

            var encodedJwt = GetJwt(claims, now);

            var encryptedRefreshToken = GetRefreshToken(claims, identity.Name);

            var response = new DtoTokenResponse
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_tokenProviderOptions.Expiration.TotalSeconds,
                RefreshToken = encryptedRefreshToken
            };

            return response;
        }

        private string GetJwt(IEnumerable<Claim> claims, DateTime now)
        {
            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _tokenProviderOptions.Issuer,
                audience: _tokenProviderOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_tokenProviderOptions.Expiration),
                signingCredentials: _tokenProviderOptions.SigningCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string GetRefreshToken(IEnumerable<Claim> claims, string userId)
        {
            var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var key = CryptoUtility.CreateCryptographicallySecureGuid().ToByteArray();

            var timeKey = time.Concat(key).ToArray();
            var serializedClaims = JsonConvert.SerializeObject(claims, new ClaimConverter());
            var payload = new RefreshTokenPayloadModel
            {
                UserId = userId,
                Claims = serializedClaims,
                TimeWithKey = timeKey
            };
            var payloadByteArr = ObjectToByteArray(payload);
            var refreshToken = Convert.ToBase64String(payloadByteArr);

            var encryptedRefreshToken = CryptoUtility.Encrypt(refreshToken, _tokenProviderOptions.RefreshTokenSigningKey);

            return encryptedRefreshToken;
        }

        private DtoTokenResponse GetNewTokenFromRefreshToken(string tokenEncrypted)
        {
            var token = CryptoUtility.Decrypt(tokenEncrypted, _tokenProviderOptions.RefreshTokenSigningKey);
            byte[] data = Convert.FromBase64String(token);

            var obj = ByteArrayToObject(data) as RefreshTokenPayloadModel;
            var claims = JsonConvert.DeserializeObject<IEnumerable<Claim>>(obj.Claims, new ClaimConverter());

            var when = DateTime.FromBinary(BitConverter.ToInt64(obj.TimeWithKey, 0));
            if (when < DateTime.UtcNow.AddHours(-4))
            {
                throw new LogicException(ExceptionMessage.EXPIRED_REFRESH_TOKEN);
            }

            var userId = claims.First(x => x.Type == "sub").Value;

            var encodedJwt = GetJwt(claims, DateTime.UtcNow);
            if ((DateTime.UtcNow - when).Hours > 3)
            {
                tokenEncrypted = GetRefreshToken(claims, userId);
            }

            return new DtoTokenResponse
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_tokenProviderOptions.Expiration.TotalSeconds,
                RefreshToken = tokenEncrypted

            };
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            }

            if (options.ApiKeyIdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.ApiKeyIdentityResolver));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return new byte[0];
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();

            }
        }

        // Convert a byte array to an Object
        private object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
