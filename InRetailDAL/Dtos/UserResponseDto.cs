using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class UserResponseDto
    {
        public UserResponseDto()
        { }
        public UserResponseDto(int id, string userName, int branchId, string token, string refreshToken)
        {
            Id = id;
            UserName = userName;
            BranchId = branchId;
            Token = token;
            RefreshToken = refreshToken;
        }
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string UserName { get; set; }
        public int BranchId { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        //[JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
