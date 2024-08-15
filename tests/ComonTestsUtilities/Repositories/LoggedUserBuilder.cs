using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {   
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggerUser => loggerUser.Get()).ReturnsAsync(user);

        return mock.Object;
    }
}
