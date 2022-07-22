using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public static class VehicleAttributes
    {
        public static void SetAttribute(this DbVehicle dbVehicle, string attribute, object val)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET " + attribute + " = @val WHERE Id = @id");
            mySqlQuery.AddParameter("@val", val);
            mySqlQuery.AddParameter("@id", dbVehicle.Id);
            MySqlHandler.ExecuteSync(mySqlQuery);
        }

        public static dynamic GetAttributeInt(this DbVehicle dbVehicle, string attribute)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id");
            mySqlQuery.AddParameter("@id", dbVehicle.Id);
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

            mySqlResult.Reader.Read();

            dynamic result = mySqlResult.Reader.GetInt32(attribute);

            mySqlResult.Connection.Dispose();

            return result;
        }

        public static dynamic GetAttributeint(this DbVehicle dbVehicle, string attribute)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id");
            mySqlQuery.AddParameter("@id", dbVehicle.Id);
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

            mySqlResult.Reader.Read();

            dynamic result = mySqlResult.Reader.GetInt32(attribute);

            mySqlResult.Connection.Dispose();

            return result;
        }

        public static dynamic GetAttributeString(this DbVehicle dbVehicle, string attribute)
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id");
            mySqlQuery.AddParameter("@id", dbVehicle.Id);
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

            mySqlResult.Reader.Read();

            dynamic result = mySqlResult.Reader.GetString(attribute);

            mySqlResult.Connection.Dispose();

            return result;
        }
    }
}
