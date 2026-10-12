using Banco.Core;

namespace Banco.Tests;

public class BankAccountTests
{
    // xUnit crea una instancia nueva de esta clase por cada prueba
    // así que cada prueba recibe una cuenta recién creada.
    private readonly BankAccount _account = new("Juan", initialBalance: 100m);

    //Constructor

    [Fact]
    public void Constructor_ValidData_InitializesOwnerAndBalance()
    {
        var account = new BankAccount("Allan", initialBalance: 50m);

        Assert.Equal("Allan", account.Owner);
        Assert.Equal(50m, account.Balance);
    }

    //Casos en los que falla
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidOwner_ThrowsArgumentException(string? owner)
    {
        Assert.Throws<ArgumentException>(() => new BankAccount(owner!, initialBalance: 0));
    }

    [Fact]
    public void Constructor_NegativeInitialBalance_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BankAccount("Luis", initialBalance: -1m));
    }

    // Depositar 

    [Fact]
    public void Deposit_ValidAmount_IncreasesBalance()
    {
        // Arrange
        decimal amount = 25.50m;

        // Act
        _account.Deposit(amount);

        // Assert
        Assert.Equal(125.50m, _account.Balance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Deposit_NonPositiveAmount_ThrowsAndKeepsBalance(decimal amount)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _account.Deposit(amount));

        Assert.Equal("amount", ex.ParamName);
        Assert.Equal(100m, _account.Balance);
    }

    // Retirar 

    [Theory]
    [InlineData(30, 70)]
    [InlineData(100, 0)] // caso límite: retirar todo el saldo
    public void Withdraw_AvailableAmount_DecreasesBalance(decimal amount, decimal expectedBalance)
    {
        _account.Withdraw(amount);

        Assert.Equal(expectedBalance, _account.Balance);
    }

    [Fact]
    public void Withdraw_AmountGreaterThanBalance_ThrowsInvalidOperationException()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => _account.Withdraw(100.01m));

        Assert.Equal("Fondos insuficientes.", ex.Message);
        Assert.Equal(100m, _account.Balance);
    }

    // Transferir

    [Fact]
    public void Transfer_SufficientFunds_MovesMoneyBetweenAccounts()
    {
        var destino = new BankAccount("Luis", initialBalance: 20m);

        _account.Transfer(destino, 40m);

        Assert.Equal(60m, _account.Balance);
        Assert.Equal(60m, destino.Balance);
    }

    [Fact]
    public void Transfer_InsufficientFunds_LeavesBothAccountsUnchanged()
    {
        var destino = new BankAccount("Luis", initialBalance: 20m);

        Assert.Throws<InvalidOperationException>(() => _account.Transfer(destino, 500m));

        Assert.Equal(100m, _account.Balance);
        Assert.Equal(20m, destino.Balance);
    }

    [Fact]
    public void Transfer_ToSameAccount_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => _account.Transfer(_account, 10m));
    }

    [Fact]
    public void Transfer_NullDestination_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _account.Transfer(null!, 10m));
    }
}
