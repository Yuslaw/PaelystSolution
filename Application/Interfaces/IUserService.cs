using PaelystSolution.Application.Dtos;
using PaelystSolution.Domain.Entities;

namespace PaelystSolution.Application.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<UserViewModel>> Create(UserViewModel model);
        Task<BaseResponse<UserDto>> CheckInfo(LoginUserViewModel model);
        Task<BaseResponse<User>> Get(Guid id);
    }
}
