using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public static class Attributes
    {
        public static void SetAttribute(this DbPlayer dbPlayer, string attribute, object val)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET " + attribute + " = @val WHERE Id = @id");
            mySqlQuery.AddParameter("@val", val);
            mySqlQuery.AddParameter("@id", dbPlayer.Id);
            MySqlHandler.ExecuteSync(mySqlQuery);
        }

        public static dynamic GetAttributeInt(this DbPlayer dbPlayer, string attribute)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Id = @id");
            mySqlQuery.AddParameter("@id", dbPlayer.Id);
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

            mySqlResult.Reader.Read();

            dynamic result = mySqlResult.Reader.GetInt32(attribute);

            mySqlResult.Connection.Dispose();

            return result;
        }

        public static dynamic GetAttributeint(this DbPlayer dbPlayer, string attribute)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Id = @id");
            mySqlQuery.AddParameter("@id", dbPlayer.Id);
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

            mySqlResult.Reader.Read();

            dynamic result = mySqlResult.Reader.GetInt32(attribute);

            mySqlResult.Connection.Dispose();

            return result;
        }

        public static dynamic GetAttributeString(this DbPlayer dbPlayer, string attribute)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Id = @id");
            mySqlQuery.AddParameter("@id", dbPlayer.Id);
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

            mySqlResult.Reader.Read();

            dynamic result = mySqlResult.Reader.GetString(attribute);

            mySqlResult.Connection.Dispose();

            return result;
        }
    }
}
