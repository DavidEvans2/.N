using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NorthwindConsole.Models;

namespace NorthwindConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {


                string choice;
                do
                {
                    Console.Clear();
                    Console.WriteLine("1) Add a Product");
                    Console.WriteLine("2) Display all Products");
                    Console.WriteLine("3) Display Active Products");
                    Console.WriteLine("4) Display Discontinued Products");
                    Console.WriteLine("5) Search Products");
                    Console.WriteLine("6) Add a  Category");
                    Console.WriteLine("7) Edit a  Category");
                    Console.WriteLine("8) Display all Categories");
                    Console.WriteLine("9) Display all non-discontinued items by Category");
                    Console.WriteLine("10) Display all non-discontinued items by specific Category");
                    Console.WriteLine("11) Delete a Category");
                    Console.WriteLine("12) Delete a  Product");
                    Console.WriteLine(" Enter the \"q\" button to quit");
                    choice = Console.ReadLine();

                    switch (choice)
                    {

                        case "1":
                            Product.addProducts(logger);
                            break;
                        case "2":
                            Product.displayAllProducts(logger);
                            break;
                        case "3":
                            Product.displayActiveProducts(logger);
                            break;
                        case "4":
                            Product.displayDiscontinuedProducts(logger);
                            break;
                        case "5":
                            Product.searchProducts(logger);
                            break;
                        case "6":
                            Category.addCategories(logger);
                            break;
                        case "7":

                            var db = new NorthwindContext();

                            Console.WriteLine("Choose category ID to edit: ");

                            var category = Category.GetCategory(db, logger);


                            if (category != null)
                            {
                                Category UpdatedCategory = Category.InputCategory(db, logger);

                                if (UpdatedCategory != null)
                                {
                                    UpdatedCategory.CategoryId = category.CategoryId;
                                    db.EditCategory(UpdatedCategory);
                                    logger.Info($"Category Id: {UpdatedCategory.CategoryId} updated");
                                }
                            }

                            Console.WriteLine();
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadLine();
                            break;
                        case "8":
                            Category.displayAllCategories(logger);
                            break;
                        case "9":
                            Category.displayAllCategoriesAndProductsNotDiscontinued(logger);
                            break;
                        case "10":
                            Category.displaySpecificCategoryAndProducts(logger);
                            break;
                        case "11":
                            db = new NorthwindContext();
                            Console.WriteLine("Select category ID to delete:");
                            var categoryToDelete = Category.GetCategory(db, logger);
                            try
                            {
                                db.deleteCategory(categoryToDelete);
                                logger.Info($"{categoryToDelete.CategoryName} deleted");
                            }
                            catch (Exception)
                            {
                                logger.Error("Cannot Delete a record that affects other tables");
                            }



                            Console.WriteLine();
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadLine();

                            break;

                        case "12":
                            db = new NorthwindContext();
                            Console.WriteLine("Select product ID to delete:");
                            var productToDelete = Product.GetProduct(db, logger);
                            try
                            {
                                db.deleteProduct(productToDelete);
                                logger.Info($"{productToDelete.ProductName} deleted");
                            }
                            catch (Exception)
                            {
                                logger.Error("Cannot Delete a record that affects other tables");
                            }



                            Console.WriteLine();
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadLine();

                            break;
                        default:
                            logger.Info("No option chosen");
                            break;
                    }


                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }



    }
}