//using Microsoft.Data.SqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using System.Data.SqlClient;
//using ClosedXML.Excel; // ודא שהוספת את חבילת ClosedXML לפרויקט שלך

//namespace Service.Services
//{
//    public class Download_to_Excel
//    {
//        static void Main()
//        {
//            // === פרטי התחברות ל-SQL Server ===
//            string connectionString = "Server=DESKTOP-1VUANBN;Database=DietSC1;Trusted_Connection=True;";

//            // === השאילתה לשליפת הנתונים מהטבלה Product ===
//            string query = "SELECT * FROM Products"; // תוכל לשנות כאן עמודות ספציפיות אם צריך
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    SqlCommand cmd = new SqlCommand(query, conn);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);
//                    conn.Close();

//                    if (dt.Rows.Count == 0)
//                    {
//                        Console.WriteLine("No data found in Product table.");
//                    }
//                    else
//                    {
//                        string filename = $"product_export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
//                        using (var workbook = new XLWorkbook())
//                        {
//                            workbook.Worksheets.Add(dt, "Products");
//                            workbook.SaveAs(filename);
//                        }
//                        Console.WriteLine($"✅ Export successful: {filename}");
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("❌ Error: " + e.Message);
//            }
//        }
//    }
//}

using Microsoft.Data.SqlClient; // ספרייה לעבודה מול SQL Server
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel; // ספרייה ליצירת קבצי Excel בצורה נוחה

namespace Service.Services
{
    public class Download_to_Excel
    {
        static void Main()
        {
            // === הגדרת מחרוזת התחברות ל-SQL Server ===
            string connectionString = "Server=DESKTOP-1VUANBN;Database=DietSC1;Trusted_Connection=True;";

            // === שאילתת SQL לשליפת כל המידע מטבלת Products ===
            string query = "SELECT * FROM Products";

            try
            {
                // יצירת חיבור למסד הנתונים
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // פתיחת החיבור

                    // הגדרת הפקודה להרצת השאילתה
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // שימוש ב-DataAdapter למילוי טבלת נתונים (DataTable)
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt); // מילוי הנתונים מהמסד אל תוך הטבלה
                    conn.Close(); // סגירת החיבור

                    // בדיקה אם התקבלו תוצאות
                    if (dt.Rows.Count == 0)
                    {
                        Console.WriteLine("לא נמצאו נתונים בטבלה Products.");
                    }
                    else
                    {
                        // יצירת שם קובץ לפי תאריך ושעה
                        string filename = $"product_export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                        // יצירת קובץ Excel עם ClosedXML
                        using (var workbook = new XLWorkbook())
                        {
                            // הוספת הגיליון לקובץ עם שם Products
                            workbook.Worksheets.Add(dt, "Products");

                            // שמירת הקובץ
                            workbook.SaveAs(filename);
                        }

                        Console.WriteLine($"✅ הקובץ נוצר בהצלחה: {filename}");
                    }
                }
            }
            catch (Exception e)
            {
                // הדפסת שגיאה במקרה של תקלה
                Console.WriteLine("❌ שגיאה: " + e.Message);
            }
        }
    }
}
