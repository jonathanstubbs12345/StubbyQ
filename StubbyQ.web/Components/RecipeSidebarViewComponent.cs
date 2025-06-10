using Microsoft.AspNetCore.Mvc;
using StubbyQ.web.Components; // Adjust namespace if needed
using System.Collections.Generic;
using System.Data.SqlClient;
using StubbyQ.web.Models;

namespace StubbyQ.web.Components
{
    public class RecipeSidebarViewComponent : ViewComponent
    {
        private readonly string _connectionString;

        public RecipeSidebarViewComponent(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("StubbyQDb");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = GetRecipeCategories();
            return View(categories);
        }

        private List<RecipeCategory> GetRecipeCategories()
        {
            var categories = new List<RecipeCategory>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT RecipeCategoryID, Name, Hyperlink, RecipeLogoURL FROM RecipeCategories", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new RecipeCategory
                    {
                        RecipeCategoryID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Hyperlink = reader.GetString(2),
                        RecipeLogoURL = reader.IsDBNull(3) ? null : reader.GetString(3)
                    });
                }
            }

            return categories;
        }
    }
}
