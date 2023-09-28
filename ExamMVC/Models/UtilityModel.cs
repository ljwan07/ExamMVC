using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ExamMVC.Models
{
    public class UtilityModel
    {
        public struct ConnectName
        {
            public static string ConnDB = "ConnDB";
        }
        ConnectName CN;


        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <param name="pSystemName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string pDBName)
        {
            return System.Web.Configuration.WebConfigurationManager.ConnectionStrings[pDBName].ConnectionString;
        }
    }
}