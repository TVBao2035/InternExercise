using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        // implememt authentication for each services
        // handle search method
        // handle dto :))
        public async Task<AppReponse<SearchResponse<UserDTO>>> Search(SearchRequest request) 
        {
           var result = new  AppReponse<SearchResponse<UserDTO>>();
            try
            {
                SearchResponse<UserDTO> reponse = new SearchResponse<UserDTO>();
                reponse.Results = new List<UserDTO>();
                var query = GetQuerySearch(request.Filters);
                var users = query.Select(u => new UserDTO
                {
                    Name = u.Name,
                    Email = u.Email
                }).ToList();
               
               if(request.Sort is not null)
                {
                    if(request.Sort.IsASC)
                        users = users.OrderBy(u => u.GetType().GetProperty(request.Sort.FieldName)?.GetValue(u, null)).ToList();
                    else
                        users = users.OrderByDescending(u => u.GetType().GetProperty(request.Sort.FieldName).GetValue(u, null)).ToList();
                }
                int pageSize = request.PageSize != 0 ? request.PageSize : 1;
                int currPage = request.CurrPage != 0 ? request.CurrPage-1 : 0;
                int totalPage = users.Count / request.PageSize;
                int skip = pageSize * currPage;
                reponse.Results = users.Skip(skip).Take(pageSize).ToList();
                reponse.CurrPage = currPage + 1;
                reponse.TotalPages = totalPage;
                
                return result.SendReponse(200, "Success", reponse);
                
            }
            catch (Exception ex)
            {

                return result.SendReponse(404, ex.Message);
            }
        }
      
        public IQueryable<User> GetQuerySearch(List<SearchFilter> filters)
        {
            IQueryable<User> query = _userRepository.Query();
            if (filters is not null && filters.Count > 0)
            {
                foreach(var filter in filters)
                {
                    var value = filter.Value.ToLower().Trim();
                    switch (filter.FieldName.ToLower().Trim())
                    {
                        case "name":
                            query = query.Where(u => u.Name.Contains(value));
                            break;
                        case "email":
                            query = query.Where(u => u.Email.Equals(value));
                            break;
                        default:
                            break;
                    }
                }
            }
            return query;
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
                         ValidateIssuer = true,
                         ValidIssuer = _config["Auth:Issuer"],
                         ValidateAudience = true,
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
                if (claims.Count == 0) return response.SendReponse(401, "Payload is not valid");

                var code = Guid.Parse(claimPrincial.FindFirst(ClaimTypes.SerialNumber).Value);
                var emailUser = claimPrincial.FindFirst("Email").Value;
                var getRefresh =  _tokenRepository.Query(t => t.Code == code).FirstOrDefault();
                var user = _userRepository.Query(u => u.Email==emailUser).FirstOrDefault();

                if (getRefresh is null)
                {
                    return response.SendReponse(404, "Not found");
                }
                if(getRefresh.Expire <= DateTime.UtcNow)
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
            double expiredAccessToken = double.Parse(_config["Auth:ExpiredAccessToken"] ?? "60");
            DateTime expired = DateTime.UtcNow.AddSeconds(expiredAccessToken);
            var claims = GetClaims(user);
            claims.Add(new Claim (JwtRegisteredClaimNames.Exp, expired.ToString()));
            var token =  GenerateToken(claims, expired);
            return token;
        }
        private (string, DateTime, Guid) CreateRefreshToken(User user)
        {
            double expiredRefreshToken = double.Parse(_config["Auth:ExpiredRefreshToken"] ?? "3600");
            DateTime expired = DateTime.UtcNow.AddSeconds(expiredRefreshToken);
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
                notBefore: DateTime.UtcNow,
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenInfor);
        }


        public async Task<AppReponse<User>> GetById(Guid Id)
        {
            // dto
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
               
                if (await _userRepository.Query(u => u.Email == request.Email).FirstOrDefaultAsync() is not null)
                {
                    return result.SendReponse(404, "Email is exisiting");
                }
                User user = new User();
                user.Id = Guid.NewGuid();
                user.Email = request.Email;
                user.Password = "12345";
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
