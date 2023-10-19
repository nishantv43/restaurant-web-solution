using Restaurant.Services.EmailAPI.Models.Dto;

namespace Restaurant.Services.EmailAPI.Service
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
    }
}
