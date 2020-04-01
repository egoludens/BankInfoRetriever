using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace BankInfoRetriever.DB.Entities
{
    class DBAccounts : DBEntityBase
    {
        public string Account;
        public DateTime DateIn;
        public string AccountStatus;
        public string AccountCBRBIC;
        public string CK;
        public string RegulationAccountType;
        public string BIC;

        public DBAccounts(Dictionary<string, string> xAccounts)
        {
            Account = GetStringFromDictionary(xAccounts, "Account");
            DateIn = Convert.ToDateTime(GetStringFromDictionary(xAccounts, "DateIn"));
            AccountStatus = GetStringFromDictionary(xAccounts, "AccountStatus");
            AccountCBRBIC = GetStringFromDictionary(xAccounts, "AccountCBRBIC");
            CK = GetStringFromDictionary(xAccounts, "CK");
            RegulationAccountType = GetStringFromDictionary(xAccounts, "RegulationAccountType");
            BIC = GetStringFromDictionary(xAccounts, "BIC");
        }

        public override void WriteToDatabase(SqlConnection sqlConnection)
        {
            string selectQuery = "SELECT * FROM Accounts WHERE Account = @Account";

            using (SqlCommand sqlSelectCommand = new SqlCommand(selectQuery, sqlConnection))
            {
                sqlSelectCommand.Parameters.Add("@Account", SqlDbType.NChar).Value = Account;
                SqlDataReader sqlReader = sqlSelectCommand.ExecuteReader();
                bool rowExists = sqlReader.HasRows;
                sqlReader.Close();

                string insertUpdateQuery = rowExists
                    ? "UPDATE Accounts SET DateIn = @DateIn, AccountStatus = @AccountStatus, AccountCBRBIC = @AccountCBRBIC, CK = @CK, RegulationAccountType = @RegulationAccountType, BIC = @BIC WHERE Account = @Account"
                    : "INSERT INTO Accounts (Account, DateIn, AccountStatus, AccountCBRBIC, CK, RegulationAccountType, BIC) VALUES (@Account, @DateIn, @AccountStatus, @AccountCBRBIC, @CK, @RegulationAccountType, @BIC)";

                using (SqlCommand sqlInsertUpdateCommand = new SqlCommand(insertUpdateQuery, sqlConnection))
                {
                    sqlInsertUpdateCommand.Parameters.Add("@Account", SqlDbType.NChar).Value = Account;
                    sqlInsertUpdateCommand.Parameters.Add("@DateIn", SqlDbType.DateTime).Value = DateIn;
                    sqlInsertUpdateCommand.Parameters.Add("@AccountStatus", SqlDbType.NChar).Value = AccountStatus;
                    sqlInsertUpdateCommand.Parameters.Add("@AccountCBRBIC", SqlDbType.NChar).Value = AccountCBRBIC;
                    sqlInsertUpdateCommand.Parameters.Add("@CK", SqlDbType.NChar).Value = CK;
                    sqlInsertUpdateCommand.Parameters.Add("@RegulationAccountType", SqlDbType.NChar).Value = RegulationAccountType;
                    sqlInsertUpdateCommand.Parameters.Add("@BIC", SqlDbType.NChar).Value = BIC;

                    sqlInsertUpdateCommand.ExecuteNonQuery();
                }
            }

        }
    }
}
