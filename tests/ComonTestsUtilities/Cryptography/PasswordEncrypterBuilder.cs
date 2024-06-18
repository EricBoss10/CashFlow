using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestsUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    public static IPasswordEncripter Build()
    {
        var mock = new Mock<IPasswordEncripter>();

        mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("!dshfAkjhd");

        return mock.Object;
    }
}
