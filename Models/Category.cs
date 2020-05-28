using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NLog;

namespace NorthwindConsole.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual List<Product> Product { get; set; }


        public static void addCategories(Logger logger)
        {

            logger.Info("Add a Category");
            var database = new NorthwindContext();

            Category category = new Category();

            Console.WriteLine("Enter the  Category Name: ");
            category.CategoryName = Console.ReadLine();

            Console.WriteLine("Enter the Description: ");
            category.Description = Console.ReadLine();

            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);


            if (database.Categories.Any(c => c.CategoryName.ToLower().Equals(category.CategoryName.ToLower())))
            {
                isValid = false;
                results.Add(new ValidationResult("Whoops, this Category already exists", new string[] { category.CategoryName }));

            }



            if (isValid)
            {
                logger.Info("Validation Passed");
                database.addCategory(category);
                logger.Info($"{category.CategoryName} Added");

            }
            else if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu");
            Console.ReadLine();


        }

        public static void displayAllCategories(Logger logger)
        {
            logger.Info("Choice: Display All Categories");
            Console.WriteLine();
            var db = new NorthwindContext();

            var categories = db.Categories.OrderBy(c => c.CategoryId);

            foreach (var item in categories)
            {
                Console.WriteLine($"Name: {item.CategoryName}");
                Console.WriteLine($"Description: {item.Description}");
                Console.WriteLine("-------------------------");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu");
            Console.ReadLine();
        }

        public static void displayAllCategoriesAndProductsNotDiscontinued(Logger logger)
        {

            logger.Info("Choice: Display non-discontinued products by category");
            Console.WriteLine();
            var db = new NorthwindContext();

            var categories = (from p in db.Products
                              join c in db.Categories
                              on p.CategoryId equals c.CategoryId
                              where p.Discontinued == false
                              orderby c.CategoryId

                              select new
                              {
                                  c.CategoryId,
                                  c.CategoryName,
                                  p.ProductName

                              }).ToList();



            foreach (var item in categories)
            {

                Console.WriteLine($"{item.CategoryName}, {item.ProductName}");

            }

            logger.Info(categories.Count());

            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu");
            Console.ReadLine();
        }

        public static void displaySpecificCategoryAndProducts(Logger logger)
        {
            logger.Info("Choice: Display specific category and products");
            var db = new NorthwindContext();

            Console.WriteLine("Enter category Id to view products: ");

            var categories = db.Categories.OrderBy(c => c.CategoryId);

            foreach (var c in categories)
            {
                Console.WriteLine($"{c.CategoryId}) {c.CategoryName}");
            }

            if (UInt32.TryParse(Console.ReadLine(), out UInt32 choice))
            {
                if (db.Categories.Any(c => c.CategoryId == choice))
                {
                    var specificCategory = (from p in db.Products
                                            join c in db.Categories
                                            on p.CategoryId equals c.CategoryId
                                            where p.Discontinued == false && c.CategoryId == choice
                                            select new
                                            {
                                                c.CategoryName,
                                                p.ProductName

                                            }).ToList();


                    logger.Info($"({specificCategory.Count()}) results returned");
                    Console.WriteLine();

                    foreach (var cat in specificCategory)
                    {
                        Console.WriteLine($"{cat.CategoryName}, {cat.ProductName}");
                    }
                }
                else
                {
                    logger.Error($"Category Id {choice} does not exist");
                }
            }
            else
            {
                logger.Error("Not a valid entry");
            }


            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu");
            Console.ReadLine();

        }

        public static Category InputCategory(NorthwindContext db, Logger logger)
        {
            Category category = new Category();

            Console.WriteLine("Enter Category Name: ");
            category.CategoryName = Console.ReadLine();

            Console.WriteLine("Enter description: ");
            category.Description = Console.ReadLine();

            if (category.CategoryName != null && category.Description != null)
            {
                return category;
            }
            else
            {
                logger.Error("name and description cannot be empty");
            }

            return null;


        }

        public static Category GetCategory(NorthwindContext db, Logger logger)
        {
            var categories = db.Categories.OrderBy(c => c.CategoryId);

            foreach (Category c in categories)
            {
                Console.WriteLine($"ID:{c.CategoryId}) {c.CategoryName}");
            }

            if (int.TryParse(Console.ReadLine(), out int CategoryId))
            {
                Category category = db.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
                if (category != null)
                {
                    return category;
                }

            }
            logger.Error("Invalid category id");
            return null;
        }
    }
}