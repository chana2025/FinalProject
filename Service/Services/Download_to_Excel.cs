using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using ClosedXML.Excel; // ודא שהוספת את חבילת ClosedXML לפרויקט שלך

namespace Service.Services
{
    public class Download_to_Excel
    {
        static void Main()
        {
            // === פרטי התחברות ל-SQL Server ===
            string connectionString = "Server=DESKTOP-1VUANBN;Database=DietSC1;Trusted_Connection=True;";

            // === השאילתה לשליפת הנתונים מהטבלה Product ===
            string query = "SELECT * FROM Products"; // תוכל לשנות כאן עמודות ספציפיות אם צריך
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    conn.Close();

                    if (dt.Rows.Count == 0)
                    {
                        Console.WriteLine("No data found in Product table.");
                    }
                    else
                    {
                        string filename = $"product_export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                        using (var workbook = new XLWorkbook())
                        {
                            workbook.Worksheets.Add(dt, "Products");
                            workbook.SaveAs(filename);
                        }
                        Console.WriteLine($"✅ Export successful: {filename}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("❌ Error: " + e.Message);
            }
        }
    }
}

