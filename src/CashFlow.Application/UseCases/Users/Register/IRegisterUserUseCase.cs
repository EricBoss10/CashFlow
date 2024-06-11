using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Register;

public interface IRegisterUserUseCase
{
    public Task<ResponseUserJson> Execute(RegisterUserUseCase request);
}
