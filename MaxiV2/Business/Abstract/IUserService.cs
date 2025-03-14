﻿using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GetUserClaims(int userId);
        IDataResult<List<OperationClaim>> GetOperationClaims();
        IResult AddClaim(UserOperationClaim operationClaim);
        IResult DeleteClaim(UserOperationClaim operationClaim);
        IDataResult<UserDto> GetUser(int id);
        IResult Add(User user);
        IResult Update(UserDto userForUpdate);
        IResult ChangePassword(ChangePasswordDto user);
        IDataResult<User> GetByMail(string email);
        IDataResult<List<UserDto>> GetUsers();
    }
}
