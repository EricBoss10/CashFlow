
using AutoMapper;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;

    public RegisterUserUseCase(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ResponseUserJson> Execute(RegisterUserUseCase request)
    {
        Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);

        return new ResponseUserJson
        {
            Name = user.Name,
        };

    }

    private void Validate(RegisterUserUseCase request)
    {
        var result = new RegisterUserValidator().Validate(request);

        if (result.IsValid == false)
        {
            var errorMessage = result.Error.Select(f => f.Message).ToList();

            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
