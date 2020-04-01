using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace BankInfoRetriever.DB.Entities
{
    class DBBICDirectoryEntry : DBEntityBase
    {
        public string BIC;

        public DateTime operationDate;

        public DBBICDirectoryEntry(Dictionary<string, string> xBICDirectoryEntry, DateTime operationDate)
        {
            BIC = GetStringFromDictionary(xBICDirectoryEntry, "BIC");
            this.operationDate = operationDate;
        }

        public override void WriteToDatabase(SqlConnection sqlConnection)
        {
            string selectQuery = "SELECT * FROM BICDirectory WHERE BIC = @BIC";

            using (SqlCommand sqlSelectCommand = new SqlCommand(selectQuery, sqlConnection))
            {
                sqlSelectCommand.Parameters.Add("@BIC", SqlDbType.NChar).Value = BIC;
                SqlDataReader sqlReader = sqlSelectCommand.ExecuteReader();
                bool rowExists = sqlReader.HasRows;
                sqlReader.Close();

                string insertUpdateQuery = rowExists
                    ? "UPDATE BICDirectory SET Updated = @Updated, ChangeType = @ChangeType WHERE BIC = @BIC"
                    : "INSERT INTO BICDirectory (BIC, Created, Updated, ChangeType) VALUES (@BIC, @Created, @Updated, @ChangeType)";

                using (SqlCommand sqlInsertUpdateCommand = new SqlCommand(insertUpdateQuery, sqlConnection))
                {
                    sqlInsertUpdateCommand.Parameters.Add("@BIC", SqlDbType.NChar).Value = BIC;
                    sqlInsertUpdateCommand.Parameters.Add("@Created", SqlDbType.DateTime).Value = operationDate;
                    sqlInsertUpdateCommand.Parameters.Add("@Updated", SqlDbType.DateTime).Value = operationDate;
                    sqlInsertUpdateCommand.Parameters.Add("@ChangeType", SqlDbType.NVarChar).Value = rowExists ? "Update" : "Create";
                    sqlInsertUpdateCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
