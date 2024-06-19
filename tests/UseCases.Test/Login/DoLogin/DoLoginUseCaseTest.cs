using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommonTestsUtilities.Cryptography;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Mapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;
        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();

        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Password);

        var act = async ()=> await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        var user = UserBuilder.Build();

        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    private DoLOginUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? password = null)
    {
        var passwordEncripted = new PasswordEncrypterBuilder().Verify(password).Build();
        var tokenGenerator = JwtTokenGenerateBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
   

        return new DoLOginUseCase(readRepository, passwordEncripted, tokenGenerator);
    }
}
