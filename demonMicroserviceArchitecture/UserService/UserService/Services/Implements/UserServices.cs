using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Models.DTOs;
using UserService.Models.Enities;
using UserService.Models.Requests;
using UserService.Models.Responses;
using UserService.Repositories.Implements;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services.Implements
{
    public class UserServices : IUserService
    {
        private IUserRepository _userRepository;
        private ITokenRepository _tokenRepository;
        private IConfiguration _config;

        public UserServices(
            IUserRepository userRepository, 
            IConfiguration config,
            ITokenRepository tokenRepository
        ) {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _config = config;
        }

        public async Task<AppReponse<SearchResponse<UserDTO>>> Search(SearchRequest request)
        {
           var result = new  AppReponse<SearchResponse<UserDTO>>();
            try
            {
                SearchResponse<UserDTO> reponse = new SearchResponse<UserDTO>();
                reponse.Results = new List<UserDTO>();
                IQueryable<User> query = _userRepository.Query();
               
                foreach (SearchFilter fileter in request.Filters) { 
                    reponse.Results.AddRange(GetValueFromSearch(fileter));
                }
                if (request.Sort.IsASC)
                    reponse.Results = reponse.Results.OrderBy(u => u.GetType().GetProperty(request.Sort.FieldName)?.GetValue(u, null)).ToList();
                else reponse.Results = reponse.Results.OrderByDescending(u => u.GetType().GetProperty(request.Sort.FieldName)?.GetValue(u, null)).ToList();
                return result.SendReponse(200, "Success", reponse);
            }
            catch (Exception ex)
            {

                return result.SendReponse(404, ex.Message);
            }
        }
      
        public List<UserDTO> GetValueFromSearch(SearchFilter filter)
        {
            var value = filter.Value.ToLower().Trim();
            IQueryable<User> query;
            switch (filter.FieldName.ToLower().Trim())
            {
                case "name":
                    query = _userRepository.Query(u => u.Name.ToLower().Trim().Equals(value)).AsQueryable();
                    break;
                case "email":
                    query = _userRepository.Query(u => u.Email.Equals(value)).AsQueryable();
                    break;
                default:
                    query = _userRepository.Query().AsQueryable();
                    break;
            }

            return query.Select(u => new UserDTO
            {
                Name = u.Name,
                Email = u.Email,
            }).ToList();
        }


        public async Task<AppReponse<LoginResponse>> Login(LoginRequest request)
        {
            var response = new AppReponse<LoginResponse>();
            try
            {
                User user = await _userRepository.Query(u => u.Email == request.Email).FirstOrDefaultAsync();
                if (user is null || !user.Password.Equals(request.Password)) 
                    return response.SendReponse(404, "Email or Pasword is wrong");
                Token token = new Token();
                token.AccessToken = CreateAccessToken(user);
                (token.RefreshToke, token.Expire, token.Code) = CreateRefreshToken(user);
                LoginResponse loginResponse = new LoginResponse();
                loginResponse.AccessToken = token.AccessToken;
                loginResponse.RefreshToken = token.RefreshToke;
                loginResponse.Name = user.Name;
                _tokenRepository.Insert(token);
                return response.SendReponse(200, "Success", loginResponse);
            }
            catch (Exception ex)
            {
                return response.SendReponse(404, ex.Message);
            }
        }

        public async   Task<AppReponse<LoginResponse>> ValidateRefreshToken(string refreshToken)
        {
            var response = new AppReponse<LoginResponse>();
            try
            {
                var claimPrincial = new JwtSecurityTokenHandler().ValidateToken(
                     refreshToken,
                     new TokenValidationParameters
                     {
                         ValidateIssuer = false,
                         ValidIssuer = _config["Auth:Issuer"],
                         ValidateAudience = false,
                         ValidAudience = _config["Auth:Audience"],
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Key"])),
                         RequireExpirationTime = true,
                         ClockSkew = TimeSpan.Zero
                     },
                     out _
                     );
                var claims = claimPrincial.Claims.ToList();
                var code = Guid.Parse(claims[3].ToString().Split(':')[2]);
                var emailUser = claims[1].ToString().Split(':')[1].Trim();
                var getRefresh =  _tokenRepository.Query(t => t.Code == code).FirstOrDefault();
                var user = _userRepository.Query(u => u.Email==emailUser).FirstOrDefault();
                if (getRefresh is null)
                {
                    return response.SendReponse(404, "Not found");
                }
                if(getRefresh.Expire <= DateTime.Now)
                {
                    return response.SendReponse(401, "Refresh Token Was Expired");
                }
                if (user is null) return response.SendReponse(404, "Not found user");


                LoginResponse loginResponse = new LoginResponse();
                Token token = new Token();
                token.AccessToken = CreateAccessToken(user);
                (token.RefreshToke, token.Expire, token.Code) = CreateRefreshToken(user);


                loginResponse.AccessToken = token.AccessToken;
                loginResponse.RefreshToken = token.RefreshToke;
                loginResponse.Name = user.Name;
                _tokenRepository.Insert(token);

                return response.SendReponse(200, "Success", loginResponse);
            }
            catch (Exception ex)
            {
                return response.SendReponse(404, ex.Message);
            }

        }
        private string CreateAccessToken(User user)
        {
            DateTime expired = DateTime.Now.AddSeconds(double.Parse(_config["Authe:ExpiredAccessToken"] ?? "60"));
            var claims = GetClaims(user);
            claims.Add(new Claim (JwtRegisteredClaimNames.Exp, expired.ToString()));
           
            return GenerateToken(claims, expired);
        }
        private (string, DateTime, Guid) CreateRefreshToken(User user)
        {
            DateTime expired = DateTime.Now.AddSeconds(double.Parse(_config["Authe:ExpiredRefreshToken"]??"30600"));
            Guid code = Guid.NewGuid();
            var claims = GetClaims(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, expired.ToString()));
            claims.Add(new Claim(ClaimTypes.SerialNumber, code.ToString()));
            var token = GenerateToken(claims, expired);
            return (token, expired, code);
        }

        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("Name", user.Name),
                new Claim("Email", user.Email)
            };
            return claims;
        }
        private string GenerateToken(List<Claim> claims, DateTime time)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenInfor = new JwtSecurityToken(
                issuer: _config["Auth:Issuer"],
                audience: _config["Auth:Audience"],
                claims,
                expires: time,
                notBefore: DateTime.Now,
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenInfor);
        }


        public async Task<AppReponse<User>> GetById(Guid Id)
        {
            var result = new AppReponse<User>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == Id).FirstOrDefaultAsync();
                if(user == null)
                {
                    return result.SendReponse(404, "Not found user");
                }

                return result.SendReponse(200, "Success", user);
            }catch(Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }
        public async Task<AppReponse<UserDTO>> Create(UserDTO request)
        {
            var result = new AppReponse<UserDTO>();
            try
            {
                User user = await _userRepository.Query(u => u.Email == request.Email).FirstOrDefaultAsync();
                if(user is not null)
                {
                    return result.SendReponse(404, "Email is exisiting");
                }
                user = new User();
                user.Id = Guid.NewGuid();
                user.Email = request.Email;
                user.Name = request.Name;
                _userRepository.Insert(user);
                return result.SendReponse(200, "Success", request);
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async  Task<AppReponse<UserDTO>> Delete(Guid Id)
        {
            var result = new AppReponse<UserDTO>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == Id).FirstOrDefaultAsync();
                if (user is  null)
                {
                    return result.SendReponse(404, "Not Found User");
                }
                _userRepository.Delete(user);
                return result.SendReponse(200, "Success", new UserDTO
                {
                    Email = user.Email,
                    Name = user.Name,
                });

            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async  Task<AppReponse<List<User>>> GetAll()
        {
            var result = new AppReponse<List<User>>();
            try
            {
                List<User> listUser = await _userRepository.Query().ToListAsync();
                return result.SendReponse(200, "Success", listUser);
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async Task<AppReponse<UserDTO>> Update(User request)
        {
            var result = new AppReponse<UserDTO>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == request.Id).FirstOrDefaultAsync();
                if (user is null) return result.SendReponse(404, "Not found user");
                user.Email = request.Email;
                user.Name = request.Name;
                _userRepository.Update(user);
                return result.SendReponse(200, "Success", new UserDTO
                {
                    Email = request.Email,
                    Name = request.Name,
                });
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }
    }
}
