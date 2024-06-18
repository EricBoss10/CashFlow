﻿using CashFlow.Application.UseCases.Users.Register;
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

    private RegisterUserUseCase CreateUseCase()
    {
        var mapper = MapperBuilder.Build();
        var UnitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripted = PasswordEncrypterBuilder.Build();
        var tokenGenerator = JwtTokenGenerateBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().Build();

        return new RegisterUserUseCase(mapper, passwordEncripted, readRepository, writeRepository, UnitOfWork, tokenGenerator);
    }
}