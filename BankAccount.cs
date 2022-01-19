using System;
using System.Collections.Generic;

namespace MyApp
{
  public class BankAccount
  {
    private static int accountNumberSeed = 1234567890;
    public string Number { get; }
    public string Owner { get; set; }
    public decimal Balance
    {
      get
      {
        decimal balance = 0;
        foreach (var item in allTransactions)
        {
          balance += item.Amount;
        }
        return balance;
      }
    }

    private readonly decimal minimumBalance;

    public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0) { }

    public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
    {
      this.Number = accountNumberSeed.ToString();
      accountNumberSeed++;

      this.Owner = name;
      this.minimumBalance = minimumBalance;

      if (initialBalance > 0)
        MakeDeposit(initialBalance, DateTime.Now, "Initial balance");
    }

    private List<Transaction> allTransactions = new List<Transaction>();

    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
      if (amount <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
      }

      var deposit = new Transaction(amount, date, note);
      allTransactions.Add(deposit);
    }

    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
      if (amount <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawl must be positive");
      }

      var overdraftTransaction = CheckWithDrawlLimit(Balance - amount < minimumBalance);


      var withdrawal = new Transaction(-amount, date, note);
      allTransactions.Add(withdrawal);

      Console.WriteLine(overdraftTransaction);
      Console.WriteLine(minimumBalance);

      if (overdraftTransaction != null)
      {
        allTransactions.Add(overdraftTransaction);
      }
    }

    protected virtual Transaction? CheckWithDrawlLimit(bool isOverdrawn)
    {
      if (isOverdrawn)
      {
        throw new InvalidOperationException("Not suffiecient funds for this transaction");
      }
      else
      {
        return default;

      }
    }

    public string GetAccountHistory()
    {
      var report = new System.Text.StringBuilder();

      decimal balance = 0;
      report.AppendLine("Date\t\tAmount\tBalance\tNote");

      foreach (var item in allTransactions)
      {
        balance += item.Amount;
        report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
      }

      return report.ToString();

    }

    public virtual void PerformMonthEndTransactions() { }
  }
}