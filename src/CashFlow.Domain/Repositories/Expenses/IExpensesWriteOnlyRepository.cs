using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
    /// <summary>
    /// this function return TRUE  if the deletion was successful otherwise returns FALSE
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<bool> Delete(long Id);
}
