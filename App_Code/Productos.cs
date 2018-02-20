using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace pvAccom.Clases
{
    public class Productos
    {

    }

    public class EntradaProd
    {
        /*private int entID_;
        private string entProducto_;
        private string entProdDesc_;
        private int entCant_;
        private decimal entCosto_;
        private decimal entImporte_;*/

        public int entID { get; set; }
        public string entProducto { get; set; }
        public short entProdAlm { get; set; }
        public string entProdDesc { get; set; }
        public float entCant { get; set; }
        public float entCosto { get; set; }
        public float entImporte { get; set; }
        public float entPrecVtaUnit { get; set; }

        protected static SqlConnection ConeccSql()
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString);
            return sqlConn;
        }

        public static string guardaEntEncabezado(short entAlmacenID, string strDoc, DateTime strFechDoc, int cveProveedor,
            string strTipoDoc, float decSubTotal, float decImpuesto, float decTotal, decimal intSumProds, string usuID, List<EntradaProd> lstEntP, string entrada)
        {
            int entFolioID = -1;
            string respuesta = "";
            try
            {
                SqlConnection sqlConn = ConeccSql();
                using (sqlConn)
                {
                    sqlConn.Open();
                    using (SqlCommand sqlComm = new SqlCommand("spEntDocGuardar", sqlConn))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;
                        sqlComm.Parameters.AddWithValue("@entrada", entrada).DbType = DbType.Int32;
                        sqlComm.Parameters.Add("@entFolioID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        sqlComm.Parameters.AddWithValue("@entAlmacenID", entAlmacenID).DbType = DbType.Int16;
                        sqlComm.Parameters.AddWithValue("@entDocumento", strDoc).DbType = DbType.String;
                        sqlComm.Parameters.AddWithValue("@entDocTipo", strTipoDoc).DbType = DbType.String;
                        sqlComm.Parameters.AddWithValue("@entFechaDoc", strFechDoc).DbType = DbType.DateTime;
                        sqlComm.Parameters.AddWithValue("@cveProveedor", cveProveedor).DbType = DbType.Int32;
                        sqlComm.Parameters.AddWithValue("@entSubtotal", decSubTotal).DbType = DbType.Decimal;
                        sqlComm.Parameters.AddWithValue("@entImpuesto", decImpuesto).DbType = DbType.Decimal;
                        sqlComm.Parameters.AddWithValue("@entTotal", decTotal).DbType = DbType.Decimal;
                        sqlComm.Parameters.AddWithValue("@entSumaProductos", intSumProds).DbType = DbType.Decimal;
                        sqlComm.Parameters.AddWithValue("@usuID", usuID).DbType = DbType.String;

                        DataTable dt = new DataTable();
                        dt.Columns.Add("entDetID");
                        dt.Columns.Add("entProductoID");
                        dt.Columns.Add("entProdCant");
                        dt.Columns.Add("entProdCostoUnit");
                        dt.Columns.Add("entPrecVtaUnit");
                        dt.Columns.Add("entImporte");

                        foreach (EntradaProd articulo in lstEntP)
                        {
                            object[] valores = { Convert.ToInt32(articulo.entID), articulo.entProducto, Convert.ToDecimal(articulo.entCant), Convert.ToDecimal(convierteMontos(articulo.entCosto)), Convert.ToDecimal(articulo.entPrecVtaUnit), Convert.ToDecimal(convierteMontos(articulo.entImporte)) };
                            dt.Rows.Add(valores);
                        }

                        sqlComm.Parameters.AddWithValue("@detalle", dt);
                        sqlComm.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                        sqlComm.ExecuteNonQuery();

                        entFolioID = Convert.ToInt32(sqlComm.Parameters["@respuesta"].Value);
                        respuesta = entFolioID.ToString();

                    }
                }
                sqlConn.Close();
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            //Obtiene EL ID DE FOLIO ASIGNADO EN LA BD PARA PASARLO A guardaEntProd
            return respuesta;
        }

        public static void guardaEntProd(int intFolioID, List<EntradaProd> lstEntP, int isla)
        {
            foreach (EntradaProd entP in lstEntP)
            {
                guardaEntProd_(intFolioID, entP.entID, entP.entProducto, isla, entP.entCant, entP.entCosto, entP.entImporte);
            }
        }

        private static void guardaEntProd_(int intFolioID, int intEntID, string strProdID, int ProdAlm, float Cant, float Costo, float Importe)
        {
            SqlConnection sqlConn = ConeccSql();
            using (sqlConn)
            {
                sqlConn.Open();
                using (SqlCommand sqlComm = new SqlCommand("spEntProdGuardar", sqlConn))
                {
                    sqlComm.CommandType = CommandType.StoredProcedure;
                    sqlComm.Parameters.AddWithValue("@entFolioID", intFolioID).DbType = DbType.Int32;
                    sqlComm.Parameters.AddWithValue("@entDetID", intEntID).SqlDbType = SqlDbType.Int;
                    sqlComm.Parameters.AddWithValue("@entAlmacenID", ProdAlm).SqlDbType = SqlDbType.SmallInt;
                    sqlComm.Parameters.AddWithValue("@entProductoID", strProdID).SqlDbType = SqlDbType.VarChar;
                    sqlComm.Parameters.AddWithValue("@entProdCant", Cant).SqlDbType = SqlDbType.Decimal;
                    sqlComm.Parameters.AddWithValue("@entProdCosto", Costo).SqlDbType = SqlDbType.Decimal;
                    sqlComm.Parameters.AddWithValue("@entImporte", Importe).SqlDbType = SqlDbType.Decimal;
                    sqlComm.Parameters.AddWithValue("@entPrecioLista", Costo).Value = SqlDbType.Decimal;
                    SqlParameter ordenObten = new SqlParameter("@respuesta", 0);
                    ordenObten.Size = 1;
                    ordenObten.Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add(ordenObten).SqlDbType = SqlDbType.VarChar;
                    try
                    {
                        sqlComm.ExecuteNonQuery();
                        int insertFila = Convert.ToInt32(sqlComm.Parameters["@respuesta"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message + " " + sqlComm.Parameters["@respuesta"].Value.ToString();
                    }
                    sqlConn.Close();
                }
            }
        }

        private static string convierteMontos(float dato)
        {
            string importe = dato.ToString();
            if (importe != "")
            {
                importe = importe.Replace('$', ',');
                importe = importe.Replace(',', ' ');
                string valor = "";
                for (int j = 0; j < importe.Length; j++)
                {
                    if (char.IsDigit(importe[j]))
                        valor = valor.Trim() + importe[j];
                    else
                    {
                        if (importe[j] == '.')
                            valor = valor.Trim() + importe[j];
                    }
                }
                importe = valor.Trim();
            }
            else
                importe = "0";
            return importe;
        }
    }
}