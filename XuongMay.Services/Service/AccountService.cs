﻿using MongoDB.Bson;
using MongoDB.Driver;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;

namespace XuongMay.Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IMongoCollection<Account> _accounts;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IMongoDatabase database, IUnitOfWork unitOfWork)
        {
            _accounts = database.GetCollection<Account>("Accounts");
            _unitOfWork = unitOfWork;
        }

        //Get account By Id
        public async Task<Account> GetAccountByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Account>();
            return await repository.GetByIdAsync(objectId);
        }

        //Get account By Role
        public async Task<IEnumerable<Account>> GetAccountsByRoleAsync(string role)
        {
            return await _accounts.Find(a => a.Role == role).ToListAsync();
        }

        //Get all accounts
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var repository = _unitOfWork.GetRepository<Account>();
            return await repository.GetAllAsync();
        }

        //Update account
        public async Task<Account> UpdateAccountAsync(string id, Account account)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Account>();
            var existingAccount = await repository.GetByIdAsync(objectId);
            if (existingAccount == null)
                return null;

            // Update thuộc tính
            existingAccount.Name = account.Name;
            existingAccount.Password = account.Password;
            existingAccount.Status = account.Status;
            existingAccount.Salary = account.Salary;

            repository.Update(existingAccount);

            return existingAccount;
        }

        public async Task<bool> DeleteAccountAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var repository = _unitOfWork.GetRepository<Account>();
            var existingAccount = await repository.GetByIdAsync(objectId);
            if (existingAccount == null)
                return false;

            // Update trạng thái thành Unavailable
            existingAccount.Status = "Unavailable";
         

            repository.Update(existingAccount);
            return true;
        }

        // Update account role
        public async Task UpdateAccountRoleAsync(string accountId, string newRole)
        {
            ObjectId objectId = ObjectId.Parse(accountId);

            // Update the role field of the account
            var updateDefinition = Builders<Account>.Update
                .Set(a => a.Role, newRole);

            // Execute the update
            var result = await _accounts.UpdateOneAsync(a => a.Id == objectId, updateDefinition);

            if (result.MatchedCount == 0)
            {
                throw new Exception("Account not found.");
            }
        }

    }
}