public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task AddAsync(Account account);
    void Update(Account account);
}