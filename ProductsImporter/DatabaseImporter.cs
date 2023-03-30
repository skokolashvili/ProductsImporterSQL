using System.Data;
using System.Data.SqlClient;

public static class DatabaseImporter
{
    private const string ConnectionString = @"Server = DESKTOP-QOITCGS; Database = G16_Catalogue; Integrated Security = True; TrustServerCertificate = True";

    public static void ImportCatalogue(IEnumerable<Category> catalogue)
    {
        using (SqlConnection connection = new(ConnectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("InsertCategories_sp", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar);
                cmd.Parameters.Add("@CategoryCode", SqlDbType.NVarChar);
                cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                using (SqlCommand cmd1 = new SqlCommand("InsertProducts_sp", connection))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@ProductName", SqlDbType.NVarChar);
                    cmd1.Parameters.Add("@ProductCode", SqlDbType.NVarChar);
                    cmd1.Parameters.Add("@Price", SqlDbType.Money);
                    cmd1.Parameters.Add("@IsDeleted", SqlDbType.Bit);
                    foreach (var category in catalogue)
                    {
                        cmd.Parameters["@CategoryName"].Value = category.CategoryName;
                        cmd.Parameters["@CategoryCode"].Value = category.CategoryCode;
                        cmd.Parameters["@IsDeleted"].Value = category.IsDeleted;
                        cmd.ExecuteNonQuery();
                        foreach (var product in category.Products)
                        {
                            cmd1.Parameters["@ProductName"].Value = product.ProductName;
                            cmd1.Parameters["@ProductCode"].Value = product.ProductCode;
                            cmd1.Parameters["@Price"].Value = product.ProductPrice;
                            cmd1.Parameters["@IsDeleted"].Value = product.IsDeleted;
                            cmd1.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
