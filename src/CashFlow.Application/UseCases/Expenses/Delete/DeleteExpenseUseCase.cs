using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{

    public class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;
        private readonly IExpenseReadOnlyRepository _expenseReadOnly;
        public DeleteExpenseUseCase(
            IExpensesWriteOnlyRepository repository, 
            IUnitOfWork unitOfWork,
            ILoggedUser loggedUser,
            IExpenseReadOnlyRepository expenseReadOnly)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
            _expenseReadOnly = expenseReadOnly;
        }
        public async Task Execute(long Id)
        {
            var loggedUser = await _loggedUser.Get();

            var expense = await _expenseReadOnly.GetById(loggedUser, Id);

            if(expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            await _repository.Delete(Id);

            await _unitOfWork.Commit();
        }
    }
}
