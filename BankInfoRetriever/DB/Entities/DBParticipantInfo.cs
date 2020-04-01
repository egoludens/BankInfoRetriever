using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace BankInfoRetriever.DB.Entities
{
    class DBParticipantInfo : DBEntityBase
    {
        public string BIC;
        public string ParticipantStatus;
        public string UID;
        public int XchType;
        public int Srvcs;
        public int PtType;
        public DateTime DateIn;
        public string Adr;
        public string Nnp;
        public string Tnp;
        public string Ind;
        public string Rgn;
        public string CntrCd;
        public string NameP;

        public DBParticipantInfo(Dictionary<string, string> xParticipantInfo)
        {
            BIC = GetStringFromDictionary(xParticipantInfo, "BIC");
            ParticipantStatus = GetStringFromDictionary(xParticipantInfo, "ParticipantStatus");
            UID = GetStringFromDictionary(xParticipantInfo, "UID");
            XchType = Convert.ToInt32(GetStringFromDictionary(xParticipantInfo, "XchType"));
            Srvcs = Convert.ToInt32(GetStringFromDictionary(xParticipantInfo, "Srvcs"));
            PtType = Convert.ToInt32(GetStringFromDictionary(xParticipantInfo, "PtType"));
            DateIn = Convert.ToDateTime(GetStringFromDictionary(xParticipantInfo, "DateIn"));
            Adr = GetStringFromDictionary(xParticipantInfo, "Adr", "");
            Nnp = GetStringFromDictionary(xParticipantInfo, "Nnp", "");
            Tnp = GetStringFromDictionary(xParticipantInfo, "Tnp", "");
            Ind = GetStringFromDictionary(xParticipantInfo, "Ind", "");
            Rgn = GetStringFromDictionary(xParticipantInfo, "Rgn", "");
            CntrCd = GetStringFromDictionary(xParticipantInfo, "CntrCd", "");
            NameP = GetStringFromDictionary(xParticipantInfo, "NameP");
        }

        public override void WriteToDatabase(SqlConnection sqlConnection)
        {
            string selectQuery = "SELECT * FROM ParticipantInfo WHERE BIC = @BIC";

            using (SqlCommand sqlSelectCommand = new SqlCommand(selectQuery, sqlConnection))
            {
                sqlSelectCommand.Parameters.Add("@BIC", SqlDbType.NChar).Value = BIC;
                SqlDataReader sqlReader = sqlSelectCommand.ExecuteReader();
                bool rowExists = sqlReader.HasRows;
                sqlReader.Close();

                string insertUpdateQuery = rowExists
                    ? "UPDATE ParticipantInfo SET ParticipantStatus = @ParticipantStatus, UID = @UID, XchType = @XchType, Srvcs = @Srvcs, PtType = @PtType, DateIn = @DateIn, Adr = @Adr, Nnp = @Nnp, Tnp = @Tnp, Ind = @Ind, Rgn = @Rgn, CntrCd = @CntrCd, NameP = @NameP WHERE BIC = @BIC"
                    : "INSERT INTO ParticipantInfo (BIC, ParticipantStatus, UID, XchType, Srvcs, PtType, DateIn, Adr, Nnp, Tnp, Ind, Rgn, CntrCd, NameP) VALUES (@BIC, @ParticipantStatus, @UID, @XchType, @Srvcs, @PtType, @DateIn, @Adr, @Nnp, @Tnp, @Ind, @Rgn, @CntrCd, @NameP)";

                using (SqlCommand sqlInsertUpdateCommand = new SqlCommand(insertUpdateQuery, sqlConnection))
                {
                    sqlInsertUpdateCommand.Parameters.Add("@BIC", SqlDbType.NChar).Value = BIC;
                    sqlInsertUpdateCommand.Parameters.Add("@ParticipantStatus", SqlDbType.NChar).Value = ParticipantStatus;
                    sqlInsertUpdateCommand.Parameters.Add("@UID", SqlDbType.NChar).Value = UID;
                    sqlInsertUpdateCommand.Parameters.Add("@XchType", SqlDbType.Int).Value = XchType;
                    sqlInsertUpdateCommand.Parameters.Add("@Srvcs", SqlDbType.Int).Value = Srvcs;
                    sqlInsertUpdateCommand.Parameters.Add("@PtType", SqlDbType.Int).Value = PtType;
                    sqlInsertUpdateCommand.Parameters.Add("@DateIn", SqlDbType.DateTime).Value = DateIn;
                    sqlInsertUpdateCommand.Parameters.Add("@Adr", SqlDbType.NChar).Value = Adr;
                    sqlInsertUpdateCommand.Parameters.Add("@Nnp", SqlDbType.NChar).Value = Nnp;
                    sqlInsertUpdateCommand.Parameters.Add("@Tnp", SqlDbType.NChar).Value = Tnp;
                    sqlInsertUpdateCommand.Parameters.Add("@Ind", SqlDbType.NChar).Value = Ind;
                    sqlInsertUpdateCommand.Parameters.Add("@Rgn", SqlDbType.NChar).Value = Rgn;
                    sqlInsertUpdateCommand.Parameters.Add("@CntrCd", SqlDbType.NChar).Value = CntrCd;
                    sqlInsertUpdateCommand.Parameters.Add("@NameP", SqlDbType.NChar).Value = NameP;
                    sqlInsertUpdateCommand.ExecuteNonQuery();
                }
            }
        }

    }
}
