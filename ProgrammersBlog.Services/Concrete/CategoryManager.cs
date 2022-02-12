﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Utilities;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;

namespace ProgrammersBlog.Services.Concrete
{
    public class CategoryManager:ManagerBase,ICategoryService
    {


        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper):base(mapper, unitOfWork)
        {
        }

        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {
            var category = await  UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    ResultStatus = ResultStatus.Success,
                    Category = category
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural:false),new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.NotFound(isPlural: false)
            });
        }
        //Bu şekilde xml dosyasını aktif etmek için service katmanına sağ tıklayıp  properties e girip build sekmesinde en altta xml kutucuğu işaretlenmeli.
        //XML uyarılarını kaldırmak için istediğimiz katmana tıklıyoruz yukarıdan view giriyoruz properties pages sekmesine giriyoruz. Build içinde warning kısmına uyarı kodunu ekliyoruz.

        /// <summary>
        /// Verilen Id parametresine ait kategorinin CategoryUpdateDto temsilini geriye döner.
        /// </summary>
        /// <param name="categoryId"> 0'dan büyük bir integer ID değeri</param>
        /// <returns> Asenkron bir  operasyon ile  Task olarak işlem  sonucu  DataResult tipinde  geriye döner.</returns>
        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = Mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            else
            {
                return new DataResult<CategoryUpdateDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), null);
            }
        }

        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(null);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    ResultStatus = ResultStatus.Success,
                    Categories = categories
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto
            {
                ResultStatus = ResultStatus.Error,
                Categories = null,
                Message = Messages.Category.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    ResultStatus = ResultStatus.Success,
                    Categories = categories
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => c.IsActive && !c.IsDeleted );
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    ResultStatus = ResultStatus.Success,
                    Categories = categories
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), null);
        }

        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {

            var category = Mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory=await UnitOfWork.Categories.AddAsync(category);
            await UnitOfWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Add(addedCategory.Name),new CategoryDto
            {
                Category = addedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.Add(addedCategory.Name)
            });


            //continuewith ile ilk fonksiyon bittikten sonra direk içerisinde yazılan fonksiyonun çalışmasını sağlıyor.
            //çok hızlı oluyor fakat önyüzde yönetimi zorlaştırabilir.
         
        }


        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category = Mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);
            category.ModifiedByName = modifiedByName;
            var updatedCategory =await UnitOfWork.Categories.UpdateAsync(category);
            await UnitOfWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success,
                Messages.Category.Update(updatedCategory.Name),new CategoryDto()
                {
                    Category =updatedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Update(updatedCategory.Name)
                });


        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId,string modifiedByName)
        {
            var result = await UnitOfWork.Categories.AnyAsync(a => a.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(a => a.Id == categoryId);
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await UnitOfWork.Categories.UpdateAsync(category);
                await UnitOfWork.SaveAsync();
                return new DataResult<CategoryDto>(ResultStatus.Success,
                    Messages.Category.Delete(deletedCategory.Name), new CategoryDto()
                    {
                        Category = deletedCategory,
                        ResultStatus = ResultStatus.Success,
                        Message = Messages.Category.Delete(deletedCategory.Name)
                    });
            }

            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(isPlural: false)
            });
        }

        public async  Task<IResult> HardDeleteAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(a => a.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(a => a.Id == categoryId);

                await UnitOfWork.Categories.DeleteAsync(category);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Category.HardDelete(category.Name));
            }
            return new Result(ResultStatus.Success, Messages.Category.NotFound(isPlural: false));
        }

        public async Task<IDataResult<int>> Count()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync();
            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }

        public async Task<IDataResult<int>> CountByNonDeleted()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync(c=>!c.IsDeleted);
            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, "Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }
    }
}