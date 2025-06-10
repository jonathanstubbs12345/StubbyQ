using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace StubbyQ.web.Pages
{
    public class TestDbModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public string ResultMessage { get; set; }

        public TestDbModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            string connStr = _configuration.GetConnectionString("StubbyQDb");

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT GETDATE()", conn);
                    var result = cmd.ExecuteScalar();
                    ResultMessage = $"✅ Connected! Azure SQL time: {result}";
                }
            }
            catch (Exception ex)
            {
                ResultMessage = $"❌ Connection failed: {ex.Message}";
            }
        }
    }
}
