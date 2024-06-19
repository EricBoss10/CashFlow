using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestsUtilities.Cryptography;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Successs()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var usecase = CreateUseCase();

        var result = await usecase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var usecase = CreateUseCase();

        var act = async () => await usecase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var UnitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripted = new PasswordEncrypterBuilder().Build();
        var tokenGenerator = JwtTokenGenerateBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrWhiteSpace(email) == false)
        {
            readRepository.ExistActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(mapper, passwordEncripted, readRepository.Build(), writeRepository, UnitOfWork, tokenGenerator);
    }
}
