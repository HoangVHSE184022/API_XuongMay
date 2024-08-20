﻿using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.CategoryModelViews;

namespace XuongMay.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Category>> GetCategoriesByPageAsync(int page, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<Category>();
            var categories = await repository.GetAllAsync();

            var pagedCategories = categories
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            return pagedCategories;
        }


        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var repository = _unitOfWork.GetRepository<Category>();
            return await repository.GetByIdAsync(objectId);
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryModelView categoryModelView)
        {
            var category = new Category
            {
                CategoryName = categoryModelView.CategoryName,
                CategoryDescription = categoryModelView.CategoryDescription,
                CategoryStatus = "Available"
            };

            var repository = _unitOfWork.GetRepository<Category>();
            await repository.InsertAsync(category);

            return category;
        }

        public async Task<Category> UpdateCategoryAsync(string id, UpdateCategoryModelView category)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return null;
            }

            var repository = _unitOfWork.GetRepository<Category>();
            var existingCategory = await repository.GetByIdAsync(objectId);
            if (existingCategory == null)
            {
                return null;
            }

            // Update các thuộc tính cần thiết
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDescription = category.CategoryDescription;
            existingCategory.CategoryStatus = category.CategoryStatus;

            await repository.UpdateAsync(existingCategory);


            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return false;
            }

            var repository = _unitOfWork.GetRepository<Category>();
            var existingCategory = await repository.GetByIdAsync(objectId);
            if (existingCategory == null)
            {
                return false;
            }

            // Update trạng thái thành Unavailable
            existingCategory.CategoryStatus = "Unavailable";
            await repository.UpdateAsync(existingCategory);


            return true;
        }
    }
}
