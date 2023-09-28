using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamMVC.Models;

namespace ExamMVC.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Route("Exam/GetDetail_Dialog")]
        public ActionResult GetDetail_Dialog(string pOrderID, string pProductID)
        {
            SqlConnection connExamDB = new SqlConnection(UtilityModel.GetConnectionString(UtilityModel.ConnectName.ConnDB));
            connExamDB.Open();

            try
            {
                string strSQL = string.Format(@"SELECT DISTINCT 
		                                                A.OrderID,
		                                                B.ProductID,
		                                                B.UnitPrice,
		                                                B.Quantity,
		                                                B.Discount 
                                                FROM Orders A LEFT JOIN [Order Details] B ON A.OrderID = B.OrderID
                                                WHERE A.OrderID = @OrderID 
                                                AND B.ProductID = @ProductID");
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@OrderID", pOrderID);
                parameters.Add("@ProductID", pProductID);
                var dt = ConnDBModel.ExecuteQuery(strSQL, parameters, connExamDB);

                OrdersDetail items = new OrdersDetail();
                List<OrdersDetail> data = new List<OrdersDetail>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    items = new OrdersDetail();
                    items.OrderID = dt.Rows[i]["OrderID"].ToString();
                    items.ProductID = dt.Rows[i]["ProductID"].ToString();
                    items.UnitPrice = String.Format("{0:N2}", double.Parse(dt.Rows[i]["UnitPrice"].ToString()));
                    items.Quantity = String.Format("{0:N0}", double.Parse(dt.Rows[i]["Quantity"].ToString()));
                    items.Discount = String.Format("{0:N2}", double.Parse(dt.Rows[i]["Discount"].ToString()));
                    data.Add(items);
                }

                return Content(JsonConvert.SerializeObject(data), "application/json");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connExamDB.Close();
                connExamDB.Dispose();
                connExamDB = null;
            }
        }


        
        [Route("Exam/UpdateOrdersDetail")]
        public ActionResult UpdateOrdersDetail(string jsonList)
        {
            DataTable dtParam = JsonConvert.DeserializeObject<DataTable>(jsonList);

            if (dtParam.Rows.Count > 0)
            {
                SqlConnection connExamDB = new SqlConnection(UtilityModel.GetConnectionString(UtilityModel.ConnectName.ConnDB));
                connExamDB.Open();

                try
                {
                    string strSQL = string.Format(@"UPDATE [Order Details] 
                                                SET UnitPrice = @UnitPrice,
                                                    Quantity = @Quantity,
                                                    Discount = @Discount 
                                                WHERE OrderID = @OrderID 
                                                AND ProductID = @ProductID");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@OrderID", dtParam.Rows[0]["OrderID"].ToString());
                    parameters.Add("@ProductID", dtParam.Rows[0]["ProductID"].ToString());
                    parameters.Add("@UnitPrice", float.Parse(dtParam.Rows[0]["UnitPrice"].ToString()));
                    parameters.Add("@Quantity", int.Parse(dtParam.Rows[0]["Quantity"].ToString()));
                    parameters.Add("@Discount", float.Parse(dtParam.Rows[0]["Discount"].ToString()));
                    var ret_result = ConnDBModel.ExecuteSQL(strSQL, parameters, connExamDB);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connExamDB.Close();
                    connExamDB.Dispose();
                    connExamDB = null;
                }

                ContentResult resultJson = new ContentResult();
                resultJson.ContentType = "application/json";
                resultJson.Content = JsonConvert.SerializeObject("Success"); ;
                return resultJson;

            }
            else
            {
                ContentResult resultJson = new ContentResult();
                resultJson.ContentType = "application/json";
                resultJson.Content = JsonConvert.SerializeObject("Fail"); ;
                return resultJson;
            }
        }


        [Route("Exam/DelOrdersDetail")]
        public ActionResult DelOrdersDetail(string pOrderID, string pProductID)
        {
            if (pOrderID != "" && pProductID != "")
            {
                SqlConnection connExamDB = new SqlConnection(UtilityModel.GetConnectionString(UtilityModel.ConnectName.ConnDB));
                connExamDB.Open();

                try
                {
                    string strSQL = string.Format(@"DELETE FROM [Order Details] 
                                                    WHERE OrderID = @OrderID 
                                                    AND ProductID = @ProductID");
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@OrderID", pOrderID);
                    parameters.Add("@ProductID", pProductID);
                    var ret_result = ConnDBModel.ExecuteSQL(strSQL, parameters, connExamDB);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connExamDB.Close();
                    connExamDB.Dispose();
                    connExamDB = null;
                }

                ContentResult resultJson = new ContentResult();
                resultJson.ContentType = "application/json";
                resultJson.Content = JsonConvert.SerializeObject("Success"); ;
                return resultJson;
            }
            else
            {
                ContentResult resultJson = new ContentResult();
                resultJson.ContentType = "application/json";
                resultJson.Content = JsonConvert.SerializeObject("Fail"); ;
                return resultJson;
            }
        }


        [HttpPost]
        [Route("Exam/GetData")]
        public ActionResult GetData(string pOrderID)
        {
            SqlConnection connExamDB = new SqlConnection(UtilityModel.GetConnectionString(UtilityModel.ConnectName.ConnDB));
            connExamDB.Open();

            try
            {
                string strSQL = string.Format(@"SELECT DISTINCT 
		                                                A.OrderID,
		                                                convert(varchar, A.OrderDate,111) AS OrderDate,
		                                                A.CustomerID,
		                                                A.EmployeeID,
		                                                A.ShipName,
		                                                A.ShipAddress,
		                                                B.ProductID,
		                                                B.UnitPrice,
		                                                B.Quantity,
		                                                B.Discount 
                                                FROM Orders A LEFT JOIN [Order Details] B ON A.OrderID = B.OrderID
                                                WHERE A.OrderID = @OrderID 
                                                ORDER BY A.OrderID");
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@OrderID", pOrderID);
                var dt = ConnDBModel.ExecuteQuery(strSQL, parameters, connExamDB);

                OrdersModel items = new OrdersModel();
                List<OrdersModel> data = new List<OrdersModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    items = new OrdersModel();
                    items.OrderID = dt.Rows[i]["OrderID"].ToString();
                    items.OrderDate = dt.Rows[i]["OrderDate"].ToString();
                    items.CustomerID = dt.Rows[i]["CustomerID"].ToString();
                    items.EmployeeID = dt.Rows[i]["EmployeeID"].ToString();
                    items.ShipName = dt.Rows[i]["ShipName"].ToString();
                    items.ShipAddress = dt.Rows[i]["ShipAddress"].ToString();
                    items.ProductID = dt.Rows[i]["ProductID"].ToString();
                    items.UnitPrice = String.Format("{0:N2}", double.Parse(dt.Rows[i]["UnitPrice"].ToString()));
                    items.Quantity = String.Format("{0:N0}", double.Parse(dt.Rows[i]["Quantity"].ToString()));
                    items.Discount = String.Format("{0:N2}", double.Parse(dt.Rows[i]["Discount"].ToString()));
                    data.Add(items);
                }

                return Content(JsonConvert.SerializeObject(data), "application/json");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connExamDB.Close();
                connExamDB.Dispose();
                connExamDB = null;
            }
        }
    }
}