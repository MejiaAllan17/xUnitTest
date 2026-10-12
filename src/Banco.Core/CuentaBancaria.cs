namespace Banco.Core;
/// Cuenta bancaria de ejemplo con depósitos, retiros y transferencias.
public class BankAccount
{
    public string Owner { get; }

    public decimal Balance { get; private set; }

    public BankAccount(string owner, decimal initialBalance = 0)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("El propietario es obligatorio.", nameof(owner));

        if (initialBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(initialBalance), "El saldo inicial no puede ser negativo.");

        Owner = owner;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        ValidateAmount(amount);
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        ValidateAmount(amount);

        if (amount > Balance)
            throw new InvalidOperationException("Fondos insuficientes.");

        Balance -= amount;
    }

    public void Transfer(BankAccount destination, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(destination);

        if (ReferenceEquals(this, destination))
            throw new InvalidOperationException("No se puede transferir a la misma cuenta.");

        Withdraw(amount);
        destination.Deposit(amount);
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "El monto debe ser mayor que cero.");
    }
}
