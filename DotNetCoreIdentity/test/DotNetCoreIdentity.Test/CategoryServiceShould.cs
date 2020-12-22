using AutoMapper;
using DotNetCoreIdentity.Application;
using DotNetCoreIdentity.Application.CategoryServices.Dtos;
using DotNetCoreIdentity.Application.Shared;
using DotNetCoreIdentity.EF.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCoreIdentity.Test
{
    public class CategoryServiceShould
    {
        [Fact]
        public async Task CreateNewCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationUserDbContext>().UseInMemoryDatabase(databaseName: "Test_NewCategoryCreate").Options;
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            ApplicationResult<CategoryDto> result = new ApplicationResult<CategoryDto>();
            using (var inMemoryContext = new ApplicationUserDbContext(options))
            {
                var service = new CategoryService(inMemoryContext, mapper);
                CreateCategoryInput fakeCategory = new CreateCategoryInput
                {
                    CreatedById = Guid.NewGuid().ToString(), // Sahte kullanici
                    CreatedBy = "Tester1",
                    Name = "Lorem Ipsum",
                    UrlName = "lorem-ipsum"
                };
                result = await service.Create(fakeCategory);
            }

            using (var inMemoryContext = new ApplicationUserDbContext(options))
            {
                Assert.True(result.Succeeded);
                Assert.NotNull(result.Result);
                Assert.Equal(1, await inMemoryContext.Categories.CountAsync());
                var item = await inMemoryContext.Categories.FirstOrDefaultAsync();
                Assert.Equal("Tester1", item.CreatedBy);
                Assert.Equal("Lorem Ipsum", item.Name);
                Assert.Equal("lorem-ipsum", item.UrlName);
                Assert.Equal(result.Result.CreatedById, item.CreatedById);
            }
        }

        [Fact]
        public async Task UpdateCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationUserDbContext>().UseInMemoryDatabase(databaseName: "Test_UpdateCategory").Options;
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            ApplicationResult<CategoryDto> resultCreate = new ApplicationResult<CategoryDto>();
            // Bir yeni kategori oluştur.
            using (var inMemoryContext = new ApplicationUserDbContext(options))
            {
                var service = new CategoryService(inMemoryContext, mapper);
                CreateCategoryInput fakeCategory = new CreateCategoryInput
                {
                    CreatedById = Guid.NewGuid().ToString(), // Sahte kullanici
                    CreatedBy = "Tester1",
                    Name = "Lorem Ipsum",
                    UrlName = "lorem-ipsum"
                };

                resultCreate = await service.Create(fakeCategory);
            }
            ApplicationResult<CategoryDto> resultUpdate = new ApplicationResult<CategoryDto>();

            // Yeni kategori olustu mu ? test et ve var olan kategoriyi güncelle
            using (var inMemoryContext = new ApplicationUserDbContext(options))
            {
                // Create servid düzgün calisti mi ?
                Assert.True(resultCreate.Succeeded);
                Assert.NotNull(resultCreate.Result);
                // Update islemini yap!                

                var item = await inMemoryContext.Categories.FirstOrDefaultAsync();
                var service = new CategoryService(inMemoryContext, mapper);
                var fakeUpdate = new UpdateCategoryInput
                {
                    Id = item.Id,
                    CreatedById = item.CreatedById,
                    ModifiedById = Guid.NewGuid().ToString(),
                    ModifiedBy = "Tester2",
                    Name = "Lorem Ipsum Dolor",
                    UrlName = "lorem-ipsum-dolor"
                };
                // Update servici calistir
                resultUpdate = await service.Update(fakeUpdate);
            }
            // Update basarili mi kontrol et
            using (var inMemoryContext = new ApplicationUserDbContext(options))
            {
                // contextte kategori var mi ?
                Assert.Equal(1, await inMemoryContext.Categories.CountAsync());
                // update servis düzgün calisti mi ?
                Assert.True(resultUpdate.Succeeded);
                Assert.NotNull(resultUpdate.Result);
                // update islem basarili mi (context ten gelen veri ile string ifadeleri karsilastir)
                var item = await inMemoryContext.Categories.FirstAsync();
                Assert.Equal("Tester1", item.CreatedBy);
                Assert.Equal("Tester2", item.ModifiedBy);
                Assert.Equal("Lorem Ipsum Dolor", item.Name);
                Assert.Equal("lorem-ipsum-dolor", item.UrlName);
                Assert.Equal(resultUpdate.Result.ModifiedById, item.ModifiedById);
            }
        }
        // Get testi
        // Delete testi
    }
}
