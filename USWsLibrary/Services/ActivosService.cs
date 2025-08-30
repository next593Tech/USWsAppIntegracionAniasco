using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    public class ActivosService
    {
        private DataModel _db;

        public ActivosService()
        {
            _db = new DataModel();
        }

        public List<ACT_ACTIVO> ListActivos(string empleadoID ){
            List<ACT_ACTIVO> ListActivo = new List<ACT_ACTIVO>();


            try
            {
                SqlParameter pEmpleadoID = new SqlParameter();
                pEmpleadoID.ParameterName = "EmpleadoID";
                pEmpleadoID.DbType = System.Data.DbType.String;
                pEmpleadoID.Direction = System.Data.ParameterDirection.Input;
                pEmpleadoID.Size = 11;
                pEmpleadoID.Value = empleadoID;

                object[] parameters = new object[] { pEmpleadoID };



                ListActivo = _db.Database.SqlQuery<ACT_ACTIVO>
                    ("dbo.OFFLINE_ACT_ACTIVOS_LISTADO  @EmpleadoID", parameters).ToList();
            }
            catch(SqlException E)
            {

            }catch(Exception G)
            {

            }
       
            return ListActivo;
        }


        public List<ACT_ASIGNACIONES> ListAsignaciones(string EmpleadoID)
        {

            List<ACT_ASIGNACIONES> ListAsignaciones = new List<ACT_ASIGNACIONES>();

            try
            {

                SqlParameter pEmpleadoID = new SqlParameter();
                pEmpleadoID.ParameterName = "EmpleadoID";
                pEmpleadoID.DbType = System.Data.DbType.String;
                pEmpleadoID.Direction = System.Data.ParameterDirection.Input;
                pEmpleadoID.Size = 11;
                pEmpleadoID.Value = EmpleadoID;
                object[] parameters = new object[] { pEmpleadoID };


                ListAsignaciones = _db.Database.SqlQuery<ACT_ASIGNACIONES>
                    ("dbo.OFFLINE_ACT_ASIGNACIONES @EmpleadoID", parameters).ToList();

            }
            catch(SqlException E)
            {


            }catch(Exception G)
            {

            }
        
            return ListAsignaciones;

        }

        public SimpleReturn UpadteEstado(AsignacionInput input)
        {
            SimpleReturn retorno = new SimpleReturn();
            ErrorItem errorItem = new ErrorItem();



            try { 
         
                SqlParameter pEstado = new SqlParameter();
                pEstado.ParameterName = "Estado";
                pEstado.DbType = System.Data.DbType.String;
                pEstado.Direction = System.Data.ParameterDirection.Input;
                pEstado.Size = 14;
                pEstado.Value = input.Estado;


                SqlParameter pFecha = new SqlParameter();
                pFecha.ParameterName = "Fecha";
                pFecha.DbType = System.Data.DbType.Date;
                pFecha.Direction = System.Data.ParameterDirection.Input;
                //pFecha.Size = 11;
                pFecha.Value = input.Fecha;

                SqlParameter pHora = new SqlParameter();
                pHora.ParameterName = "Hora";
                pHora.SqlDbType = System.Data.SqlDbType.Time;
                pHora.Direction = System.Data.ParameterDirection.Input;
                //pFecha.Size = 11;
                pHora.Value = input.Hora;






                SqlParameter pUsuario = new SqlParameter();
                pUsuario.ParameterName = "Usuario";
                pUsuario.DbType = System.Data.DbType.String;
                pUsuario.Direction = System.Data.ParameterDirection.Input;
                pUsuario.Size = 11;
                pUsuario.Value = input.Usuario;


                SqlParameter pAsignacionID = new SqlParameter();
                pAsignacionID.ParameterName = "AsignacionID";
                pAsignacionID.DbType = System.Data.DbType.String;
                pAsignacionID.Direction = System.Data.ParameterDirection.Input;
                pAsignacionID.Size = 11;
                pAsignacionID.Value = input.AsignacionID;




                object[] parameters = new object[] { pEstado, pUsuario, pAsignacionID,pFecha,pHora };

                errorItem = _db.Database.SqlQuery<ErrorItem>("dbo.OFFLINE_ACT_ASIGNACIONES_UPDATE @Estado,@Usuario,@AsignacionID,@Fecha,@Hora", parameters).FirstOrDefault();

                if (errorItem != null)
                {
                    if (errorItem.ErrorMessage.Length == 0)
                    {
                        retorno.DocumentId = "0";
                        retorno.ExistError = false;
                        retorno.ErrorIfExist = errorItem;
                    }
                    else
                    {
                        retorno.DocumentId = "0";
                        retorno.ExistError = true;
                        retorno.Text = errorItem.ErrorMessage;
                        retorno.ErrorIfExist = errorItem;
                    }

                }



            }catch(SqlException E)
            {

            }catch(Exception g)
            {

            }

            return retorno;

        }


        public SimpleReturn UpdateActivo(ActivoInput input)
        {
            SimpleReturn retorno = new SimpleReturn();
            ErrorItem errorItem = new ErrorItem();
            try
            {

                SqlParameter pActivoID = new SqlParameter();
                pActivoID.ParameterName = "ActivoID";
                pActivoID.DbType = System.Data.DbType.String;
                pActivoID.Direction = System.Data.ParameterDirection.Input;
                pActivoID.Size = 11;
                pActivoID.Value = input.AcitvoID;



                SqlParameter pEdificio = new SqlParameter();
                pEdificio.ParameterName = "Edificio";
                pEdificio.DbType = System.Data.DbType.String;
                pEdificio.Direction = System.Data.ParameterDirection.Input;
                pEdificio.Size = 40;
                pEdificio.Value = input.Edificio;



                SqlParameter pPiso = new SqlParameter();
                pPiso.ParameterName = "Piso";
                pPiso.DbType = System.Data.DbType.String;
                pPiso.Direction = System.Data.ParameterDirection.Input;
                pPiso.Size = 40;
                pPiso.Value = input.Piso;


                SqlParameter pRoom = new SqlParameter();
                pRoom.ParameterName = "Room";
                pRoom.DbType = System.Data.DbType.String;
                pRoom.Direction = System.Data.ParameterDirection.Input;
                pRoom.Size = 40;
                pRoom.Value = input.Room;



                SqlParameter pReceptor = new SqlParameter();
                pReceptor.ParameterName = "Receptor";
                pReceptor.DbType = System.Data.DbType.String;
                pReceptor.Direction = System.Data.ParameterDirection.Input;
                pReceptor.Size = 40;
                pReceptor.Value = input.Receptor;






                SqlParameter pHorasUso = new SqlParameter();
                pHorasUso.ParameterName = "HorasUso";
                pHorasUso.DbType = System.Data.DbType.Int32;
                pHorasUso.Direction = System.Data.ParameterDirection.Input;
                pHorasUso.Size = 40;
                pHorasUso.Value = input.HorasUsadas;
                

                object[] parameters = new object[] { pActivoID, pEdificio, pPiso, pRoom, pReceptor,pHorasUso };

                errorItem = _db.Database.SqlQuery<ErrorItem>("dbo.OFFLINE_ACT_ACTIVOS_UPDATE @ActivoID ,@Edificio,@Piso,@Room,@Receptor,@HorasUso", parameters).FirstOrDefault();

                if (errorItem != null)
                {
                    if (errorItem.ErrorMessage.Length == 0)
                    {
                        retorno.DocumentId = "0";
                        retorno.ExistError = false;
                        retorno.ErrorIfExist = errorItem;
                    }
                    else
                    {
                        retorno.DocumentId = "0";
                        retorno.ExistError = true;
                        retorno.Text = errorItem.ErrorMessage;
                        retorno.ErrorIfExist = errorItem;
                    }

                }


            }
            catch(SqlException e)
            {

            }catch(Exception e)
            {

            }

            return retorno;
        }



    }
}
