using System;
using System.Data.Entity;

namespace NorthwindConsole.Models
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext() : base("name=NorthwindContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public void addProduct(Product product)
        {
            this.Products.Add(product);
            this.SaveChanges();
        }

        public void addCategory(Category category)
        {
            this.Categories.Add(category);
            this.SaveChanges();
        }
        public void EditCategory(Category UpdatedCategory)
        {
            Category category = this.Categories.Find(UpdatedCategory.CategoryId);
            category.CategoryName = UpdatedCategory.CategoryName;
            category.Description = UpdatedCategory.Description;
            this.SaveChanges();
        }

        public void deleteCategory(Category category)
        {
            this.Categories.Remove(category);
            this.SaveChanges();
        }
        public void deleteProduct(Product product)
        {
            this.Products.Remove(product);
            this.SaveChanges();
        }
    }
}
