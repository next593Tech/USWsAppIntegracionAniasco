using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USWsLibrary.Models;
using USWsApp;
using USWsLibrary.ModelDobraDatabase;

namespace USWsLibrary.Services
{
	public class ClientesServices
	{
		#region Propiedades y campos
		private DataModel _db;
		#endregion

		#region Constructores

		public ClientesServices()
		{
			_db = new DataModel();

		}

		#endregion


		public ErrorSave saveClient(PagedList<CLI_CLIENTES> clients)
		{

			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in clients.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  ";

						try
						{
							if (db.CLI_CLIENTES.Any(cl => cl.ID == item.ID && cl.Código.Trim()==item.Código.Trim()))
							{
								
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								if (db.CLI_CLIENTES.Any(cl => cl.ID != item.ID && cl.Código.Trim() == item.Código.Trim()))
								{
									ErrorClienteCedula errorClienteCedula= new  ErrorClienteCedula();
									errorSave.errorExit = true;
									errorSave.errorMessage = errorSave.errorMessage + "id diferente y cedula igual:"+item.ID;
									//errorSave.ID.Add(item.ID);
								}
								else if(db.CLI_CLIENTES.Any(cl => cl.ID == item.ID && cl.Código != item.Código))
								{


									errorSave.errorExit = true;
									errorSave.errorMessage = errorSave.errorMessage + "id igual y cedula diferente:" + item.ID;
									//errorSave.ID.Add(item.ID);
								}
								else
								{
									db.CLI_CLIENTES.Add(item);
									db.SaveChanges();
								}
							
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


	


		public ErrorSave updateClient(CLI_CLIENTES clients)
		{
			using (DobraConnection db = new DobraConnection())
			{
				//  foreach(var item in clients.Results)
				//{
				db.Entry(clients).State = System.Data.Entity.EntityState.Modified;
				//  }
				db.SaveChanges();
			}
			return new ErrorSave();
		}

		public PagedList<CLI_CLIENTES> listClient(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_CLIENTES> cli = new PagedList<CLI_CLIENTES>();
			using (DobraConnection db = new DobraConnection())
			{
				cli.Results = db.CLI_CLIENTES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				cli.Total = cli.Results.Count;
				cli.Count = cli.Results.Count;
			}
			return cli;
		}


		public PagedList<INV_PRODUCTOS> listProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRODUCTOS> products = new PagedList<INV_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				products.Results = db.INV_PRODUCTOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				products.Total = products.Results.Count;
				products.Count = products.Results.Count;
			}
			return products;
		}

		public ErrorSave saveProducts(PagedList<INV_PRODUCTOS> products)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in products.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;


						Console.WriteLine(item.Código);
						try
						{
							if (db.INV_PRODUCTOS.Any(pro => pro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PRODUCTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{

					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_PRODUCTOS> listProductsByIDerror(ErrorSave errorSave)
		{
			PagedList<INV_PRODUCTOS> products = new PagedList<INV_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				products.Results = db.INV_PRODUCTOS.Where(e => errorSave.Listid.Contains(e.ID)).ToList();
				products.Total = products.Results.Count;
				products.Count = products.Results.Count;
			}
			return products;
		}


		public PagedList<INV_PRODUCTOS_EMPAQUES> listProductsEmpaqueByIDerror(ErrorSave errorSave)
		{
			PagedList<INV_PRODUCTOS_EMPAQUES> products = new PagedList<INV_PRODUCTOS_EMPAQUES>();
			using (DobraConnection db = new DobraConnection())
			{
				products.Results = db.INV_PRODUCTOS_EMPAQUES.Where(e => errorSave.Listid.Contains(e.ProductoID)).ToList();
				products.Total = products.Results.Count;
				products.Count = products.Results.Count;
			}
			return products;
		}


		public PagedList<INV_PRODUCTOS_PRECIOS> listProductoPrecioByIDerror(ErrorSave errorSave)
		{
			PagedList<INV_PRODUCTOS_PRECIOS> products = new PagedList<INV_PRODUCTOS_PRECIOS>();
			using (DobraConnection db = new DobraConnection())
			{
				products.Results = db.INV_PRODUCTOS_PRECIOS.Where(e => errorSave.Listid.Contains(e.ProductoID)).ToList();
				products.Total = products.Results.Count;
				products.Count = products.Results.Count;
			}
			return products;
		}




		public ErrorSave saveProductsListProductID(PagedList<INV_PRODUCTOS> products)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage = "ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in products.Results)
					{
						//errorSave.errorMessage = errorSave.errorMessage + "\n" + "ID:  " + item.ID;


			
						try
						{
							if (db.INV_PRODUCTOS.Any(pro => pro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PRODUCTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{

					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_PRODUCTOS_EMPAQUES> listPackagesProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRODUCTOS_EMPAQUES> packages = new PagedList<INV_PRODUCTOS_EMPAQUES>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRODUCTOS_EMPAQUES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_PRODUCTOS_EMPAQUES>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}


		public ErrorSave savePackageProductos(PagedList<INV_PRODUCTOS_EMPAQUES> products)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in products.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;


						try
						{
							if (db.INV_PRODUCTOS_EMPAQUES.Any(pro => pro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PRODUCTOS_EMPAQUES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{

					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_PRODUCTOS_PRECIOS> listPriceProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRODUCTOS_PRECIOS> packages = new PagedList<INV_PRODUCTOS_PRECIOS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRODUCTOS_PRECIOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_PRODUCTOS_PRECIOS>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave savePriceProducts(PagedList<INV_PRODUCTOS_PRECIOS> products)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";
			var productoIDOLD = "";
			var productoIDOLDnew = "";
			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in products.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						if (String.IsNullOrEmpty(productoIDOLDnew))
							productoIDOLDnew = item.ProductoID;
						

						try
						{
							//if(productoIDOLD.Equals())



							if (db.INV_PRODUCTOS_PRECIOS.AsNoTracking().Where(pro => pro.ID == item.ID).Count() > 0)
							{

								var old = db.INV_PRODUCTOS_PRECIOS.Find(item.ID);
								db.Entry(old).State = System.Data.Entity.EntityState.Detached;
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PRODUCTOS_PRECIOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_COMBOS> listComboProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_COMBOS> packages = new PagedList<INV_COMBOS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_COMBOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_COMBOS>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave saveCombos(PagedList<INV_COMBOS> products)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in products.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;


						try
						{
							if (db.INV_COMBOS.Any(pro => pro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_COMBOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_COMBOS_COMPONENTES> listComboComponentesProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_COMBOS_COMPONENTES> packages = new PagedList<INV_COMBOS_COMPONENTES>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_COMBOS_COMPONENTES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || e.ExportadoDate > lastUpdate).ToList<INV_COMBOS_COMPONENTES>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave saveComboComponente(PagedList<INV_COMBOS_COMPONENTES> comboComponentes)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in comboComponentes.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;


						try
						{
							if (db.INV_COMBOS_COMPONENTES.Any(pro => pro.ProductoID == item.ProductoID && pro.ComboID == item.ComboID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_COMBOS_COMPONENTES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;

		}


		/*public PagedList<INV_PD_BODEGA_STOCK> listPdBodegaStock(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PD_BODEGA_STOCK> packages = new PagedList<INV_PD_BODEGA_STOCK>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PD_BODEGA_STOCK.Where(e => e.ExportadoDate > lastUpdate).ToList<INV_PD_BODEGA_STOCK>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}*/

		/*public ErrorSave savePdBodegaStock(PagedList<INV_PD_BODEGA_STOCK> bodegaStock)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in bodegaStock.Results)
					{
						errorSave.errorMessage=item.ProductoID+"   "+item.BodegaID;


						try
						{
							if (db.INV_PD_BODEGA_STOCK.Any(pro => pro.ProductoID == item.ProductoID && pro.BodegaID == item.BodegaID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PD_BODEGA_STOCK.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/

		public PagedList<INV_PRECIOS> listInvPrecios(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRECIOS> packages = new PagedList<INV_PRECIOS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRECIOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave saveInvPrecio(PagedList<INV_PRECIOS> precios)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in precios.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;


						try
						{
							if (db.INV_PRECIOS.Any(pro => pro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PRECIOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_PRECIOS_DT> listInvPreciosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRECIOS_DT> packages = new PagedList<INV_PRECIOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRECIOS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave saveInvPrecioDt(PagedList<INV_PRECIOS_DT> precios)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in precios.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;


						try
						{
							if (db.INV_PRECIOS_DT.Any(pro => pro.PrecioID == item.PrecioID && pro.ProductoID == item.ProductoID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();

							}
							else
							{
								db.INV_PRECIOS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_PRODUCTOS_STOCK> listInvProductsStock(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRODUCTOS_STOCK> packages = new PagedList<INV_PRODUCTOS_STOCK>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRODUCTOS_STOCK.Where(e => e.Exportadodate > lastUpdate).ToList<INV_PRODUCTOS_STOCK>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave saveInvProductsStock(PagedList<INV_PRODUCTOS_STOCK> precios)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in precios.Results)
					{
						errorSave.errorMessage=item.ProductoID;


						try
						{
							if (db.INV_PRODUCTOS_STOCK.Any(pro => pro.ProductoID == item.ProductoID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();

							}
							else
							{
								db.INV_PRODUCTOS_STOCK.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_RUBROS> listInvRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_RUBROS> packages = new PagedList<INV_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave saveInvRubros(PagedList<INV_RUBROS> precios)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in precios.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_RUBROS.Any(pro => pro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<ACC_CUENTAS> listAccCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACC_CUENTAS> packages = new PagedList<ACC_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.ACC_CUENTAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
		}

		public ErrorSave saveAccCuentas(PagedList<ACC_CUENTAS> cuentas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cuentas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACC_CUENTAS.Any(pro => pro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACC_CUENTAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<EMP_EMPLEADOS> listEmployess(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_EMPLEADOS> employess = new PagedList<EMP_EMPLEADOS>();
			using (DobraConnection db = new DobraConnection())
			{
				employess.Results = db.EMP_EMPLEADOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				employess.Total = employess.Results.Count;
				employess.Count = employess.Results.Count;
			}
			return employess;
		}

		public ErrorSave saveEmployess(PagedList<EMP_EMPLEADOS> employes)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in employes.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.EMP_EMPLEADOS.Any(emp => emp.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_EMPLEADOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_BANCOS> listBanks(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_BANCOS> bancos = new PagedList<BAN_BANCOS>();
			using (DobraConnection db = new DobraConnection())
			{
				bancos.Results = db.BAN_BANCOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<BAN_BANCOS>();

				bancos.Total = bancos.Results.Count;
				bancos.Count = bancos.Results.Count;
			}
			return bancos;
		}

		public ErrorSave saveBanks(PagedList<BAN_BANCOS> bancos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in bancos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_BANCOS.Any(banco => banco.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_BANCOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<CLI_RUBROS> listCliRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_RUBROS> rubros = new PagedList<CLI_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				rubros.Results = db.CLI_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<CLI_RUBROS>();

				rubros.Total = rubros.Results.Count;
				rubros.Count = rubros.Results.Count;
			}
			return rubros;
		}

		public ErrorSave saveCliRubros(PagedList<CLI_RUBROS> cliRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in cliRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_RUBROS.Any(cliRubro => cliRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_EMPAQUES> listInvPackages(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_EMPAQUES> empques = new PagedList<INV_EMPAQUES>();
			using (DobraConnection db = new DobraConnection())
			{
				empques.Results = db.INV_EMPAQUES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_EMPAQUES>();

				empques.Total = empques.Results.Count;
				empques.Count = empques.Results.Count;
			}
			return empques;

		}



		public ErrorSave saveInvPackages(PagedList<INV_EMPAQUES> empauqes)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empauqes.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_EMPAQUES.Any(empauqe => empauqe.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_EMPAQUES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<SEG_PERFILES> listSegProfiles(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SEG_PERFILES> perfiles = new PagedList<SEG_PERFILES>();
			using (DobraConnection db = new DobraConnection())
			{
				perfiles.Results = db.SEG_PERFILES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				perfiles.Total = perfiles.Results.Count;
				perfiles.Count = perfiles.Results.Count;
			}
			return perfiles;

		}

		public ErrorSave saveSegProfiles(PagedList<SEG_PERFILES> segPerfiles)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in segPerfiles.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.id;

						try
						{
							if (db.SEG_PERFILES.Any(segPerfile => segPerfile.id == item.id))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SEG_PERFILES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<SEG_RECURSOS> listSegRecursos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SEG_RECURSOS> recursos = new PagedList<SEG_RECURSOS>();
			using (DobraConnection db = new DobraConnection())
			{
				recursos.Results = db.SEG_RECURSOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				recursos.Total = recursos.Results.Count;
				recursos.Count = recursos.Results.Count;
			}
			return recursos;

		}

		public ErrorSave saveSegRecursos(PagedList<SEG_RECURSOS> segRecursos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in segRecursos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.SEG_RECURSOS.Any(segRecurso => segRecurso.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SEG_RECURSOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<SEG_USUARIOS> listSegUsuarios(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SEG_USUARIOS> usuarios = new PagedList<SEG_USUARIOS>();
			using (DobraConnection db = new DobraConnection())
			{
				usuarios.Results = db.SEG_USUARIOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				usuarios.Total = usuarios.Results.Count;
				usuarios.Count = usuarios.Results.Count;
			}
			return usuarios;

		}

		public ErrorSave saveSegUsuarios(PagedList<SEG_USUARIOS> segUsuarios)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in segUsuarios.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.SEG_USUARIOS.Any(segUsuario => segUsuario.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SEG_USUARIOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<SIS_DIVISIONES> listSisDivisiones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SIS_DIVISIONES> divisiones = new PagedList<SIS_DIVISIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				divisiones.Results = db.SIS_DIVISIONES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				divisiones.Total = divisiones.Results.Count;
				divisiones.Count = divisiones.Results.Count;
			}
			return divisiones;

		}

		public ErrorSave saveDivisiones(PagedList<SIS_DIVISIONES> sisDivisiones)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in sisDivisiones.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.SIS_DIVISIONES.Any(sisDivision => sisDivision.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SIS_DIVISIONES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<SIS_PARAMETROS> listSisParametros(DateTime lastUpdate, DateTime lastUpdate2)
		{

			PagedList<SIS_PARAMETROS> parametros = new PagedList<SIS_PARAMETROS>();
			using (DobraConnection db = new DobraConnection())
			{
				parametros.Results = db.SIS_PARAMETROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				parametros.Total = parametros.Results.Count;
				parametros.Count = parametros.Results.Count;
			}
			return parametros;

		}

		public ErrorSave saveSisParametros(PagedList<SIS_PARAMETROS> sisDivisiones)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in sisDivisiones.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.SIS_PARAMETROS.Any(sisDivision => sisDivision.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SIS_PARAMETROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<SIS_SUCURSALES> listSisSucursales(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SIS_SUCURSALES> sucursales = new PagedList<SIS_SUCURSALES>();
			using (DobraConnection db = new DobraConnection())
			{
				sucursales.Results = db.SIS_SUCURSALES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				sucursales.Total = sucursales.Results.Count;
				sucursales.Count = sucursales.Results.Count;
			}
			return sucursales;

		}

		public ErrorSave saveSisSucursales(PagedList<SIS_SUCURSALES> sisSucursales)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in sisSucursales.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.SIS_SUCURSALES.Any(sisSucursal => sisSucursal.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SIS_SUCURSALES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<SRI_SECUENCIAL> listSriSecuencial(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SRI_SECUENCIAL> sriSecuencial = new PagedList<SRI_SECUENCIAL>();
			using (DobraConnection db = new DobraConnection())
			{
				sriSecuencial.Results = db.SRI_SECUENCIAL.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				sriSecuencial.Total = sriSecuencial.Results.Count;
				sriSecuencial.Count = sriSecuencial.Results.Count;
			}
			return sriSecuencial;

		}

		public ErrorSave saveSriSecuencial(PagedList<SRI_SECUENCIAL> sriSecuencial)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in sriSecuencial.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.SRI_SECUENCIAL.Any(sri => sri.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SRI_SECUENCIAL.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<SEG_PERFILES_RECURSOS> listSegPerfilesRecursos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SEG_PERFILES_RECURSOS> perfilesRecuros = new PagedList<SEG_PERFILES_RECURSOS>();
			using (DobraConnection db = new DobraConnection())
			{
				perfilesRecuros.Results = db.SEG_PERFILES_RECURSOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				perfilesRecuros.Total = perfilesRecuros.Results.Count;
				perfilesRecuros.Count = perfilesRecuros.Results.Count;
			}
			return perfilesRecuros;

		}

		public ErrorSave saveSegPerfilesRecursos(PagedList<SEG_PERFILES_RECURSOS> perfilesRecuros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in perfilesRecuros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.id;

						try
						{
							if (db.SEG_PERFILES_RECURSOS.Any(perfilRe => perfilRe.id == item.id))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.SEG_PERFILES_RECURSOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<ACC_ASIENTOS> listAccAsientos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACC_ASIENTOS> accCuentas = new PagedList<ACC_ASIENTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				accCuentas.Results = db.ACC_ASIENTOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				accCuentas.Total = accCuentas.Results.Count;
				accCuentas.Count = accCuentas.Results.Count;
			}
			return accCuentas;

		}

		public ErrorSave saveAccAsientos(PagedList<ACC_ASIENTOS> perfilesRecuros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in perfilesRecuros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACC_ASIENTOS.Any(perfilRe => perfilRe.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACC_ASIENTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<ACC_ASIENTOS_DT> listAccAsientosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACC_ASIENTOS_DT> accAsientos = new PagedList<ACC_ASIENTOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				accAsientos.Results = db.ACC_ASIENTOS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || e.ExportadoDate > lastUpdate).ToList();

				accAsientos.Total = accAsientos.Results.Count;
				accAsientos.Count = accAsientos.Results.Count;
			}
			return accAsientos;

		}

		public ErrorSave saveAccAsientosDt(PagedList<ACC_ASIENTOS_DT> accAsientosDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in accAsientosDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACC_ASIENTOS_DT.Any(accAsiento => accAsiento.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACC_ASIENTOS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_INGRESOS> listBanIngresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_INGRESOS> banIngresos = new PagedList<BAN_INGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresos.Results = db.BAN_INGRESOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				banIngresos.Total = banIngresos.Results.Count;
				banIngresos.Count = banIngresos.Results.Count;
			}
			return banIngresos;

		}

		public ErrorSave saveBanIngresos(PagedList<BAN_INGRESOS> banIngresos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in banIngresos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_INGRESOS.Any(banIngreso => banIngreso.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_INGRESOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_INGRESOS_DT> listBanIngresosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_INGRESOS_DT> banIngresosDt = new PagedList<BAN_INGRESOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosDt.Results = db.BAN_INGRESOS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosDt.Total = banIngresosDt.Results.Count;
				banIngresosDt.Count = banIngresosDt.Results.Count;
			}
			return banIngresosDt;

		}

		public ErrorSave saveBanIngresosDt(PagedList<BAN_INGRESOS_DT> banIngresosDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in banIngresosDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_INGRESOS_DT.Any(banIngreso => banIngreso.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_INGRESOS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<CLI_CLIENTES_DEUDAS> listClientesDeduas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_CLIENTES_DEUDAS> clienteDeduaas = new PagedList<CLI_CLIENTES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				clienteDeduaas.Results = db.CLI_CLIENTES_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				clienteDeduaas.Total = clienteDeduaas.Results.Count;
				clienteDeduaas.Count = clienteDeduaas.Results.Count;
			}
			return clienteDeduaas;

		}

		public ErrorSave saveClienteDeudas(PagedList<CLI_CLIENTES_DEUDAS> clienteDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in clienteDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_CLIENTES_DEUDAS.Any(clienteDeuda => clienteDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_CLIENTES_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ModelDobraDatabase.CLI_CREDITOS> listCliCreditos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ModelDobraDatabase.CLI_CREDITOS> cliCreditos = new PagedList<ModelDobraDatabase.CLI_CREDITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditos.Results = db.CLI_CREDITOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				cliCreditos.Total = cliCreditos.Results.Count;
				cliCreditos.Count = cliCreditos.Results.Count;
			}
			return cliCreditos;

		}

		public ErrorSave saveCliCreditos(PagedList<ModelDobraDatabase.CLI_CREDITOS> clienteDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in clienteDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_CREDITOS.Any(clienteDeuda => clienteDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_CREDITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> listCliCreditosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> cliCreditosProductos = new PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditosProductos.Results = db.CLI_CREDITOS_PRODUCTOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliCreditosProductos.Total = cliCreditosProductos.Results.Count;
				cliCreditosProductos.Count = cliCreditosProductos.Results.Count;
			}
			return cliCreditosProductos;

		}

		public ErrorSave saveCliCreditosProductos(PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> clienteCreditoProductos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in clienteCreditoProductos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_CREDITOS_PRODUCTOS.Any(clienteCreditoProducto => clienteCreditoProducto.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_CREDITOS_PRODUCTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_PRODUCTOS_CARDEX> listInvProductosCardex(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRODUCTOS_CARDEX> invProductosCardex = new PagedList<INV_PRODUCTOS_CARDEX>();
			using (DobraConnection db = new DobraConnection())
			{
				invProductosCardex.Results = db.INV_PRODUCTOS_CARDEX.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invProductosCardex.Total = invProductosCardex.Results.Count;
				invProductosCardex.Count = invProductosCardex.Results.Count;
			}
			return invProductosCardex;

		}

		public ErrorSave saveInvProductosCardex(PagedList<INV_PRODUCTOS_CARDEX> productosCardex)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in productosCardex.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_PRODUCTOS_CARDEX.Any(productoCardex => productoCardex.ID==item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PRODUCTOS_CARDEX.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		/*public PagedList<POS_CIERRES_CAJA> listPosCierresCajas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<POS_CIERRES_CAJA> posCierresCaja = new PagedList<POS_CIERRES_CAJA>();
			using (DobraConnection db = new DobraConnection())
			{
				posCierresCaja.Results = db.POS_CIERRES_CAJA.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				posCierresCaja.Total = posCierresCaja.Results.Count;
				posCierresCaja.Count = posCierresCaja.Results.Count;
			}
			return posCierresCaja;

		}*/

	/*	public ErrorSave savePosCierresCajas(PagedList<POS_CIERRES_CAJA> productosCardex)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in productosCardex.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.POS_CIERRES_CAJA.Any(posCierreCaja => posCierreCaja.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.POS_CIERRES_CAJA.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/


		public PagedList<POS_CIERRES> listPosCierres(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<POS_CIERRES> posCierres = new PagedList<POS_CIERRES>();
			using (DobraConnection db = new DobraConnection())
			{
				posCierres.Results = db.POS_CIERRES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				posCierres.Total = posCierres.Results.Count;
				posCierres.Count = posCierres.Results.Count;
			}
			return posCierres;

		}

		public ErrorSave savePosCierres(PagedList<POS_CIERRES> posCierres)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in posCierres.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.POS_CIERRES.Any(posCierre => posCierre.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.POS_CIERRES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<VEN_FACTURAS> listVenFacturas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<VEN_FACTURAS> venFacturas = new PagedList<VEN_FACTURAS>();
			using (DobraConnection db = new DobraConnection())
			{

				venFacturas.Results = db.VEN_FACTURAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				venFacturas.Total = venFacturas.Results.Count;
				venFacturas.Count = venFacturas.Results.Count;
			}
			return venFacturas;

		}

		public ErrorSave saveVenFacturas(PagedList<VEN_FACTURAS> venFacturas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{

				//venFacturas.Results.ForEach(n => db.VEN_FACTURAS.Add(n));
				try
				{
					foreach (var item in venFacturas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;


						
						try
						{


							var clientes   =db.CLI_CLIENTES.Where(cliente => cliente.Cédula.Trim() == item.Ruc.Trim()  ||
							cliente.Ruc.Trim() == item.Ruc.Trim()
							);

							if (db.VEN_FACTURAS.Any(venFactura => venFactura.ID == item.ID))
							{


								if (clientes.Count() > 0)
								{
									item.ClienteID = clientes.First<CLI_CLIENTES>().ID;
								}

								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								//db.SaveChanges();
							}
							else
							{

								if (clientes.Count() > 0)
								{
									item.ClienteID = clientes.First<CLI_CLIENTES>().ID;
								}

								db.VEN_FACTURAS.Add(item);
								

							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
					db.SaveChanges();
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<VEN_FACTURAS_DT> listVenFacturasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<VEN_FACTURAS_DT> venFacturas = new PagedList<VEN_FACTURAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				venFacturas.Results = db.VEN_FACTURAS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				venFacturas.Total = venFacturas.Results.Count;
				venFacturas.Count = venFacturas.Results.Count;
			}
			return venFacturas;

		}

		public ErrorSave saveVenFacturasDt(PagedList<VEN_FACTURAS_DT> venFacturas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in venFacturas.Results)
					{
						errorSave.errorMessage = item.ID;


						try
						{
							if (db.VEN_FACTURAS_DT.Any(venFactura => venFactura.ID.Trim() == item.ID.Trim()))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
							}
							else
							{
								db.VEN_FACTURAS_DT.Add(item);
								
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
					db.SaveChanges();

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_INGRESOS_DEUDAS> listBanIngresoDeuda(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_INGRESOS_DEUDAS> banIngresosDeudas = new PagedList<BAN_INGRESOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosDeudas.Results = db.BAN_INGRESOS_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosDeudas.Total = banIngresosDeudas.Results.Count;
				banIngresosDeudas.Count = banIngresosDeudas.Results.Count;
			}
			return banIngresosDeudas;

		}

		public ErrorSave saveBanIngresoDeuda(PagedList<BAN_INGRESOS_DEUDAS> banIngresosDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banIngresosDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_INGRESOS_DEUDAS.Any(banIngresoDeuda => banIngresoDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_INGRESOS_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_BANCOS_CARDEX> listBanBancosCardex(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_BANCOS_CARDEX> banBancoCardex = new PagedList<BAN_BANCOS_CARDEX>();
			using (DobraConnection db = new DobraConnection())
			{
				banBancoCardex.Results = db.BAN_BANCOS_CARDEX.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banBancoCardex.Total = banBancoCardex.Results.Count;
				banBancoCardex.Count = banBancoCardex.Results.Count;
			}
			return banBancoCardex;

		}

		public ErrorSave saveBanBancosCardex(PagedList<BAN_BANCOS_CARDEX> banBancoCardex)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banBancoCardex.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_BANCOS_CARDEX.Any(banbankCardex => banbankCardex.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_BANCOS_CARDEX.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_DEPOSITOS> listBanDepositos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_DEPOSITOS> banDepositos = new PagedList<BAN_DEPOSITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDepositos.Results = db.BAN_DEPOSITOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDepositos.Total = banDepositos.Results.Count;
				banDepositos.Count = banDepositos.Results.Count;
			}
			return banDepositos;

		}

		public ErrorSave saveBanDepositos(PagedList<BAN_DEPOSITOS> banDepositos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banDepositos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_DEPOSITOS.Any(banDeposito => banDeposito.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_DEPOSITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_DEPOSITOS_DT> listBanDepositosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_DEPOSITOS_DT> banDepositosDt = new PagedList<BAN_DEPOSITOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banDepositosDt.Results = db.BAN_DEPOSITOS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDepositosDt.Total = banDepositosDt.Results.Count;
				banDepositosDt.Count = banDepositosDt.Results.Count;
			}
			return banDepositosDt;

		}

		public ErrorSave saveBanDepositosDt(PagedList<BAN_DEPOSITOS_DT> banDepositosDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banDepositosDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_DEPOSITOS_DT.Any(banDepositoDt => banDepositoDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_DEPOSITOS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		/*public PagedList<BAN_DEPOSITOS_PAPELETAS> listBanDepositoPapeletas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_DEPOSITOS_PAPELETAS> banDepositosDt = new PagedList<BAN_DEPOSITOS_PAPELETAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDepositosDt.Results = db.BAN_DEPOSITOS_PAPELETAS.Where(e => e.Fecha >= lastUpdate).ToList();
				//banDepositosDt.Results = db.BAN_DEPOSITOS_PAPELETAS.Where(e => e.CreadoDate >= lastUpdate).ToList();
				banDepositosDt.Total = banDepositosDt.Results.Count;
				banDepositosDt.Count = banDepositosDt.Results.Count;
			}
			return banDepositosDt;

		}*/

		/*public ErrorSave saveBanDepositoPapeletas(PagedList<BAN_DEPOSITOS_PAPELETAS> banDepositosPapeletas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banDepositosPapeletas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_DEPOSITOS_PAPELETAS.Any(banDepositoPapelete => banDepositoPapelete.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_DEPOSITOS_PAPELETAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/

		public PagedList<COM_FACTURAS> listComFacturas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<COM_FACTURAS> comFacturas = new PagedList<COM_FACTURAS>();
			using (DobraConnection db = new DobraConnection())
			{
				comFacturas.Results = db.COM_FACTURAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				comFacturas.Total = comFacturas.Results.Count;
				comFacturas.Count = comFacturas.Results.Count;
			}
			return comFacturas;

		}

		public ErrorSave saveComFacturas(PagedList<COM_FACTURAS> comFacturas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in comFacturas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.COM_FACTURAS.Any(comFactura => comFactura.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.COM_FACTURAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<COM_FACTURAS_DT> listComFacturasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<COM_FACTURAS_DT> comFacturasDt = new PagedList<COM_FACTURAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				comFacturasDt.Results = db.COM_FACTURAS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				comFacturasDt.Total = comFacturasDt.Results.Count;
				comFacturasDt.Count = comFacturasDt.Results.Count;
			}
			return comFacturasDt;

		}

		public ErrorSave saveComFacturasDt(PagedList<COM_FACTURAS_DT> comFacturasDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in comFacturasDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.COM_FACTURAS_DT.Any(comFacturaDt => comFacturaDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.COM_FACTURAS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<COM_FACTURAS_PAGOS> listComFacturasPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<COM_FACTURAS_PAGOS> comFacturasPagos = new PagedList<COM_FACTURAS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{
				comFacturasPagos.Results = db.COM_FACTURAS_PAGOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				comFacturasPagos.Total = comFacturasPagos.Results.Count;
				comFacturasPagos.Count = comFacturasPagos.Results.Count;
			}
			return comFacturasPagos;

		}

		public ErrorSave saveComFacturasPagos(PagedList<COM_FACTURAS_PAGOS> comFacturasPagos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in comFacturasPagos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.COM_FACTURAS_PAGOS.Any(comFacturaPago => comFacturaPago.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.COM_FACTURAS_PAGOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_RETENCIONES> listAcrRetenciones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_RETENCIONES> acrRetenciones = new PagedList<ACR_RETENCIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRetenciones.Results = db.ACR_RETENCIONES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRetenciones.Total = acrRetenciones.Results.Count;
				acrRetenciones.Count = acrRetenciones.Results.Count;
			}
			return acrRetenciones;

		}

		public ErrorSave saveAcrRetenciones(PagedList<ACR_RETENCIONES> acrRetenciones)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrRetenciones.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_RETENCIONES.Any(acrRetencion => acrRetencion.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_RETENCIONES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}





		public PagedList<ACR_RETENCIONES_DT> listAcrRetencionesDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_RETENCIONES_DT> acrRetencionesDt = new PagedList<ACR_RETENCIONES_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRetencionesDt.Results = db.ACR_RETENCIONES_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRetencionesDt.Total = acrRetencionesDt.Results.Count;
				acrRetencionesDt.Count = acrRetencionesDt.Results.Count;
			}
			return acrRetencionesDt;

		}

		public ErrorSave saveAcrRetencionesDt(PagedList<ACR_RETENCIONES_DT> acrRetencionesDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrRetencionesDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_RETENCIONES_DT.Any(acrRetencionDt => acrRetencionDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_RETENCIONES_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<ACR_RETENCIONES_DEUDAS> listAcrRetencionesDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_RETENCIONES_DEUDAS> acrRetencionesDeudas = new PagedList<ACR_RETENCIONES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRetencionesDeudas.Results = db.ACR_RETENCIONES_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRetencionesDeudas.Total = acrRetencionesDeudas.Results.Count;
				acrRetencionesDeudas.Count = acrRetencionesDeudas.Results.Count;
			}
			return acrRetencionesDeudas;

		}


		public ErrorSave saveAcrRetencionesDeudas(PagedList<ACR_RETENCIONES_DEUDAS> acrRetencionesDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrRetencionesDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_RETENCIONES_DEUDAS.Any(acrRetencionDeuda => acrRetencionDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_RETENCIONES_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_ACREEDORES_DEUDAS> listAcrAcreedoresDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_ACREEDORES_DEUDAS> acrAcreedoresDeudas = new PagedList<ACR_ACREEDORES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrAcreedoresDeudas.Results = db.ACR_ACREEDORES_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrAcreedoresDeudas.Total = acrAcreedoresDeudas.Results.Count;
				acrAcreedoresDeudas.Count = acrAcreedoresDeudas.Results.Count;
			}
			return acrAcreedoresDeudas;

		}

		public ErrorSave saveAcrAcreedoresDeudas(PagedList<ACR_ACREEDORES_DEUDAS> acrAcreedoresDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrAcreedoresDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_ACREEDORES_DEUDAS.Any(acrAcreedorDeuda => acrAcreedorDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_ACREEDORES_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<PRV_FACTURAS> listPvrFacturas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<PRV_FACTURAS> pvrFacturas = new PagedList<PRV_FACTURAS>();
			using (DobraConnection db = new DobraConnection())
			{
				pvrFacturas.Results = db.PRV_FACTURAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				pvrFacturas.Total = pvrFacturas.Results.Count;
				pvrFacturas.Count = pvrFacturas.Results.Count;
			}
			return pvrFacturas;

		}

		public ErrorSave savePvrFacturas(PagedList<PRV_FACTURAS> pvrFacturas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in pvrFacturas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.PRV_FACTURAS.Any(prvFactura => prvFactura.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.PRV_FACTURAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<PRV_FACTURAS_DT> listPvrFacturasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<PRV_FACTURAS_DT> pvrFacturasDt = new PagedList<PRV_FACTURAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				pvrFacturasDt.Results = db.PRV_FACTURAS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				pvrFacturasDt.Total = pvrFacturasDt.Results.Count;
				pvrFacturasDt.Count = pvrFacturasDt.Results.Count;
			}
			return pvrFacturasDt;

		}

		public ErrorSave savePvrFacturasDt(PagedList<PRV_FACTURAS_DT> pvrFacturasDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in pvrFacturasDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.PRV_FACTURAS_DT.Any(prvFacturaDt => prvFacturaDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.PRV_FACTURAS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<PRV_FACTURASCTA_DT> listPvrFacturasCtaDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<PRV_FACTURASCTA_DT> pvrFacturasCtaDt = new PagedList<PRV_FACTURASCTA_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				pvrFacturasCtaDt.Results = db.PRV_FACTURASCTA_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				pvrFacturasCtaDt.Total = pvrFacturasCtaDt.Results.Count;
				pvrFacturasCtaDt.Count = pvrFacturasCtaDt.Results.Count;
			}
			return pvrFacturasCtaDt;

		}

		public ErrorSave savePvrFacturasCtaDt(PagedList<PRV_FACTURASCTA_DT> pvrFacturasCtaDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in pvrFacturasCtaDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.PRV_FACTURASCTA_DT.Any(prvFacturaCtaDt => prvFacturaCtaDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.PRV_FACTURASCTA_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<EMP_ROLES> listEmpRoles(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_ROLES> empRoles = new PagedList<EMP_ROLES>();
			using (DobraConnection db = new DobraConnection())
			{
				empRoles.Results = db.EMP_ROLES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empRoles.Total = empRoles.Results.Count;
				empRoles.Count = empRoles.Results.Count;
			}
			return empRoles;
		}

		public ErrorSave saveEmpRoles(PagedList<EMP_ROLES> empRoles)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empRoles.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.EMP_ROLES.Any(empRol => empRol.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_ROLES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<EMP_ROLES_EMPLEADOS> listEmpRolesEmpleados(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_ROLES_EMPLEADOS> empRolesEmpleados = new PagedList<EMP_ROLES_EMPLEADOS>();
			using (DobraConnection db = new DobraConnection())
			{
				empRolesEmpleados.Results = db.EMP_ROLES_EMPLEADOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empRolesEmpleados.Total = empRolesEmpleados.Results.Count;
				empRolesEmpleados.Count = empRolesEmpleados.Results.Count;
			}
			return empRolesEmpleados;

		}

		public ErrorSave saveEmpRolesEmpleados(PagedList<EMP_ROLES_EMPLEADOS> empRolesEmpleados)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empRolesEmpleados.Results)
					{
						errorSave.errorMessage=item.RolID;

						try
						{
							if (db.EMP_ROLES_EMPLEADOS.Any(empRolEmpleado => empRolEmpleado.RolID == item.RolID && empRolEmpleado.EmpleadoID == item.EmpleadoID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_ROLES_EMPLEADOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<EMP_ROLES_RUBROS> listEmpRolesRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_ROLES_RUBROS> empRolesRubros = new PagedList<EMP_ROLES_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				empRolesRubros.Results = db.EMP_ROLES_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empRolesRubros.Total = empRolesRubros.Results.Count;
				empRolesRubros.Count = empRolesRubros.Results.Count;
			}
			return empRolesRubros;

		}

		public ErrorSave saveEmpRolesRubros(PagedList<EMP_ROLES_RUBROS> empRolesRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empRolesRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.EMP_ROLES_RUBROS.Any(empRolRubro => empRolRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_ROLES_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<EMP_EMPLEADOS_DEUDAS> listEmpEmpleadosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_EMPLEADOS_DEUDAS> empEmpleadosDeudas = new PagedList<EMP_EMPLEADOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				empEmpleadosDeudas.Results = db.EMP_EMPLEADOS_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empEmpleadosDeudas.Total = empEmpleadosDeudas.Results.Count;
				empEmpleadosDeudas.Count = empEmpleadosDeudas.Results.Count;
			}
			return empEmpleadosDeudas;

		}

		public ErrorSave saveEmpEmpleadosDeudas(PagedList<EMP_EMPLEADOS_DEUDAS> empEmpleadosDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empEmpleadosDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.EMP_EMPLEADOS_DEUDAS.Any(empEmpleadoDeuda => empEmpleadoDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_EMPLEADOS_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<EMP_EMPLEADOS_HORAS> listEmpEmpleadosHoras(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_EMPLEADOS_HORAS> empEmpleadosHoras = new PagedList<EMP_EMPLEADOS_HORAS>();
			using (DobraConnection db = new DobraConnection())
			{
				empEmpleadosHoras.Results = db.EMP_EMPLEADOS_HORAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empEmpleadosHoras.Total = empEmpleadosHoras.Results.Count;
				empEmpleadosHoras.Count = empEmpleadosHoras.Results.Count;
			}
			return empEmpleadosHoras;

		}

		public ErrorSave saveEmpEmpleadosHoras(PagedList<EMP_EMPLEADOS_HORAS> empEmpleadosHoras)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empEmpleadosHoras.Results)
					{
						errorSave.errorMessage=item.EmpleadoID;

						try
						{
							if (db.EMP_EMPLEADOS_HORAS.Any(empEmpleadoHora => empEmpleadoHora.Año == item.Año
						&& empEmpleadoHora.Mes == item.Mes && empEmpleadoHora.EmpleadoID == item.EmpleadoID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_EMPLEADOS_HORAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<EMP_DEBITOS> listEmpDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_DEBITOS> empDebitos = new PagedList<EMP_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				empDebitos.Results = db.EMP_DEBITOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empDebitos.Total = empDebitos.Results.Count;
				empDebitos.Count = empDebitos.Results.Count;
			}
			return empDebitos;

		}

		public ErrorSave saveEmpDebitos(PagedList<EMP_DEBITOS> empDebitos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empDebitos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.EMP_DEBITOS.Any(empDebito => empDebito.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_DEBITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}




		public PagedList<EMP_DEBITOS_RUBROS> listEmpDebitosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<EMP_DEBITOS_RUBROS> empDebitosRubros = new PagedList<EMP_DEBITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				empDebitosRubros.Results = db.EMP_DEBITOS_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empDebitosRubros.Total = empDebitosRubros.Results.Count;
				empDebitosRubros.Count = empDebitosRubros.Results.Count;
			}
			return empDebitosRubros;

		}

		public ErrorSave saveEmpDebitosRubros(PagedList<EMP_DEBITOS_RUBROS> empDebitosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empDebitosRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.EMP_DEBITOS_RUBROS.Any(empDebitoRubro => empDebitoRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.EMP_DEBITOS_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<CLI_GRUPOS> listCliGrupos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_GRUPOS> cliGrupos = new PagedList<CLI_GRUPOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliGrupos.Results = db.CLI_GRUPOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliGrupos.Total = cliGrupos.Results.Count;
				cliGrupos.Count = cliGrupos.Results.Count;
			}
			return cliGrupos;

		}

		public ErrorSave saveCliGrupos(PagedList<CLI_GRUPOS> cliGrupos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cliGrupos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_GRUPOS.Any(cliGrupo => cliGrupo.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_GRUPOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_BODEGAS> listInvBodegas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_BODEGAS> invBodegas = new PagedList<INV_BODEGAS>();
			using (DobraConnection db = new DobraConnection())
			{
				invBodegas.Results = db.INV_BODEGAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invBodegas.Total = invBodegas.Results.Count;
				invBodegas.Count = invBodegas.Results.Count;
			}
			return invBodegas;

		}

		public ErrorSave saveInvBodegas(PagedList<INV_BODEGAS> invBodegas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invBodegas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_BODEGAS.Any(invBodega => invBodega.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_BODEGAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		/*public PagedList<INV_PRODUCTOS_EXHIBICION> listInvProductosExhibicion(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRODUCTOS_EXHIBICION> inProductosExhibicion = new PagedList<INV_PRODUCTOS_EXHIBICION>();
			using (DobraConnection db = new DobraConnection())
			{
				inProductosExhibicion.Results = db.INV_PRODUCTOS_EXHIBICION.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				inProductosExhibicion.Total = inProductosExhibicion.Results.Count;
				inProductosExhibicion.Count = inProductosExhibicion.Results.Count;
			}
			return inProductosExhibicion;

		}*/

		/*public ErrorSave saveInvProductosExhibicion(PagedList<INV_PRODUCTOS_EXHIBICION> inProductosExhibicion)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in inProductosExhibicion.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_PRODUCTOS_EXHIBICION.Any(empDebitoRubro => empDebitoRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PRODUCTOS_EXHIBICION.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/

		public PagedList<ACR_CREDITOS> listAcrCreditos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_CREDITOS> empDebitosRubros = new PagedList<ACR_CREDITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				empDebitosRubros.Results = db.ACR_CREDITOS.Where(e => e.Fecha >= lastUpdate).ToList();

				empDebitosRubros.Total = empDebitosRubros.Results.Count;
				empDebitosRubros.Count = empDebitosRubros.Results.Count;
			}
			return empDebitosRubros;

		}

		public ErrorSave saveAcrCreditos(PagedList<ACR_CREDITOS> empDebitosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in empDebitosRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_CREDITOS.Any(empDebitoRubro => empDebitoRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_CREDITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<ACR_CREDITOS_DEUDAS> listAcrCreditosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_CREDITOS_DEUDAS> acrCreditosDeudas = new PagedList<ACR_CREDITOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrCreditosDeudas.Results = db.ACR_CREDITOS_DEUDAS.Where(e => e.CreadoDate >= lastUpdate).ToList();

				acrCreditosDeudas.Total = acrCreditosDeudas.Results.Count;
				acrCreditosDeudas.Count = acrCreditosDeudas.Results.Count;
			}
			return acrCreditosDeudas;

		}

		public ErrorSave saveAcrCreditosDeudas(PagedList<ACR_CREDITOS_DEUDAS> acrCreditosDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrCreditosDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_CREDITOS_DEUDAS.Any(acrCreditosDeuda => acrCreditosDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_CREDITOS_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_CREDITOS_RUBROS> listAcrCreditosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_CREDITOS_RUBROS> acrCreditosRubros = new PagedList<ACR_CREDITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrCreditosRubros.Results = db.ACR_CREDITOS_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrCreditosRubros.Total = acrCreditosRubros.Results.Count;
				acrCreditosRubros.Count = acrCreditosRubros.Results.Count;
			}
			return acrCreditosRubros;

		}

		public ErrorSave saveAcrCreditosRubros(PagedList<ACR_CREDITOS_RUBROS> acrCreditosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrCreditosRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_CREDITOS_RUBROS.Any(acrCreditoRubro => acrCreditoRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_CREDITOS_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_DEBITOS> listAcrDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_DEBITOS> acrDebitos = new PagedList<ACR_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitos.Results = db.ACR_DEBITOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitos.Total = acrDebitos.Results.Count;
				acrDebitos.Count = acrDebitos.Results.Count;
			}
			return acrDebitos;

		}

		public ErrorSave saveAcrDebitos(PagedList<ACR_DEBITOS> acrDebitos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrDebitos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_DEBITOS.Any(acrDebito => acrDebito.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_DEBITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_DEBITOS_DEUDAS> listAcrDebitosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_DEBITOS_DEUDAS> acrDebitosDeudas = new PagedList<ACR_DEBITOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitosDeudas.Results = db.ACR_DEBITOS_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitosDeudas.Total = acrDebitosDeudas.Results.Count;
				acrDebitosDeudas.Count = acrDebitosDeudas.Results.Count;
			}
			return acrDebitosDeudas;

		}

		public ErrorSave saveAcrDebitosDeudas(PagedList<ACR_DEBITOS_DEUDAS> acrDebitosDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrDebitosDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_DEBITOS_DEUDAS.Any(acrDebitosDeuda => acrDebitosDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_DEBITOS_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_DEBITOS_RUBROS> listAcrDebitoRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_DEBITOS_RUBROS> acrDebitosRubros = new PagedList<ACR_DEBITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitosRubros.Results = db.ACR_DEBITOS_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitosRubros.Total = acrDebitosRubros.Results.Count;
				acrDebitosRubros.Count = acrDebitosRubros.Results.Count;
			}
			return acrDebitosRubros;

		}

		public ErrorSave saveAcrDebitoRubros(PagedList<ACR_DEBITOS_RUBROS> acrDebitosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrDebitosRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_DEBITOS_RUBROS.Any(acrDebitosRubro => acrDebitosRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_DEBITOS_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ACR_DEBITOS_PRODUCTOS> listAcrDebitosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_DEBITOS_PRODUCTOS> acrDebitosProductos = new PagedList<ACR_DEBITOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitosProductos.Results = db.ACR_DEBITOS_PRODUCTOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitosProductos.Total = acrDebitosProductos.Results.Count;
				acrDebitosProductos.Count = acrDebitosProductos.Results.Count;
			}
			return acrDebitosProductos;

		}

		public ErrorSave saveAcrDebitosProductos(PagedList<ACR_DEBITOS_PRODUCTOS> acrDebitosProductos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrDebitosProductos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_DEBITOS_PRODUCTOS.Any(acrDebitosProducto => acrDebitosProducto.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_DEBITOS_PRODUCTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_RECIBOS> listAcrRecibos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_RECIBOS> acrRecibos = new PagedList<ACR_RECIBOS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRecibos.Results = db.ACR_RECIBOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRecibos.Total = acrRecibos.Results.Count;
				acrRecibos.Count = acrRecibos.Results.Count;
			}
			return acrRecibos;

		}

		public ErrorSave saveAcrRecibos(PagedList<ACR_RECIBOS> acrRecibos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrRecibos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_RECIBOS.Any(acrRecibo => acrRecibo.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_RECIBOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ACR_RECIBOS_DT> listAcrRecibosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_RECIBOS_DT> acrRecibosDt = new PagedList<ACR_RECIBOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRecibosDt.Results = db.ACR_RECIBOS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRecibosDt.Total = acrRecibosDt.Results.Count;
				acrRecibosDt.Count = acrRecibosDt.Results.Count;
			}
			return acrRecibosDt;

		}


		public ErrorSave saveAcrRecibosDt(PagedList<ACR_RECIBOS_DT> acrRecibosDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrRecibosDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_RECIBOS_DT.Any(acrReciboDt => acrReciboDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_RECIBOS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<ACR_RECIBOS_DEUDAS> listAcrReciboDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_RECIBOS_DEUDAS> acrRecibosDeudas = new PagedList<ACR_RECIBOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRecibosDeudas.Results = db.ACR_RECIBOS_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRecibosDeudas.Total = acrRecibosDeudas.Results.Count;
				acrRecibosDeudas.Count = acrRecibosDeudas.Results.Count;
			}
			return acrRecibosDeudas;

		}

		public ErrorSave saveAcrReciboDeudas(PagedList<ACR_RECIBOS_DEUDAS> acrRecibosDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrRecibosDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_RECIBOS_DEUDAS.Any(acrReciboDeuda => acrReciboDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_RECIBOS_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_DEBITOS> listBanDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_DEBITOS> banDebitos = new PagedList<BAN_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDebitos.Results = db.BAN_DEBITOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDebitos.Total = banDebitos.Results.Count;
				banDebitos.Count = banDebitos.Results.Count;
			}
			return banDebitos;

		}

		public ErrorSave saveBanDebitos(PagedList<BAN_DEBITOS> banDebitos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banDebitos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_DEBITOS.Any(banDebito => banDebito.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_DEBITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_DEBITOS_CUENTAS> listBanDebitosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_DEBITOS_CUENTAS> banDebitosCuentas = new PagedList<BAN_DEBITOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDebitosCuentas.Results = db.BAN_DEBITOS_CUENTAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDebitosCuentas.Total = banDebitosCuentas.Results.Count;
				banDebitosCuentas.Count = banDebitosCuentas.Results.Count;
			}
			return banDebitosCuentas;

		}

		public ErrorSave saveBanDebitosCuentas(PagedList<BAN_DEBITOS_CUENTAS> banDebitosCuentas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banDebitosCuentas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_DEBITOS_CUENTAS.Any(banDebitoceunta => banDebitoceunta.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_DEBITOS_CUENTAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_EGRESOS> listbanEgresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_EGRESOS> banEgresos = new PagedList<BAN_EGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresos.Results = db.BAN_EGRESOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresos.Total = banEgresos.Results.Count;
				banEgresos.Count = banEgresos.Results.Count;
			}
			return banEgresos;

		}

		public ErrorSave savebanEgresos(PagedList<BAN_EGRESOS> banEgresos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banEgresos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_EGRESOS.Any(banEgreso => banEgreso.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_EGRESOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_EGRESOS_ANEXOS> listbanEgresosAnexos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_EGRESOS_ANEXOS> banEgresosAnexos = new PagedList<BAN_EGRESOS_ANEXOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosAnexos.Results = db.BAN_EGRESOS_ANEXOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosAnexos.Total = banEgresosAnexos.Results.Count;
				banEgresosAnexos.Count = banEgresosAnexos.Results.Count;
			}
			return banEgresosAnexos;

		}

		public ErrorSave savebanEgresosAnexos(PagedList<BAN_EGRESOS_ANEXOS> banEgresosAnexos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banEgresosAnexos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_EGRESOS_ANEXOS.Any(banEgresoAnexo => banEgresoAnexo.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_EGRESOS_ANEXOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_EGRESOS_ANTICIPOS> listbanEgresosAnticipos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_EGRESOS_ANTICIPOS> banEgresosAnticipos = new PagedList<BAN_EGRESOS_ANTICIPOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosAnticipos.Results = db.BAN_EGRESOS_ANTICIPOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosAnticipos.Total = banEgresosAnticipos.Results.Count;
				banEgresosAnticipos.Count = banEgresosAnticipos.Results.Count;
			}
			return banEgresosAnticipos;

		}

		public ErrorSave savebanEgresosAnticipos(PagedList<BAN_EGRESOS_ANTICIPOS> banEgresosAnticipos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banEgresosAnticipos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_EGRESOS_ANTICIPOS.Any(banEgresoAnexoAnticipo => banEgresoAnexoAnticipo.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_EGRESOS_ANTICIPOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<BAN_EGRESOS_CUENTAS> listbanEgresosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_EGRESOS_CUENTAS> banEgresosCuentas = new PagedList<BAN_EGRESOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosCuentas.Results = db.BAN_EGRESOS_CUENTAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosCuentas.Total = banEgresosCuentas.Results.Count;
				banEgresosCuentas.Count = banEgresosCuentas.Results.Count;
			}
			return banEgresosCuentas;

		}

		public ErrorSave savebanEgresosCuentas(PagedList<BAN_EGRESOS_CUENTAS> banEgresosCuentas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in banEgresosCuentas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_EGRESOS_CUENTAS.Any(banEgresoCuenta => banEgresoCuenta.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_EGRESOS_CUENTAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_EGRESOS_DEUDAS> listbanEgresosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_EGRESOS_DEUDAS> banEgresosDeudas = new PagedList<BAN_EGRESOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosDeudas.Results = db.BAN_EGRESOS_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosDeudas.Total = banEgresosDeudas.Results.Count;
				banEgresosDeudas.Count = banEgresosDeudas.Results.Count;
			}
			return banEgresosDeudas;

		}

		public ErrorSave savebanEgresosDeudas(PagedList<BAN_EGRESOS_DEUDAS> banEgresosDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banEgresosDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_EGRESOS_DEUDAS.Any(banEgresoDeuda => banEgresoDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_EGRESOS_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}




		public PagedList<BAN_EGRESOS_DT> listbanEgresosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_EGRESOS_DT> banEgresosDt = new PagedList<BAN_EGRESOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosDt.Results = db.BAN_EGRESOS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosDt.Total = banEgresosDt.Results.Count;
				banEgresosDt.Count = banEgresosDt.Results.Count;
			}
			return banEgresosDt;

		}

		public ErrorSave savebanEgresosDt(PagedList<BAN_EGRESOS_DT> banEgresosDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banEgresosDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_EGRESOS_DT.Any(banEgresoDeuda => banEgresoDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_EGRESOS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_EGRESOS_PAGOS> listbanEgresosPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_EGRESOS_PAGOS> banEgresosPagos = new PagedList<BAN_EGRESOS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosPagos.Results = db.BAN_EGRESOS_PAGOS.Where(e => e.ExportadoDate > lastUpdate).ToList();

				banEgresosPagos.Total = banEgresosPagos.Results.Count;
				banEgresosPagos.Count = banEgresosPagos.Results.Count;
			}
			return banEgresosPagos;

		}

		public ErrorSave savebanEgresosPagos(PagedList<BAN_EGRESOS_PAGOS> banEgresosPagos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banEgresosPagos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_EGRESOS_PAGOS.Any(banEgresoDeuda => banEgresoDeuda.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_EGRESOS_PAGOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<BAN_INGRESOS_CUENTAS> listbanIngresosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_INGRESOS_CUENTAS> banIngresosCuentas = new PagedList<BAN_INGRESOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosCuentas.Results = db.BAN_INGRESOS_CUENTAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosCuentas.Total = banIngresosCuentas.Results.Count;
				banIngresosCuentas.Count = banIngresosCuentas.Results.Count;
			}
			return banIngresosCuentas;
		}

		public ErrorSave savebanIngresosCuentas(PagedList<BAN_INGRESOS_CUENTAS> banIngresosCuentas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banIngresosCuentas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_INGRESOS_CUENTAS.Any(banIngresoCuenta => banIngresoCuenta.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_INGRESOS_CUENTAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		/*public PagedList<BAN_INGRESOS_TARJETAS> listbanIngresosTarjetas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_INGRESOS_TARJETAS> banIngresosTarjetas = new PagedList<BAN_INGRESOS_TARJETAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosTarjetas.Results = db.BAN_INGRESOS_TARJETAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosTarjetas.Total = banIngresosTarjetas.Results.Count;
				banIngresosTarjetas.Count = banIngresosTarjetas.Results.Count;
			}
			return banIngresosTarjetas;

		}*/

		/*public ErrorSave savebanIngresosTarjetas(PagedList<BAN_INGRESOS_TARJETAS> banIngresosTarjetas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banIngresosTarjetas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_INGRESOS_TARJETAS.Any(banIngresoTarjeta => banIngresoTarjeta.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_INGRESOS_TARJETAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/


		public PagedList<BAN_PAPELETAS> listbanPapeletas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_PAPELETAS> banPapeletas = new PagedList<BAN_PAPELETAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banPapeletas.Results = db.BAN_PAPELETAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banPapeletas.Total = banPapeletas.Results.Count;
				banPapeletas.Count = banPapeletas.Results.Count;
			}
			return banPapeletas;

		}

		public ErrorSave savebanPapeletas(PagedList<BAN_PAPELETAS> banPapeletas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banPapeletas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_PAPELETAS.Any(banPapeleta => banPapeleta.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_PAPELETAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<BAN_TRANSFERENCIAS> listbanTransferencias(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_TRANSFERENCIAS> banTransferencias = new PagedList<BAN_TRANSFERENCIAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banTransferencias.Results = db.BAN_TRANSFERENCIAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banTransferencias.Total = banTransferencias.Results.Count;
				banTransferencias.Count = banTransferencias.Results.Count;
			}
			return banTransferencias;

		}

		public ErrorSave savebanTransferencias(PagedList<BAN_TRANSFERENCIAS> banTransferencias)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banTransferencias.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_TRANSFERENCIAS.Any(banTransferencia => banTransferencia.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_TRANSFERENCIAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_TRANSFERENCIAS_DT> listbanTransferenciasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_TRANSFERENCIAS_DT> banTransferenciasDt = new PagedList<BAN_TRANSFERENCIAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banTransferenciasDt.Results = db.BAN_TRANSFERENCIAS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banTransferenciasDt.Total = banTransferenciasDt.Results.Count;
				banTransferenciasDt.Count = banTransferenciasDt.Results.Count;
			}
			return banTransferenciasDt;

		}

		public ErrorSave savebanTransferenciasDt(PagedList<BAN_TRANSFERENCIAS_DT> banTransferenciasDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banTransferenciasDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_TRANSFERENCIAS_DT.Any(banTransferenciaDt => banTransferenciaDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_TRANSFERENCIAS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<VEN_FACTURAS_PAGOS> listvenFacturasPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<VEN_FACTURAS_PAGOS> venFacturasPagos = new PagedList<VEN_FACTURAS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{
				venFacturasPagos.Results = db.VEN_FACTURAS_PAGOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				venFacturasPagos.Total = venFacturasPagos.Results.Count;
				venFacturasPagos.Count = venFacturasPagos.Results.Count;
			}
			return venFacturasPagos;

		}

		public ErrorSave savevenFacturasPagos(PagedList<VEN_FACTURAS_PAGOS> venFacturasPagos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in venFacturasPagos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.id;


					
						try
						{
							if (db.VEN_FACTURAS_PAGOS.Any(venFacturasPago => venFacturasPago.id == item.id))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.VEN_FACTURAS_PAGOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_EGRESOS> listinvEgresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_EGRESOS> invEgresos = new PagedList<INV_EGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invEgresos.Results = db.INV_EGRESOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invEgresos.Total = invEgresos.Results.Count;
				invEgresos.Count = invEgresos.Results.Count;
			}
			return invEgresos;

		}

		public ErrorSave saveinvEgresos(PagedList<INV_EGRESOS> invEgresos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invEgresos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_EGRESOS.Any(invEgreso => invEgreso.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_EGRESOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_EGRESOS_RUBROS> listinvEgresosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_EGRESOS_RUBROS> invEgresosRubros = new PagedList<INV_EGRESOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				invEgresosRubros.Results = db.INV_EGRESOS_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invEgresosRubros.Total = invEgresosRubros.Results.Count;
				invEgresosRubros.Count = invEgresosRubros.Results.Count;
			}
			return invEgresosRubros;

		}

		public ErrorSave saveinvEgresosRubros(PagedList<INV_EGRESOS_RUBROS> invEgresosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invEgresosRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_EGRESOS_RUBROS.Any(invEgresosRubro => invEgresosRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_EGRESOS_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_EGRESOS_PRODUCTOS> listinvEgresosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_EGRESOS_PRODUCTOS> invEgresosProductos = new PagedList<INV_EGRESOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invEgresosProductos.Results = db.INV_EGRESOS_PRODUCTOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invEgresosProductos.Total = invEgresosProductos.Results.Count;
				invEgresosProductos.Count = invEgresosProductos.Results.Count;
			}
			return invEgresosProductos;

		}

		public ErrorSave saveinvEgresosProductos(PagedList<INV_EGRESOS_PRODUCTOS> invEgresosProductos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invEgresosProductos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_EGRESOS_PRODUCTOS.Any(invEgresosProducto => invEgresosProducto.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_EGRESOS_PRODUCTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<ModelDobraDatabase.INV_INGRESOS> listinvIngresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ModelDobraDatabase.INV_INGRESOS> invIngresos = new PagedList<ModelDobraDatabase.INV_INGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invIngresos.Results = db.INV_INGRESOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invIngresos.Total = invIngresos.Results.Count;
				invIngresos.Count = invIngresos.Results.Count;
			}
			return invIngresos;

		}

		public ErrorSave saveinvIngresos(PagedList<ModelDobraDatabase.INV_INGRESOS> invIngresos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invIngresos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_INGRESOS.Any(invIngreso => invIngreso.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_INGRESOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_INGRESOS_RUBROS> listinvIngresosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_INGRESOS_RUBROS> invIngresosRubros = new PagedList<INV_INGRESOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				invIngresosRubros.Results = db.INV_INGRESOS_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invIngresosRubros.Total = invIngresosRubros.Results.Count;
				invIngresosRubros.Count = invIngresosRubros.Results.Count;
			}
			return invIngresosRubros;

		}

		public ErrorSave saveinvIngresosRubros(PagedList<INV_INGRESOS_RUBROS> invIngresosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invIngresosRubros.Results)
					{
						//errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_INGRESOS_RUBROS.Any(invIngreso => invIngreso.DivisaID == item.DivisaID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_INGRESOS_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> listinvIngresosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> invIngresosProductos = new PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invIngresosProductos.Results = db.INV_INGRESOS_PRODUCTOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invIngresosProductos.Total = invIngresosProductos.Results.Count;
				invIngresosProductos.Count = invIngresosProductos.Results.Count;
			}
			return invIngresosProductos;

		}

		public ErrorSave saveinvIngresosProductos(PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> invIngresosProductos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invIngresosProductos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_INGRESOS_PRODUCTOS.Any(invIngreso => invIngreso.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_INGRESOS_PRODUCTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		/*public PagedList<INV_PROMOCIONES> listinvPromociones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PROMOCIONES> invPromociones = new PagedList<INV_PROMOCIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				invPromociones.Results = db.INV_PROMOCIONES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invPromociones.Total = invPromociones.Results.Count;
				invPromociones.Count = invPromociones.Results.Count;
			}
			return invPromociones;

		}*/

		/*public ErrorSave saveinvPromociones(PagedList<INV_PROMOCIONES> invPromociones)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invPromociones.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_PROMOCIONES.Any(invPromocion => invPromocion.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PROMOCIONES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/

		/*public PagedList<INV_PROMOCIONES_DT> listInvPromocionesDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PROMOCIONES_DT> invPromocionesDt = new PagedList<INV_PROMOCIONES_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				invPromocionesDt.Results = db.INV_PROMOCIONES_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invPromocionesDt.Total = invPromocionesDt.Results.Count;
				invPromocionesDt.Count = invPromocionesDt.Results.Count;
			}
			return invPromocionesDt;

		}*/

	/*	public ErrorSave saveInvPromocionesDt(PagedList<INV_PROMOCIONES_DT> invPromocionesDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invPromocionesDt.Results)
					{
						errorSave.errorMessage=item.ID.ToString();

						try
						{
							if (db.INV_PROMOCIONES_DT.Any(invPromocionDt => invPromocionDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PROMOCIONES_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/

		/*public PagedList<INV_PROMOCIONES_DT2> listInvPromocionesDt2(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PROMOCIONES_DT2> invPromocionesDt2 = new PagedList<INV_PROMOCIONES_DT2>();
			using (DobraConnection db = new DobraConnection())
			{
				invPromocionesDt2.Results = db.INV_PROMOCIONES_DT2.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invPromocionesDt2.Total = invPromocionesDt2.Results.Count;
				invPromocionesDt2.Count = invPromocionesDt2.Results.Count;
			}
			return invPromocionesDt2;

		}*/

		/*public ErrorSave saveInvPromocionesDt2(PagedList<INV_PROMOCIONES_DT2> invPromocionesDt2)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage = "";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invPromocionesDt2.Results)
					{
						errorSave.errorMessage=item.ID.ToString();

						try
						{
							if (db.INV_PROMOCIONES_DT2.Any(invPromocionDt2 => invPromocionDt2.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_PROMOCIONES_DT2.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/



		public PagedList<INV_TRANSFERENCIAS> listinvTransferencias(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_TRANSFERENCIAS> invTransferencias = new PagedList<INV_TRANSFERENCIAS>();
			using (DobraConnection db = new DobraConnection())
			{
				invTransferencias.Results = db.INV_TRANSFERENCIAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				invTransferencias.Total = invTransferencias.Results.Count;
				invTransferencias.Count = invTransferencias.Results.Count;
			}
			return invTransferencias;

		}

		public ErrorSave saveinvTransferencias(PagedList<INV_TRANSFERENCIAS> invTransferencias)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invTransferencias.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_TRANSFERENCIAS.Any(invTransferencia => invTransferencia.ID == item.ID   &&  invTransferencia.Estado!= "RECIBIDO"))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();


							}
							else if (db.INV_TRANSFERENCIAS.Any(invTransferencia => invTransferencia.ID == item.ID && invTransferencia.Estado == "RECIBIDO"))
							{

							}
							else
							{
								db.INV_TRANSFERENCIAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<INV_TRANSFERENCIAS_DT> listinvTransferenciasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_TRANSFERENCIAS_DT> invTransferenciasDt = new PagedList<INV_TRANSFERENCIAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				invTransferenciasDt.Results = db.INV_TRANSFERENCIAS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				invTransferenciasDt.Total = invTransferenciasDt.Results.Count;
				invTransferenciasDt.Count = invTransferenciasDt.Results.Count;
			}
			return invTransferenciasDt;

		}

		public ErrorSave saveinvTransferenciasDt(PagedList<INV_TRANSFERENCIAS_DT> invTransferenciasDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="";

			using (DobraConnection db = new DobraConnection())
			{

				errorSave.Listid=  invTransferenciasDt.Results.Select(dt => dt.ProductoID).ToList();


				try
				{
					foreach (var item in invTransferenciasDt.Results)
					{
						errorSave.errorMessage = errorSave.errorMessage + item.ID;

						try
						{
							if (db.INV_TRANSFERENCIAS_DT.Any(invTransferenciaDt => invTransferenciaDt.ID == item.ID     ))
							{

								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_TRANSFERENCIAS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{

					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<POS_TRANSFERENCIAS> listPosTransferencias(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<POS_TRANSFERENCIAS> posTransferencias = new PagedList<POS_TRANSFERENCIAS>();
			using (DobraConnection db = new DobraConnection())
			{
				posTransferencias.Results = db.POS_TRANSFERENCIAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				posTransferencias.Total = posTransferencias.Results.Count;
				posTransferencias.Count = posTransferencias.Results.Count;
			}
			return posTransferencias;

		}

		public ErrorSave savePosTransferencias(PagedList<POS_TRANSFERENCIAS> posTransferencias)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in posTransferencias.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.POS_TRANSFERENCIAS.Any(posTransferencia => posTransferencia.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.POS_TRANSFERENCIAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}

					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public void encontrarError(Exception e, ErrorSave errorSave)
		{

			if (e.InnerException == null)
			{
				errorSave.errorMessage = errorSave.errorMessage +"  Error de excepcion: "+e.Message;
				errorSave.errorExit = true;
			}
			else
			{
				encontrarError(e.InnerException, errorSave);
			}
		}

		public void encontrarError2<T>(Exception e, ErrorSave errorSave)
		{

			if (e.InnerException == null)
			{
				errorSave.errorMessage = errorSave.errorMessage + "  Error de excepcion: " + e.Message;
				errorSave.errorExit = true;
			}
			else
			{
				encontrarError2<T>(e.InnerException, errorSave);
			}
		}


		public PagedList<POS_TRANSFERENCIAS_DT> listPosTransferenciasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<POS_TRANSFERENCIAS_DT> posTransferenciasDt = new PagedList<POS_TRANSFERENCIAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				posTransferenciasDt.Results = db.POS_TRANSFERENCIAS_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();
				posTransferenciasDt.Total = posTransferenciasDt.Results.Count;
				posTransferenciasDt.Count = posTransferenciasDt.Results.Count;
			}
			return posTransferenciasDt;

		}

		public ErrorSave savePosTransferenciasDt(PagedList<POS_TRANSFERENCIAS_DT> posTransferenciasDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in posTransferenciasDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.POS_TRANSFERENCIAS_DT.Any(posTransferenciaDt => posTransferenciaDt.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.POS_TRANSFERENCIAS_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<CLI_CREDITOS_DEUDAS> listCliCreditosDuedas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_CREDITOS_DEUDAS> cliCreditosDeudas = new PagedList<CLI_CREDITOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditosDeudas.Results = db.CLI_CREDITOS_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliCreditosDeudas.Total = cliCreditosDeudas.Results.Count;
				cliCreditosDeudas.Count = cliCreditosDeudas.Results.Count;
			}
			return cliCreditosDeudas;

		}

		public ErrorSave saveCliCreditosDuedas(PagedList<CLI_CREDITOS_DEUDAS> cliCreditosDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cliCreditosDeudas.Results)
					{
						errorSave.errorMessage = errorSave.errorMessage + "\n" + "ID:  " + item.id;

						try
						{
							if (db.CLI_CREDITOS_DEUDAS.Any(cliCreditoDeuda => cliCreditoDeuda.id == item.id))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_CREDITOS_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<CLI_CREDITOS_RUBROS> listCliCreditosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_CREDITOS_RUBROS> cliCreditosRubros = new PagedList<CLI_CREDITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditosRubros.Results = db.CLI_CREDITOS_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliCreditosRubros.Total = cliCreditosRubros.Results.Count;
				cliCreditosRubros.Count = cliCreditosRubros.Results.Count;
			}
			return cliCreditosRubros;

		}

		public ErrorSave saveCliCreditosRubros(PagedList<CLI_CREDITOS_RUBROS> cliCreditosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cliCreditosRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_CREDITOS_RUBROS.Any(cliCreditoRubro => cliCreditoRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_CREDITOS_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}

				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}



		public PagedList<CLI_DEBITOS> listCliDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_DEBITOS> cliDebitos = new PagedList<CLI_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliDebitos.Results = db.CLI_DEBITOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliDebitos.Total = cliDebitos.Results.Count;
				cliDebitos.Count = cliDebitos.Results.Count;
			}
			return cliDebitos;

		}

		public ErrorSave saveCliDebitos(PagedList<CLI_DEBITOS> cliDebitos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cliDebitos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_DEBITOS.Any(cliDebito => cliDebito.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_DEBITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<CLI_DEBITOS_RUBROS> listCliDebitosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_DEBITOS_RUBROS> cliDebitosRubros = new PagedList<CLI_DEBITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliDebitosRubros.Results = db.CLI_DEBITOS_RUBROS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliDebitosRubros.Total = cliDebitosRubros.Results.Count;
				cliDebitosRubros.Count = cliDebitosRubros.Results.Count;
			}
			return cliDebitosRubros;

		}

		public ErrorSave saveCliDebitosRubros(PagedList<CLI_DEBITOS_RUBROS> cliDebitosRubros)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cliDebitosRubros.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_DEBITOS_RUBROS.Any(cliDebitoRubro => cliDebitoRubro.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_DEBITOS_RUBROS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<CLI_RETENCIONES> listCliRetenciones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_RETENCIONES> cliRetenciones = new PagedList<CLI_RETENCIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				cliRetenciones.Results = db.CLI_RETENCIONES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliRetenciones.Total = cliRetenciones.Results.Count;
				cliRetenciones.Count = cliRetenciones.Results.Count;
			}
			return cliRetenciones;

		}

		public ErrorSave saveCliRetenciones(PagedList<CLI_RETENCIONES> cliRetenciones)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cliRetenciones.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.CLI_RETENCIONES.Any(cliRetencion => cliRetencion.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_RETENCIONES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<CLI_RETENCIONES_DEUDAS> listCliRetencionesDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_RETENCIONES_DEUDAS> cliRetencionesDeudas = new PagedList<CLI_RETENCIONES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliRetencionesDeudas.Results = db.CLI_RETENCIONES_DEUDAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliRetencionesDeudas.Total = cliRetencionesDeudas.Results.Count;
				cliRetencionesDeudas.Count = cliRetencionesDeudas.Results.Count;
			}
			return cliRetencionesDeudas;

		}

		public ErrorSave saveCliRetencionesDeudas(PagedList<CLI_RETENCIONES_DEUDAS> cliRetencionesDeudas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in cliRetencionesDeudas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.DivisaID;

						try
						{
							if (db.CLI_RETENCIONES_DEUDAS.Any(cliRetencionDeuda => cliRetencionDeuda.DivisaID == item.DivisaID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_RETENCIONES_DEUDAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<CLI_RETENCIONES_DT> listCliRetencionesDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<CLI_RETENCIONES_DT> cliRetencionesDt = new PagedList<CLI_RETENCIONES_DT>();
			using (DobraConnection db = new DobraConnection())
			{

				cliRetencionesDt.Results = db.CLI_RETENCIONES_DT.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliRetencionesDt.Total = cliRetencionesDt.Results.Count;
				cliRetencionesDt.Count = cliRetencionesDt.Results.Count;
			}
			return cliRetencionesDt;

		}

		public ErrorSave saveCliRetencionesDt(PagedList<CLI_RETENCIONES_DT> cliRetencionesDt)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{

					foreach (var item in cliRetencionesDt.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.id;

						try
						{
							if (db.CLI_RETENCIONES_DT.Any(cliRetencionDt => cliRetencionDt.id == item.id))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.CLI_RETENCIONES_DT.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<ACR_ACREEDORES> listAcrAcreedores(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ACR_ACREEDORES> acrAcreedores = new PagedList<ACR_ACREEDORES>();
			using (DobraConnection db = new DobraConnection())
			{

				acrAcreedores.Results = db.ACR_ACREEDORES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrAcreedores.Total = acrAcreedores.Results.Count;
				acrAcreedores.Count = acrAcreedores.Results.Count;
			}
			return acrAcreedores;

		}

		public ErrorSave saveAcrAcreedores(PagedList<ACR_ACREEDORES> acrAcreedores)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in acrAcreedores.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ACR_ACREEDORES.Any(acrAcrredor => acrAcrredor.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ACR_ACREEDORES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}

		public PagedList<INV_GRUPOS> listInvGrupos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_GRUPOS> invGrupos = new PagedList<INV_GRUPOS>();
			using (DobraConnection db = new DobraConnection())
			{

				invGrupos.Results = db.INV_GRUPOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invGrupos.Total = invGrupos.Results.Count;
				invGrupos.Count = invGrupos.Results.Count;
			}
			return invGrupos;

		}

		public ErrorSave saveInvGrupos(PagedList<INV_GRUPOS> invGrupos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in invGrupos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.INV_GRUPOS.Any(invGrupo => invGrupo.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.INV_GRUPOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		/*public PagedList<PRV_FACTURAS_PAGOS> listPrvFacturasPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<PRV_FACTURAS_PAGOS> prvFacturasPagos = new PagedList<PRV_FACTURAS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{

				prvFacturasPagos.Results = db.PRV_FACTURAS_PAGOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				prvFacturasPagos.Total = prvFacturasPagos.Results.Count;
				prvFacturasPagos.Count = prvFacturasPagos.Results.Count;
			}
			return prvFacturasPagos;

		}*/

	/*	public ErrorSave savePrvFacturasPagos(PagedList<PRV_FACTURAS_PAGOS> prvFacturasPagos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in prvFacturasPagos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.PRV_FACTURAS_PAGOS.Any(prvFacturas => prvFacturas.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.PRV_FACTURAS_PAGOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}*/

		public PagedList<BAN_CREDITOS> listBanCreditos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_CREDITOS> banCreditos = new PagedList<BAN_CREDITOS>();
			using (DobraConnection db = new DobraConnection())
			{

				banCreditos.Results = db.BAN_CREDITOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banCreditos.Total = banCreditos.Results.Count;
				banCreditos.Count = banCreditos.Results.Count;
			}
			return banCreditos;

		}


		public ErrorSave saveBanCreditos(PagedList<BAN_CREDITOS> banCreditos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banCreditos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_CREDITOS.Any(banCredito => banCredito.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_CREDITOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<BAN_CREDITOS_CUENTAS> listBanCreditosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<BAN_CREDITOS_CUENTAS> banCreditosCuentas = new PagedList<BAN_CREDITOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{

				banCreditosCuentas.Results = db.BAN_CREDITOS_CUENTAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banCreditosCuentas.Total = banCreditosCuentas.Results.Count;
				banCreditosCuentas.Count = banCreditosCuentas.Results.Count;
			}
			return banCreditosCuentas;

		}


		public ErrorSave saveBanCreditosCuentas(PagedList<BAN_CREDITOS_CUENTAS> banCreditosCuentas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in banCreditosCuentas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.BAN_CREDITOS_CUENTAS.Any(banCreditoCuenta => banCreditoCuenta.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.BAN_CREDITOS_CUENTAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ORG_BUZONES> listOrgBuzones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ORG_BUZONES> orgBuzones = new PagedList<ORG_BUZONES>();
			using (DobraConnection db = new DobraConnection())
			{

				orgBuzones.Results = db.ORG_BUZONES.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				orgBuzones.Total = orgBuzones.Results.Count;
				orgBuzones.Count = orgBuzones.Results.Count;
			}
			return orgBuzones;

		}


		public ErrorSave saveOrgBuzones(PagedList<ORG_BUZONES> orgBuzones)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in orgBuzones.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ORG_BUZONES.Any(orgBuzon => orgBuzon.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ORG_BUZONES.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ORG_DOCUMENTOS> listOrgDocumentos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ORG_DOCUMENTOS> orgDocumentos = new PagedList<ORG_DOCUMENTOS>();
			using (DobraConnection db = new DobraConnection())
			{

				orgDocumentos.Results = db.ORG_DOCUMENTOS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				orgDocumentos.Total = orgDocumentos.Results.Count;
				orgDocumentos.Count = orgDocumentos.Results.Count;
			}
			return orgDocumentos;

		}

		public ErrorSave saveOrgDocumentos(PagedList<ORG_DOCUMENTOS> orgDocuemntos)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in orgDocuemntos.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ORG_DOCUMENTOS.Any(orgDocumento => orgDocumento.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ORG_DOCUMENTOS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}


		public PagedList<ORG_TAREAS> listOrgTareas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<ORG_TAREAS> orgTareas = new PagedList<ORG_TAREAS>();
			using (DobraConnection db = new DobraConnection())
			{

				orgTareas.Results = db.ORG_TAREAS.Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				orgTareas.Total = orgTareas.Results.Count;
				orgTareas.Count = orgTareas.Results.Count;
			}
			return orgTareas;

		}

		public ErrorSave saveOrgTareas(PagedList<ORG_TAREAS> orgTareas)
		{
			ErrorSave errorSave = new ErrorSave();

			errorSave.errorMessage="ID:  ";

			using (DobraConnection db = new DobraConnection())
			{
				try
				{
					foreach (var item in orgTareas.Results)
					{
						errorSave.errorMessage=errorSave.errorMessage+"\n" + "ID:  "+item.ID;

						try
						{
							if (db.ORG_TAREAS.Any(orgTarea => orgTarea.ID == item.ID))
							{
								db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								db.SaveChanges();
							}
							else
							{
								db.ORG_TAREAS.Add(item);
								db.SaveChanges();
							}
						}
						catch (Exception e)
						{
							encontrarError(e, errorSave);
						}
					}
				}
				catch (Exception e)
				{
					encontrarError(e, errorSave);
				}
			}
			return errorSave;
		}





	}

}