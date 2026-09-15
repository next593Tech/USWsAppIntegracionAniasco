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
			var errorSave = new ErrorSave { Tabla = "CLI_CLIENTES", errorExit = false };
			if (clients == null || clients.Results == null || clients.Results.Count == 0) return errorSave;

			var totalCount = clients.Results.Count;
			int chunkSize = 500;

			for (int offset = 0; offset < totalCount; offset += chunkSize)
			{
				var chunk = clients.Results.Skip(offset).Take(chunkSize).ToList();
				var distinctChunk = chunk.GroupBy(x => x.ID).Select(g => g.Key == null ? g.First() : g.Last()).ToList();
				var chunkIds = distinctChunk.Select(c => c.ID).Where(id => id != null).Distinct().ToList();
				var chunkCodes = distinctChunk.Select(c => c.Código != null ? c.Código.Trim() : "").Where(c => c != "").Distinct().ToList();

				using (var db = new DobraConnection())
				{
					db.Configuration.AutoDetectChangesEnabled = false;
					db.Configuration.ValidateOnSaveEnabled = false;

					using (var tx = db.Database.BeginTransaction())
					{
						try
						{
							var existing = db.CLI_CLIENTES
								.Where(c => chunkIds.Contains(c.ID) || chunkCodes.Contains(c.Código.Trim()))
								.Select(c => new { c.ID, Código = c.Código.Trim() })
								.ToList();

							var existingById = existing.ToDictionary(c => c.ID, c => c.Código);
							var existingByCode = existing.GroupBy(c => c.Código).ToDictionary(g => g.Key, g => g.First().ID);

							foreach (var item in distinctChunk)
							{
								var itemCode = item.Código != null ? item.Código.Trim() : "";
								var hasId = existingById.TryGetValue(item.ID, out var dbCodeForId);
								var hasCode = existingByCode.TryGetValue(itemCode, out var dbIdForCode);

								if (hasId && dbCodeForId == itemCode)
								{
									db.Entry(item).State = System.Data.Entity.EntityState.Modified;
								}
								else if (hasCode && dbIdForCode != item.ID)
								{
									errorSave.errorExit = true;
									errorSave.errorMessage = (errorSave.errorMessage ?? "") + "\nID diferente y cédula igual: " + item.ID;
								}
								else if (hasId && dbCodeForId != itemCode)
								{
									errorSave.errorExit = true;
									errorSave.errorMessage = (errorSave.errorMessage ?? "") + "\nID igual y cédula diferente: " + item.ID;
								}
								else
								{
									db.CLI_CLIENTES.Add(item);
								}
							}

							db.Configuration.AutoDetectChangesEnabled = true;
							db.SaveChanges();
							tx.Commit();
						}
						catch (Exception ex)
						{
							try { tx.Rollback(); } catch { }
							BatchSyncHelper.FormatError(ex, "CLI_CLIENTES", distinctChunk, x => x.ID, errorSave);
							return errorSave;
						}
					}
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
			return BatchSyncHelper.ExecuteList(db => db.CLI_CLIENTES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}


		public PagedList<INV_PRODUCTOS> listProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveProducts(PagedList<INV_PRODUCTOS> products)
		{
			if (products == null || products.Results == null || products.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				products.Results,
				"INV_PRODUCTOS",
				x => x.ID,
				db => db.INV_PRODUCTOS,
				(db, keys) => new HashSet<string>(db.INV_PRODUCTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}




		public PagedList<INV_PRODUCTOS> listProductsByIDerror(ErrorSave errorSave)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS.AsNoTracking().Where(e => errorSave.Listid.Contains(e.ID)));
		}


		public PagedList<INV_PRODUCTOS_EMPAQUES> listProductsEmpaqueByIDerror(ErrorSave errorSave)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS_EMPAQUES.AsNoTracking().Where(e => errorSave.Listid.Contains(e.ProductoID)));
		}


		public PagedList<INV_PRODUCTOS_PRECIOS> listProductoPrecioByIDerror(ErrorSave errorSave)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS_PRECIOS.AsNoTracking().Where(e => errorSave.Listid.Contains(e.ProductoID)));
		}




		public ErrorSave saveProductsListProductID(PagedList<INV_PRODUCTOS> products)
		{
			if (products == null || products.Results == null || products.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				products.Results,
				"INV_PRODUCTOS",
				x => x.ID,
				db => db.INV_PRODUCTOS,
				(db, keys) => new HashSet<string>(db.INV_PRODUCTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_PRODUCTOS_EMPAQUES> listPackagesProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS_EMPAQUES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}


		public ErrorSave savePackageProductos(PagedList<INV_PRODUCTOS_EMPAQUES> products)
		{
			if (products == null || products.Results == null || products.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				products.Results,
				"INV_PRODUCTOS_EMPAQUES",
				x => x.ID,
				db => db.INV_PRODUCTOS_EMPAQUES,
				(db, keys) => new HashSet<string>(db.INV_PRODUCTOS_EMPAQUES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<INV_PRODUCTOS_PRECIOS> listPriceProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS_PRECIOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave savePriceProducts(PagedList<INV_PRODUCTOS_PRECIOS> products)
		{
			if (products == null || products.Results == null || products.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				products.Results,
				"INV_PRODUCTOS_PRECIOS",
				x => x.ID,
				db => db.INV_PRODUCTOS_PRECIOS,
				(db, keys) => new HashSet<string>(db.INV_PRODUCTOS_PRECIOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<INV_COMBOS> listComboProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_COMBOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCombos(PagedList<INV_COMBOS> products)
		{
			if (products == null || products.Results == null || products.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				products.Results,
				"INV_COMBOS",
				x => x.ID,
				db => db.INV_COMBOS,
				(db, keys) => new HashSet<string>(db.INV_COMBOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_COMBOS_COMPONENTES> listComboComponentesProducts(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_COMBOS_COMPONENTES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || e.ExportadoDate > lastUpdate));
		}

		public ErrorSave saveComboComponente(PagedList<INV_COMBOS_COMPONENTES> comboComponentes)
		{
			if (comboComponentes == null || comboComponentes.Results == null || comboComponentes.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSaveComposite(
				comboComponentes.Results,
				"INV_COMBOS_COMPONENTES",
				x => x.ComboID + "|" + x.ProductoID,
				db => db.INV_COMBOS_COMPONENTES,
				(db, chunk) =>
				{
					var comboIds = chunk.Select(c => c.ComboID).Distinct().ToList();
					var prodIds = chunk.Select(c => c.ProductoID).Distinct().ToList();
					var existing = db.INV_COMBOS_COMPONENTES
						.Where(x => comboIds.Contains(x.ComboID) && prodIds.Contains(x.ProductoID))
						.Select(x => new { x.ComboID, x.ProductoID })
						.ToList();
					return new HashSet<string>(existing.Select(x => x.ComboID + "|" + x.ProductoID));
				}
			);
		}


		/*public PagedList<INV_PD_BODEGA_STOCK> listPdBodegaStock(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PD_BODEGA_STOCK> packages = new PagedList<INV_PD_BODEGA_STOCK>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PD_BODEGA_STOCK.AsNoTracking().Where(e => e.ExportadoDate > lastUpdate).ToList<INV_PD_BODEGA_STOCK>();
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
			return BatchSyncHelper.ExecuteList(db => db.INV_PRECIOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvPrecio(PagedList<INV_PRECIOS> precios)
		{
			if (precios == null || precios.Results == null || precios.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				precios.Results,
				"INV_PRECIOS",
				x => x.ID,
				db => db.INV_PRECIOS,
				(db, keys) => new HashSet<string>(db.INV_PRECIOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_PRECIOS_DT> listInvPreciosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRECIOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvPrecioDt(PagedList<INV_PRECIOS_DT> precios)
		{
			if (precios == null || precios.Results == null || precios.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSaveComposite(
				precios.Results,
				"INV_PRECIOS_DT",
				x => x.PrecioID + "|" + x.ProductoID,
				db => db.INV_PRECIOS_DT,
				(db, chunk) =>
				{
					var precioIds = chunk.Select(c => c.PrecioID).Distinct().ToList();
					var prodIds = chunk.Select(c => c.ProductoID).Distinct().ToList();
					var existing = db.INV_PRECIOS_DT
						.Where(x => precioIds.Contains(x.PrecioID) && prodIds.Contains(x.ProductoID))
						.Select(x => new { x.PrecioID, x.ProductoID })
						.ToList();
					return new HashSet<string>(existing.Select(x => x.PrecioID + "|" + x.ProductoID));
				}
			);
		}


		public PagedList<INV_PRODUCTOS_STOCK> listInvProductsStock(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS_STOCK.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvProductsStock(PagedList<INV_PRODUCTOS_STOCK> precios)
		{
			if (precios == null || precios.Results == null || precios.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				precios.Results,
				"INV_PRODUCTOS_STOCK",
				x => x.ProductoID,
				db => db.INV_PRODUCTOS_STOCK,
				(db, keys) => new HashSet<string>(db.INV_PRODUCTOS_STOCK.Where(x => keys.Contains(x.ProductoID)).Select(x => x.ProductoID))
			);
		}

		public PagedList<INV_RUBROS> listInvRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvRubros(PagedList<INV_RUBROS> precios)
		{
			if (precios == null || precios.Results == null || precios.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				precios.Results,
				"INV_RUBROS",
				x => x.ID,
				db => db.INV_RUBROS,
				(db, keys) => new HashSet<string>(db.INV_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<ACC_CUENTAS> listAccCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACC_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAccCuentas(PagedList<ACC_CUENTAS> cuentas)
		{
			if (cuentas == null || cuentas.Results == null || cuentas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cuentas.Results,
				"ACC_CUENTAS",
				x => x.ID,
				db => db.ACC_CUENTAS,
				(db, keys) => new HashSet<string>(db.ACC_CUENTAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<EMP_EMPLEADOS> listEmployess(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_EMPLEADOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmployess(PagedList<EMP_EMPLEADOS> employes)
		{
			if (employes == null || employes.Results == null || employes.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				employes.Results,
				"EMP_EMPLEADOS",
				x => x.ID,
				db => db.EMP_EMPLEADOS,
				(db, keys) => new HashSet<string>(db.EMP_EMPLEADOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_BANCOS> listBanks(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_BANCOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanks(PagedList<BAN_BANCOS> bancos)
		{
			if (bancos == null || bancos.Results == null || bancos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				bancos.Results,
				"BAN_BANCOS",
				x => x.ID,
				db => db.BAN_BANCOS,
				(db, keys) => new HashSet<string>(db.BAN_BANCOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<CLI_RUBROS> listCliRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliRubros(PagedList<CLI_RUBROS> cliRubros)
		{
			if (cliRubros == null || cliRubros.Results == null || cliRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliRubros.Results,
				"CLI_RUBROS",
				x => x.ID,
				db => db.CLI_RUBROS,
				(db, keys) => new HashSet<string>(db.CLI_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_EMPAQUES> listInvPackages(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_EMPAQUES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}



		public ErrorSave saveInvPackages(PagedList<INV_EMPAQUES> empauqes)
		{
			if (empauqes == null || empauqes.Results == null || empauqes.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				empauqes.Results,
				"INV_EMPAQUES",
				x => x.ID,
				db => db.INV_EMPAQUES,
				(db, keys) => new HashSet<string>(db.INV_EMPAQUES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<SEG_PERFILES> listSegProfiles(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SEG_PERFILES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveSegProfiles(PagedList<SEG_PERFILES> segPerfiles)
		{
			if (segPerfiles == null || segPerfiles.Results == null || segPerfiles.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				segPerfiles.Results,
				"SEG_PERFILES",
				x => x.id,
				db => db.SEG_PERFILES,
				(db, keys) => new HashSet<string>(db.SEG_PERFILES.Where(x => keys.Contains(x.id)).Select(x => x.id))
			);
		}

		public PagedList<SEG_RECURSOS> listSegRecursos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SEG_RECURSOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveSegRecursos(PagedList<SEG_RECURSOS> segRecursos)
		{
			if (segRecursos == null || segRecursos.Results == null || segRecursos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				segRecursos.Results,
				"SEG_RECURSOS",
				x => x.ID,
				db => db.SEG_RECURSOS,
				(db, keys) => new HashSet<string>(db.SEG_RECURSOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<SEG_USUARIOS> listSegUsuarios(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SEG_USUARIOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveSegUsuarios(PagedList<SEG_USUARIOS> segUsuarios)
		{
			if (segUsuarios == null || segUsuarios.Results == null || segUsuarios.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				segUsuarios.Results,
				"SEG_USUARIOS",
				x => x.ID,
				db => db.SEG_USUARIOS,
				(db, keys) => new HashSet<string>(db.SEG_USUARIOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<SIS_DIVISIONES> listSisDivisiones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SIS_DIVISIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveDivisiones(PagedList<SIS_DIVISIONES> sisDivisiones)
		{
			if (sisDivisiones == null || sisDivisiones.Results == null || sisDivisiones.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				sisDivisiones.Results,
				"SIS_DIVISIONES",
				x => x.ID,
				db => db.SIS_DIVISIONES,
				(db, keys) => new HashSet<string>(db.SIS_DIVISIONES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<SIS_PARAMETROS> listSisParametros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SIS_PARAMETROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveSisParametros(PagedList<SIS_PARAMETROS> sisDivisiones)
		{
			if (sisDivisiones == null || sisDivisiones.Results == null || sisDivisiones.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				sisDivisiones.Results,
				"SIS_PARAMETROS",
				x => x.ID,
				db => db.SIS_PARAMETROS,
				(db, keys) => new HashSet<string>(db.SIS_PARAMETROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<SIS_SUCURSALES> listSisSucursales(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SIS_SUCURSALES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}


		public PagedList<SIS_ZONAS> listSisZonas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SIS_ZONAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveSisZonas(PagedList<SIS_ZONAS> sisZonas)
		{
			if (sisZonas == null || sisZonas.Results == null || sisZonas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				sisZonas.Results,
				"SIS_ZONAS",
				x => x.ID,
				db => db.SIS_ZONAS,
				(db, keys) => new HashSet<string>(db.SIS_ZONAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public ErrorSave saveSisSucursales(PagedList<SIS_SUCURSALES> sisSucursales)
		{
			if (sisSucursales == null || sisSucursales.Results == null || sisSucursales.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				sisSucursales.Results,
				"SIS_SUCURSALES",
				x => x.ID,
				db => db.SIS_SUCURSALES,
				(db, keys) => new HashSet<string>(db.SIS_SUCURSALES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<SRI_SECUENCIAL> listSriSecuencial(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SRI_SECUENCIAL.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveSriSecuencial(PagedList<SRI_SECUENCIAL> sriSecuencial)
		{
			if (sriSecuencial == null || sriSecuencial.Results == null || sriSecuencial.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				sriSecuencial.Results,
				"SRI_SECUENCIAL",
				x => x.ID,
				db => db.SRI_SECUENCIAL,
				(db, keys) => new HashSet<string>(db.SRI_SECUENCIAL.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<SEG_PERFILES_RECURSOS> listSegPerfilesRecursos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.SEG_PERFILES_RECURSOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveSegPerfilesRecursos(PagedList<SEG_PERFILES_RECURSOS> perfilesRecuros)
		{
			if (perfilesRecuros == null || perfilesRecuros.Results == null || perfilesRecuros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				perfilesRecuros.Results,
				"SEG_PERFILES_RECURSOS",
				x => x.id,
				db => db.SEG_PERFILES_RECURSOS,
				(db, keys) => new HashSet<string>(db.SEG_PERFILES_RECURSOS.Where(x => keys.Contains(x.id)).Select(x => x.id))
			);
		}

		public PagedList<ACC_ASIENTOS> listAccAsientos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACC_ASIENTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAccAsientos(PagedList<ACC_ASIENTOS> perfilesRecuros)
		{
			if (perfilesRecuros == null || perfilesRecuros.Results == null || perfilesRecuros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				perfilesRecuros.Results,
				"ACC_ASIENTOS",
				x => x.ID,
				db => db.ACC_ASIENTOS,
				(db, keys) => new HashSet<string>(db.ACC_ASIENTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<ACC_ASIENTOS_DT> listAccAsientosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACC_ASIENTOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || e.ExportadoDate > lastUpdate));
		}

		public ErrorSave saveAccAsientosDt(PagedList<ACC_ASIENTOS_DT> accAsientosDt)
		{
			if (accAsientosDt == null || accAsientosDt.Results == null || accAsientosDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				accAsientosDt.Results,
				"ACC_ASIENTOS_DT",
				x => x.ID,
				db => db.ACC_ASIENTOS_DT,
				(db, keys) => new HashSet<string>(db.ACC_ASIENTOS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_INGRESOS> listBanIngresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_INGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanIngresos(PagedList<BAN_INGRESOS> banIngresos)
		{
			if (banIngresos == null || banIngresos.Results == null || banIngresos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banIngresos.Results,
				"BAN_INGRESOS",
				x => x.ID,
				db => db.BAN_INGRESOS,
				(db, keys) => new HashSet<string>(db.BAN_INGRESOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_INGRESOS_DT> listBanIngresosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_INGRESOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanIngresosDt(PagedList<BAN_INGRESOS_DT> banIngresosDt)
		{
			if (banIngresosDt == null || banIngresosDt.Results == null || banIngresosDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banIngresosDt.Results,
				"BAN_INGRESOS_DT",
				x => x.ID,
				db => db.BAN_INGRESOS_DT,
				(db, keys) => new HashSet<string>(db.BAN_INGRESOS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<CLI_CLIENTES_DEUDAS> listClientesDeduas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_CLIENTES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveClienteDeudas(PagedList<CLI_CLIENTES_DEUDAS> clienteDeudas)
		{
			if (clienteDeudas == null || clienteDeudas.Results == null || clienteDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				clienteDeudas.Results,
				"CLI_CLIENTES_DEUDAS",
				x => x.ID,
				db => db.CLI_CLIENTES_DEUDAS,
				(db, keys) => new HashSet<string>(db.CLI_CLIENTES_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<ModelDobraDatabase.CLI_CREDITOS> listCliCreditos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_CREDITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliCreditos(PagedList<ModelDobraDatabase.CLI_CREDITOS> clienteDeudas)
		{
			if (clienteDeudas == null || clienteDeudas.Results == null || clienteDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				clienteDeudas.Results,
				"CLI_CREDITOS",
				x => x.ID,
				db => db.CLI_CREDITOS,
				(db, keys) => new HashSet<string>(db.CLI_CREDITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> listCliCreditosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_CREDITOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliCreditosProductos(PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> clienteCreditoProductos)
		{
			if (clienteCreditoProductos == null || clienteCreditoProductos.Results == null || clienteCreditoProductos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				clienteCreditoProductos.Results,
				"CLI_CREDITOS_PRODUCTOS",
				x => x.ID,
				db => db.CLI_CREDITOS_PRODUCTOS,
				(db, keys) => new HashSet<string>(db.CLI_CREDITOS_PRODUCTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<INV_PRODUCTOS_CARDEX> listInvProductosCardex(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PRODUCTOS_CARDEX.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvProductosCardex(PagedList<INV_PRODUCTOS_CARDEX> productosCardex)
		{
			if (productosCardex == null || productosCardex.Results == null || productosCardex.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				productosCardex.Results,
				"INV_PRODUCTOS_CARDEX",
				x => x.ID,
				db => db.INV_PRODUCTOS_CARDEX,
				(db, keys) => new HashSet<long>(db.INV_PRODUCTOS_CARDEX.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<POS_CIERRES_CAJA> listPosCierresCajas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.POS_CIERRES_CAJA.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave savePosCierresCajas(PagedList<POS_CIERRES_CAJA> productosCardex)
		{
			if (productosCardex == null || productosCardex.Results == null || productosCardex.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				productosCardex.Results,
				"POS_CIERRES_CAJA",
				x => x.ID,
				db => db.POS_CIERRES_CAJA,
				(db, keys) => new HashSet<string>(db.POS_CIERRES_CAJA.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<POS_CIERRES> listPosCierres(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.POS_CIERRES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave savePosCierres(PagedList<POS_CIERRES> posCierres)
		{
			if (posCierres == null || posCierres.Results == null || posCierres.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				posCierres.Results,
				"POS_CIERRES",
				x => x.ID,
				db => db.POS_CIERRES,
				(db, keys) => new HashSet<string>(db.POS_CIERRES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<VEN_FACTURAS> listVenFacturas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.VEN_FACTURAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveVenFacturas(PagedList<VEN_FACTURAS> venFacturas)
		{
			if (venFacturas == null || venFacturas.Results == null || venFacturas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				venFacturas.Results,
				"VEN_FACTURAS",
				x => x.ID,
				db => db.VEN_FACTURAS,
				(db, keys) => new HashSet<string>(db.VEN_FACTURAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<VEN_FACTURAS_DT> listVenFacturasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.VEN_FACTURAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveVenFacturasDt(PagedList<VEN_FACTURAS_DT> venFacturas)
		{
			if (venFacturas == null || venFacturas.Results == null || venFacturas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				venFacturas.Results,
				"VEN_FACTURAS_DT",
				x => x.ID,
				db => db.VEN_FACTURAS_DT,
				(db, keys) => new HashSet<string>(db.VEN_FACTURAS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_INGRESOS_DEUDAS> listBanIngresoDeuda(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_INGRESOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanIngresoDeuda(PagedList<BAN_INGRESOS_DEUDAS> banIngresosDeudas)
		{
			if (banIngresosDeudas == null || banIngresosDeudas.Results == null || banIngresosDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banIngresosDeudas.Results,
				"BAN_INGRESOS_DEUDAS",
				x => x.ID,
				db => db.BAN_INGRESOS_DEUDAS,
				(db, keys) => new HashSet<string>(db.BAN_INGRESOS_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_BANCOS_CARDEX> listBanBancosCardex(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_BANCOS_CARDEX.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanBancosCardex(PagedList<BAN_BANCOS_CARDEX> banBancoCardex)
		{
			if (banBancoCardex == null || banBancoCardex.Results == null || banBancoCardex.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banBancoCardex.Results,
				"BAN_BANCOS_CARDEX",
				x => x.ID,
				db => db.BAN_BANCOS_CARDEX,
				(db, keys) => new HashSet<string>(db.BAN_BANCOS_CARDEX.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_DEPOSITOS> listBanDepositos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_DEPOSITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanDepositos(PagedList<BAN_DEPOSITOS> banDepositos)
		{
			if (banDepositos == null || banDepositos.Results == null || banDepositos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banDepositos.Results,
				"BAN_DEPOSITOS",
				x => x.ID,
				db => db.BAN_DEPOSITOS,
				(db, keys) => new HashSet<string>(db.BAN_DEPOSITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_DEPOSITOS_DT> listBanDepositosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_DEPOSITOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanDepositosDt(PagedList<BAN_DEPOSITOS_DT> banDepositosDt)
		{
			if (banDepositosDt == null || banDepositosDt.Results == null || banDepositosDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banDepositosDt.Results,
				"BAN_DEPOSITOS_DT",
				x => x.ID,
				db => db.BAN_DEPOSITOS_DT,
				(db, keys) => new HashSet<string>(db.BAN_DEPOSITOS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_DEPOSITOS_PAPELETAS> listBanDepositoPapeletas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_DEPOSITOS_PAPELETAS.AsNoTracking().Where(e => e.CreadoDAte >= lastUpdate));
		}

		public ErrorSave saveBanDepositoPapeletas(PagedList<BAN_DEPOSITOS_PAPELETAS> banDepositosPapeletas)
		{
			if (banDepositosPapeletas == null || banDepositosPapeletas.Results == null || banDepositosPapeletas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banDepositosPapeletas.Results,
				"BAN_DEPOSITOS_PAPELETAS",
				x => x.ID,
				db => db.BAN_DEPOSITOS_PAPELETAS,
				(db, keys) => new HashSet<string>(db.BAN_DEPOSITOS_PAPELETAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<COM_FACTURAS> listComFacturas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.COM_FACTURAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveComFacturas(PagedList<COM_FACTURAS> comFacturas)
		{
			if (comFacturas == null || comFacturas.Results == null || comFacturas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				comFacturas.Results,
				"COM_FACTURAS",
				x => x.ID,
				db => db.COM_FACTURAS,
				(db, keys) => new HashSet<string>(db.COM_FACTURAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<COM_FACTURAS_DT> listComFacturasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.COM_FACTURAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveComFacturasDt(PagedList<COM_FACTURAS_DT> comFacturasDt)
		{
			if (comFacturasDt == null || comFacturasDt.Results == null || comFacturasDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				comFacturasDt.Results,
				"COM_FACTURAS_DT",
				x => x.ID,
				db => db.COM_FACTURAS_DT,
				(db, keys) => new HashSet<string>(db.COM_FACTURAS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<COM_FACTURAS_PAGOS> listComFacturasPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.COM_FACTURAS_PAGOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveComFacturasPagos(PagedList<COM_FACTURAS_PAGOS> comFacturasPagos)
		{
			if (comFacturasPagos == null || comFacturasPagos.Results == null || comFacturasPagos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				comFacturasPagos.Results,
				"COM_FACTURAS_PAGOS",
				x => x.ID,
				db => db.COM_FACTURAS_PAGOS,
				(db, keys) => new HashSet<string>(db.COM_FACTURAS_PAGOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_RETENCIONES> listAcrRetenciones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_RETENCIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrRetenciones(PagedList<ACR_RETENCIONES> acrRetenciones)
		{
			if (acrRetenciones == null || acrRetenciones.Results == null || acrRetenciones.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrRetenciones.Results,
				"ACR_RETENCIONES",
				x => x.ID,
				db => db.ACR_RETENCIONES,
				(db, keys) => new HashSet<string>(db.ACR_RETENCIONES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}





		public PagedList<ACR_RETENCIONES_DT> listAcrRetencionesDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_RETENCIONES_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrRetencionesDt(PagedList<ACR_RETENCIONES_DT> acrRetencionesDt)
		{
			if (acrRetencionesDt == null || acrRetencionesDt.Results == null || acrRetencionesDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrRetencionesDt.Results,
				"ACR_RETENCIONES_DT",
				x => x.ID,
				db => db.ACR_RETENCIONES_DT,
				(db, keys) => new HashSet<string>(db.ACR_RETENCIONES_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<ACR_RETENCIONES_DEUDAS> listAcrRetencionesDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_RETENCIONES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}


		public ErrorSave saveAcrRetencionesDeudas(PagedList<ACR_RETENCIONES_DEUDAS> acrRetencionesDeudas)
		{
			if (acrRetencionesDeudas == null || acrRetencionesDeudas.Results == null || acrRetencionesDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrRetencionesDeudas.Results,
				"ACR_RETENCIONES_DEUDAS",
				x => x.ID,
				db => db.ACR_RETENCIONES_DEUDAS,
				(db, keys) => new HashSet<string>(db.ACR_RETENCIONES_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_ACREEDORES_DEUDAS> listAcrAcreedoresDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_ACREEDORES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrAcreedoresDeudas(PagedList<ACR_ACREEDORES_DEUDAS> acrAcreedoresDeudas)
		{
			if (acrAcreedoresDeudas == null || acrAcreedoresDeudas.Results == null || acrAcreedoresDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrAcreedoresDeudas.Results,
				"ACR_ACREEDORES_DEUDAS",
				x => x.ID,
				db => db.ACR_ACREEDORES_DEUDAS,
				(db, keys) => new HashSet<string>(db.ACR_ACREEDORES_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<PRV_FACTURAS> listPvrFacturas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.PRV_FACTURAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savePvrFacturas(PagedList<PRV_FACTURAS> pvrFacturas)
		{
			if (pvrFacturas == null || pvrFacturas.Results == null || pvrFacturas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				pvrFacturas.Results,
				"PRV_FACTURAS",
				x => x.ID,
				db => db.PRV_FACTURAS,
				(db, keys) => new HashSet<string>(db.PRV_FACTURAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<PRV_FACTURAS_DT> listPvrFacturasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.PRV_FACTURAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savePvrFacturasDt(PagedList<PRV_FACTURAS_DT> pvrFacturasDt)
		{
			if (pvrFacturasDt == null || pvrFacturasDt.Results == null || pvrFacturasDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				pvrFacturasDt.Results,
				"PRV_FACTURAS_DT",
				x => x.ID,
				db => db.PRV_FACTURAS_DT,
				(db, keys) => new HashSet<string>(db.PRV_FACTURAS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<PRV_FACTURASCTA_DT> listPvrFacturasCtaDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.PRV_FACTURASCTA_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savePvrFacturasCtaDt(PagedList<PRV_FACTURASCTA_DT> pvrFacturasCtaDt)
		{
			if (pvrFacturasCtaDt == null || pvrFacturasCtaDt.Results == null || pvrFacturasCtaDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				pvrFacturasCtaDt.Results,
				"PRV_FACTURASCTA_DT",
				x => x.ID,
				db => db.PRV_FACTURASCTA_DT,
				(db, keys) => new HashSet<string>(db.PRV_FACTURASCTA_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<EMP_ROLES> listEmpRoles(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_ROLES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmpRoles(PagedList<EMP_ROLES> empRoles)
		{
			if (empRoles == null || empRoles.Results == null || empRoles.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				empRoles.Results,
				"EMP_ROLES",
				x => x.ID,
				db => db.EMP_ROLES,
				(db, keys) => new HashSet<string>(db.EMP_ROLES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<EMP_ROLES_EMPLEADOS> listEmpRolesEmpleados(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_ROLES_EMPLEADOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmpRolesEmpleados(PagedList<EMP_ROLES_EMPLEADOS> empRolesEmpleados)
		{
			if (empRolesEmpleados == null || empRolesEmpleados.Results == null || empRolesEmpleados.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSaveComposite(
				empRolesEmpleados.Results,
				"EMP_ROLES_EMPLEADOS",
				x => x.RolID + "|" + x.EmpleadoID,
				db => db.EMP_ROLES_EMPLEADOS,
				(db, chunk) =>
				{
					var rolIds = chunk.Select(c => c.RolID).Distinct().ToList();
					var empIds = chunk.Select(c => c.EmpleadoID).Distinct().ToList();
					var existing = db.EMP_ROLES_EMPLEADOS
						.Where(x => rolIds.Contains(x.RolID) && empIds.Contains(x.EmpleadoID))
						.Select(x => new { x.RolID, x.EmpleadoID })
						.ToList();
					return new HashSet<string>(existing.Select(x => x.RolID + "|" + x.EmpleadoID));
				}
			);
		}

		public PagedList<EMP_ROLES_RUBROS> listEmpRolesRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_ROLES_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmpRolesRubros(PagedList<EMP_ROLES_RUBROS> empRolesRubros)
		{
			if (empRolesRubros == null || empRolesRubros.Results == null || empRolesRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				empRolesRubros.Results,
				"EMP_ROLES_RUBROS",
				x => x.ID,
				db => db.EMP_ROLES_RUBROS,
				(db, keys) => new HashSet<string>(db.EMP_ROLES_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<EMP_EMPLEADOS_DEUDAS> listEmpEmpleadosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_EMPLEADOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmpEmpleadosDeudas(PagedList<EMP_EMPLEADOS_DEUDAS> empEmpleadosDeudas)
		{
			if (empEmpleadosDeudas == null || empEmpleadosDeudas.Results == null || empEmpleadosDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				empEmpleadosDeudas.Results,
				"EMP_EMPLEADOS_DEUDAS",
				x => x.ID,
				db => db.EMP_EMPLEADOS_DEUDAS,
				(db, keys) => new HashSet<string>(db.EMP_EMPLEADOS_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<EMP_EMPLEADOS_HORAS> listEmpEmpleadosHoras(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_EMPLEADOS_HORAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmpEmpleadosHoras(PagedList<EMP_EMPLEADOS_HORAS> empEmpleadosHoras)
		{
			if (empEmpleadosHoras == null || empEmpleadosHoras.Results == null || empEmpleadosHoras.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSaveComposite(
				empEmpleadosHoras.Results,
				"EMP_EMPLEADOS_HORAS",
				x => x.Año + "|" + x.Mes + "|" + x.EmpleadoID,
				db => db.EMP_EMPLEADOS_HORAS,
				(db, chunk) =>
				{
					var empIds = chunk.Select(c => c.EmpleadoID).Distinct().ToList();
					var existing = db.EMP_EMPLEADOS_HORAS
						.Where(x => empIds.Contains(x.EmpleadoID))
						.Select(x => new { x.Año, x.Mes, x.EmpleadoID })
						.ToList();
					return new HashSet<string>(existing.Select(x => x.Año + "|" + x.Mes + "|" + x.EmpleadoID));
				}
			);
		}



		public PagedList<EMP_DEBITOS> listEmpDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmpDebitos(PagedList<EMP_DEBITOS> empDebitos)
		{
			if (empDebitos == null || empDebitos.Results == null || empDebitos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				empDebitos.Results,
				"EMP_DEBITOS",
				x => x.ID,
				db => db.EMP_DEBITOS,
				(db, keys) => new HashSet<string>(db.EMP_DEBITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}




		public PagedList<EMP_DEBITOS_RUBROS> listEmpDebitosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.EMP_DEBITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveEmpDebitosRubros(PagedList<EMP_DEBITOS_RUBROS> empDebitosRubros)
		{
			if (empDebitosRubros == null || empDebitosRubros.Results == null || empDebitosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				empDebitosRubros.Results,
				"EMP_DEBITOS_RUBROS",
				x => x.ID,
				db => db.EMP_DEBITOS_RUBROS,
				(db, keys) => new HashSet<string>(db.EMP_DEBITOS_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<CLI_GRUPOS> listCliGrupos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_GRUPOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliGrupos(PagedList<CLI_GRUPOS> cliGrupos)
		{
			if (cliGrupos == null || cliGrupos.Results == null || cliGrupos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliGrupos.Results,
				"CLI_GRUPOS",
				x => x.ID,
				db => db.CLI_GRUPOS,
				(db, keys) => new HashSet<string>(db.CLI_GRUPOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_BODEGAS> listInvBodegas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_BODEGAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvBodegas(PagedList<INV_BODEGAS> invBodegas)
		{
			if (invBodegas == null || invBodegas.Results == null || invBodegas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invBodegas.Results,
				"INV_BODEGAS",
				x => x.ID,
				db => db.INV_BODEGAS,
				(db, keys) => new HashSet<string>(db.INV_BODEGAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		/*public PagedList<INV_PRODUCTOS_EXHIBICION> listInvProductosExhibicion(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<INV_PRODUCTOS_EXHIBICION> inProductosExhibicion = new PagedList<INV_PRODUCTOS_EXHIBICION>();
			using (DobraConnection db = new DobraConnection())
			{
				inProductosExhibicion.Results = db.INV_PRODUCTOS_EXHIBICION.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

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
			return BatchSyncHelper.ExecuteList(db => db.ACR_CREDITOS.AsNoTracking().Where(e => e.Fecha >= lastUpdate));
		}

		public ErrorSave saveAcrCreditos(PagedList<ACR_CREDITOS> empDebitosRubros)
		{
			if (empDebitosRubros == null || empDebitosRubros.Results == null || empDebitosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				empDebitosRubros.Results,
				"ACR_CREDITOS",
				x => x.ID,
				db => db.ACR_CREDITOS,
				(db, keys) => new HashSet<string>(db.ACR_CREDITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<ACR_CREDITOS_DEUDAS> listAcrCreditosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_CREDITOS_DEUDAS.AsNoTracking().Where(e => e.CreadoDate >= lastUpdate));
		}

		public ErrorSave saveAcrCreditosDeudas(PagedList<ACR_CREDITOS_DEUDAS> acrCreditosDeudas)
		{
			if (acrCreditosDeudas == null || acrCreditosDeudas.Results == null || acrCreditosDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrCreditosDeudas.Results,
				"ACR_CREDITOS_DEUDAS",
				x => x.ID,
				db => db.ACR_CREDITOS_DEUDAS,
				(db, keys) => new HashSet<string>(db.ACR_CREDITOS_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_CREDITOS_RUBROS> listAcrCreditosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_CREDITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrCreditosRubros(PagedList<ACR_CREDITOS_RUBROS> acrCreditosRubros)
		{
			if (acrCreditosRubros == null || acrCreditosRubros.Results == null || acrCreditosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrCreditosRubros.Results,
				"ACR_CREDITOS_RUBROS",
				x => x.ID,
				db => db.ACR_CREDITOS_RUBROS,
				(db, keys) => new HashSet<string>(db.ACR_CREDITOS_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_DEBITOS> listAcrDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrDebitos(PagedList<ACR_DEBITOS> acrDebitos)
		{
			if (acrDebitos == null || acrDebitos.Results == null || acrDebitos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrDebitos.Results,
				"ACR_DEBITOS",
				x => x.ID,
				db => db.ACR_DEBITOS,
				(db, keys) => new HashSet<string>(db.ACR_DEBITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_DEBITOS_DEUDAS> listAcrDebitosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_DEBITOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrDebitosDeudas(PagedList<ACR_DEBITOS_DEUDAS> acrDebitosDeudas)
		{
			if (acrDebitosDeudas == null || acrDebitosDeudas.Results == null || acrDebitosDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrDebitosDeudas.Results,
				"ACR_DEBITOS_DEUDAS",
				x => x.ID,
				db => db.ACR_DEBITOS_DEUDAS,
				(db, keys) => new HashSet<string>(db.ACR_DEBITOS_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_DEBITOS_RUBROS> listAcrDebitoRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_DEBITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrDebitoRubros(PagedList<ACR_DEBITOS_RUBROS> acrDebitosRubros)
		{
			if (acrDebitosRubros == null || acrDebitosRubros.Results == null || acrDebitosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrDebitosRubros.Results,
				"ACR_DEBITOS_RUBROS",
				x => x.ID,
				db => db.ACR_DEBITOS_RUBROS,
				(db, keys) => new HashSet<string>(db.ACR_DEBITOS_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<ACR_DEBITOS_PRODUCTOS> listAcrDebitosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_DEBITOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrDebitosProductos(PagedList<ACR_DEBITOS_PRODUCTOS> acrDebitosProductos)
		{
			if (acrDebitosProductos == null || acrDebitosProductos.Results == null || acrDebitosProductos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrDebitosProductos.Results,
				"ACR_DEBITOS_PRODUCTOS",
				x => x.ID,
				db => db.ACR_DEBITOS_PRODUCTOS,
				(db, keys) => new HashSet<string>(db.ACR_DEBITOS_PRODUCTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_RECIBOS> listAcrRecibos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_RECIBOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrRecibos(PagedList<ACR_RECIBOS> acrRecibos)
		{
			if (acrRecibos == null || acrRecibos.Results == null || acrRecibos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrRecibos.Results,
				"ACR_RECIBOS",
				x => x.ID,
				db => db.ACR_RECIBOS,
				(db, keys) => new HashSet<string>(db.ACR_RECIBOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<ACR_RECIBOS_DT> listAcrRecibosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_RECIBOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}


		public ErrorSave saveAcrRecibosDt(PagedList<ACR_RECIBOS_DT> acrRecibosDt)
		{
			if (acrRecibosDt == null || acrRecibosDt.Results == null || acrRecibosDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrRecibosDt.Results,
				"ACR_RECIBOS_DT",
				x => x.ID,
				db => db.ACR_RECIBOS_DT,
				(db, keys) => new HashSet<string>(db.ACR_RECIBOS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<ACR_RECIBOS_DEUDAS> listAcrReciboDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_RECIBOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrReciboDeudas(PagedList<ACR_RECIBOS_DEUDAS> acrRecibosDeudas)
		{
			if (acrRecibosDeudas == null || acrRecibosDeudas.Results == null || acrRecibosDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrRecibosDeudas.Results,
				"ACR_RECIBOS_DEUDAS",
				x => x.ID,
				db => db.ACR_RECIBOS_DEUDAS,
				(db, keys) => new HashSet<string>(db.ACR_RECIBOS_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_DEBITOS> listBanDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanDebitos(PagedList<BAN_DEBITOS> banDebitos)
		{
			if (banDebitos == null || banDebitos.Results == null || banDebitos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banDebitos.Results,
				"BAN_DEBITOS",
				x => x.ID,
				db => db.BAN_DEBITOS,
				(db, keys) => new HashSet<string>(db.BAN_DEBITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_DEBITOS_CUENTAS> listBanDebitosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_DEBITOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanDebitosCuentas(PagedList<BAN_DEBITOS_CUENTAS> banDebitosCuentas)
		{
			if (banDebitosCuentas == null || banDebitosCuentas.Results == null || banDebitosCuentas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banDebitosCuentas.Results,
				"BAN_DEBITOS_CUENTAS",
				x => x.ID,
				db => db.BAN_DEBITOS_CUENTAS,
				(db, keys) => new HashSet<string>(db.BAN_DEBITOS_CUENTAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_EGRESOS> listbanEgresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_EGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanEgresos(PagedList<BAN_EGRESOS> banEgresos)
		{
			if (banEgresos == null || banEgresos.Results == null || banEgresos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banEgresos.Results,
				"BAN_EGRESOS",
				x => x.ID,
				db => db.BAN_EGRESOS,
				(db, keys) => new HashSet<string>(db.BAN_EGRESOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_EGRESOS_ANEXOS> listbanEgresosAnexos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_EGRESOS_ANEXOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanEgresosAnexos(PagedList<BAN_EGRESOS_ANEXOS> banEgresosAnexos)
		{
			if (banEgresosAnexos == null || banEgresosAnexos.Results == null || banEgresosAnexos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banEgresosAnexos.Results,
				"BAN_EGRESOS_ANEXOS",
				x => x.ID,
				db => db.BAN_EGRESOS_ANEXOS,
				(db, keys) => new HashSet<string>(db.BAN_EGRESOS_ANEXOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_EGRESOS_ANTICIPOS> listbanEgresosAnticipos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_EGRESOS_ANTICIPOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanEgresosAnticipos(PagedList<BAN_EGRESOS_ANTICIPOS> banEgresosAnticipos)
		{
			if (banEgresosAnticipos == null || banEgresosAnticipos.Results == null || banEgresosAnticipos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banEgresosAnticipos.Results,
				"BAN_EGRESOS_ANTICIPOS",
				x => x.ID,
				db => db.BAN_EGRESOS_ANTICIPOS,
				(db, keys) => new HashSet<string>(db.BAN_EGRESOS_ANTICIPOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<BAN_EGRESOS_CUENTAS> listbanEgresosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_EGRESOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanEgresosCuentas(PagedList<BAN_EGRESOS_CUENTAS> banEgresosCuentas)
		{
			if (banEgresosCuentas == null || banEgresosCuentas.Results == null || banEgresosCuentas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banEgresosCuentas.Results,
				"BAN_EGRESOS_CUENTAS",
				x => x.ID,
				db => db.BAN_EGRESOS_CUENTAS,
				(db, keys) => new HashSet<string>(db.BAN_EGRESOS_CUENTAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_EGRESOS_DEUDAS> listbanEgresosDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_EGRESOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanEgresosDeudas(PagedList<BAN_EGRESOS_DEUDAS> banEgresosDeudas)
		{
			if (banEgresosDeudas == null || banEgresosDeudas.Results == null || banEgresosDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banEgresosDeudas.Results,
				"BAN_EGRESOS_DEUDAS",
				x => x.ID,
				db => db.BAN_EGRESOS_DEUDAS,
				(db, keys) => new HashSet<string>(db.BAN_EGRESOS_DEUDAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}




		public PagedList<BAN_EGRESOS_DT> listbanEgresosDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_EGRESOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanEgresosDt(PagedList<BAN_EGRESOS_DT> banEgresosDt)
		{
			if (banEgresosDt == null || banEgresosDt.Results == null || banEgresosDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banEgresosDt.Results,
				"BAN_EGRESOS_DT",
				x => x.ID,
				db => db.BAN_EGRESOS_DT,
				(db, keys) => new HashSet<string>(db.BAN_EGRESOS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_EGRESOS_PAGOS> listbanEgresosPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_EGRESOS_PAGOS.AsNoTracking().Where(e => e.ExportadoDate > lastUpdate));
		}

		public ErrorSave savebanEgresosPagos(PagedList<BAN_EGRESOS_PAGOS> banEgresosPagos)
		{
			if (banEgresosPagos == null || banEgresosPagos.Results == null || banEgresosPagos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banEgresosPagos.Results,
				"BAN_EGRESOS_PAGOS",
				x => x.ID,
				db => db.BAN_EGRESOS_PAGOS,
				(db, keys) => new HashSet<string>(db.BAN_EGRESOS_PAGOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_INGRESOS_CUENTAS> listbanIngresosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_INGRESOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanIngresosCuentas(PagedList<BAN_INGRESOS_CUENTAS> banIngresosCuentas)
		{
			if (banIngresosCuentas == null || banIngresosCuentas.Results == null || banIngresosCuentas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banIngresosCuentas.Results,
				"BAN_INGRESOS_CUENTAS",
				x => x.ID,
				db => db.BAN_INGRESOS_CUENTAS,
				(db, keys) => new HashSet<string>(db.BAN_INGRESOS_CUENTAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<BAN_INGRESOS_PINPAD> listBanIngresoPinpad(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_INGRESOS_PINPAD.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveBanIngresoPinpad(PagedList<BAN_INGRESOS_PINPAD> banIngresosPinpad)
		{
			if (banIngresosPinpad == null || banIngresosPinpad.Results == null || banIngresosPinpad.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banIngresosPinpad.Results,
				"BAN_INGRESOS_PINPAD",
				x => x.ID,
				db => db.BAN_INGRESOS_PINPAD,
				(db, keys) => new HashSet<string>(db.BAN_INGRESOS_PINPAD.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}





		public PagedList<BAN_INGRESOS_TARJETAS> listbanIngresosTarjetas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_INGRESOS_TARJETAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanIngresosTarjetas(PagedList<BAN_INGRESOS_TARJETAS> banIngresosTarjetas)
		{
			if (banIngresosTarjetas == null || banIngresosTarjetas.Results == null || banIngresosTarjetas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banIngresosTarjetas.Results,
				"BAN_INGRESOS_TARJETAS",
				x => x.ID,
				db => db.BAN_INGRESOS_TARJETAS,
				(db, keys) => new HashSet<string>(db.BAN_INGRESOS_TARJETAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_PAPELETAS> listbanPapeletas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_PAPELETAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanPapeletas(PagedList<BAN_PAPELETAS> banPapeletas)
		{
			if (banPapeletas == null || banPapeletas.Results == null || banPapeletas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banPapeletas.Results,
				"BAN_PAPELETAS",
				x => x.ID,
				db => db.BAN_PAPELETAS,
				(db, keys) => new HashSet<string>(db.BAN_PAPELETAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<BAN_TRANSFERENCIAS> listbanTransferencias(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_TRANSFERENCIAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanTransferencias(PagedList<BAN_TRANSFERENCIAS> banTransferencias)
		{
			if (banTransferencias == null || banTransferencias.Results == null || banTransferencias.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banTransferencias.Results,
				"BAN_TRANSFERENCIAS",
				x => x.ID,
				db => db.BAN_TRANSFERENCIAS,
				(db, keys) => new HashSet<string>(db.BAN_TRANSFERENCIAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_TRANSFERENCIAS_DT> listbanTransferenciasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_TRANSFERENCIAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savebanTransferenciasDt(PagedList<BAN_TRANSFERENCIAS_DT> banTransferenciasDt)
		{
			if (banTransferenciasDt == null || banTransferenciasDt.Results == null || banTransferenciasDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banTransferenciasDt.Results,
				"BAN_TRANSFERENCIAS_DT",
				x => x.ID,
				db => db.BAN_TRANSFERENCIAS_DT,
				(db, keys) => new HashSet<string>(db.BAN_TRANSFERENCIAS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<VEN_FACTURAS_PAGOS> listvenFacturasPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.VEN_FACTURAS_PAGOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savevenFacturasPagos(PagedList<VEN_FACTURAS_PAGOS> venFacturasPagos)
		{
			if (venFacturasPagos == null || venFacturasPagos.Results == null || venFacturasPagos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				venFacturasPagos.Results,
				"VEN_FACTURAS_PAGOS",
				x => x.id,
				db => db.VEN_FACTURAS_PAGOS,
				(db, keys) => new HashSet<string>(db.VEN_FACTURAS_PAGOS.Where(x => keys.Contains(x.id)).Select(x => x.id))
			);
		}

		public PagedList<INV_EGRESOS> listinvEgresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_EGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvEgresos(PagedList<INV_EGRESOS> invEgresos)
		{
			if (invEgresos == null || invEgresos.Results == null || invEgresos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invEgresos.Results,
				"INV_EGRESOS",
				x => x.ID,
				db => db.INV_EGRESOS,
				(db, keys) => new HashSet<string>(db.INV_EGRESOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_EGRESOS_RUBROS> listinvEgresosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_EGRESOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvEgresosRubros(PagedList<INV_EGRESOS_RUBROS> invEgresosRubros)
		{
			if (invEgresosRubros == null || invEgresosRubros.Results == null || invEgresosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invEgresosRubros.Results,
				"INV_EGRESOS_RUBROS",
				x => x.ID,
				db => db.INV_EGRESOS_RUBROS,
				(db, keys) => new HashSet<string>(db.INV_EGRESOS_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<INV_EGRESOS_PRODUCTOS> listinvEgresosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_EGRESOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvEgresosProductos(PagedList<INV_EGRESOS_PRODUCTOS> invEgresosProductos)
		{
			if (invEgresosProductos == null || invEgresosProductos.Results == null || invEgresosProductos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invEgresosProductos.Results,
				"INV_EGRESOS_PRODUCTOS",
				x => x.ID,
				db => db.INV_EGRESOS_PRODUCTOS,
				(db, keys) => new HashSet<string>(db.INV_EGRESOS_PRODUCTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<ModelDobraDatabase.INV_INGRESOS> listinvIngresos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_INGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvIngresos(PagedList<ModelDobraDatabase.INV_INGRESOS> invIngresos)
		{
			if (invIngresos == null || invIngresos.Results == null || invIngresos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invIngresos.Results,
				"INV_INGRESOS",
				x => x.ID,
				db => db.INV_INGRESOS,
				(db, keys) => new HashSet<string>(db.INV_INGRESOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<INV_INGRESOS_RUBROS> listinvIngresosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_INGRESOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvIngresosRubros(PagedList<INV_INGRESOS_RUBROS> invIngresosRubros)
		{
			if (invIngresosRubros == null || invIngresosRubros.Results == null || invIngresosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invIngresosRubros.Results,
				"INV_INGRESOS_RUBROS",
				x => x.DivisaID,
				db => db.INV_INGRESOS_RUBROS,
				(db, keys) => new HashSet<string>(db.INV_INGRESOS_RUBROS.Where(x => keys.Contains(x.DivisaID)).Select(x => x.DivisaID))
			);
		}


		public PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> listinvIngresosProductos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_INGRESOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvIngresosProductos(PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> invIngresosProductos)
		{
			if (invIngresosProductos == null || invIngresosProductos.Results == null || invIngresosProductos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invIngresosProductos.Results,
				"INV_INGRESOS_PRODUCTOS",
				x => x.ID,
				db => db.INV_INGRESOS_PRODUCTOS,
				(db, keys) => new HashSet<string>(db.INV_INGRESOS_PRODUCTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_PROMOCIONES> listinvPromociones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PROMOCIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvPromociones(PagedList<INV_PROMOCIONES> invPromociones)
		{
			if (invPromociones == null || invPromociones.Results == null || invPromociones.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invPromociones.Results,
				"INV_PROMOCIONES",
				x => x.ID,
				db => db.INV_PROMOCIONES,
				(db, keys) => new HashSet<string>(db.INV_PROMOCIONES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_PROMOCIONES_DT> listInvPromocionesDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PROMOCIONES_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvPromocionesDt(PagedList<INV_PROMOCIONES_DT> invPromocionesDt)
		{
			if (invPromocionesDt == null || invPromocionesDt.Results == null || invPromocionesDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invPromocionesDt.Results,
				"INV_PROMOCIONES_DT",
				x => x.id,
				db => db.INV_PROMOCIONES_DT,
				(db, keys) => new HashSet<string>(db.INV_PROMOCIONES_DT.Where(x => keys.Contains(x.id)).Select(x => x.id))
			);
		}

		public PagedList<INV_PROMOCIONES_DT2> listInvPromocionesDt2(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_PROMOCIONES_DT2.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvPromocionesDt2(PagedList<INV_PROMOCIONES_DT2> invPromocionesDt2)
		{
			if (invPromocionesDt2 == null || invPromocionesDt2.Results == null || invPromocionesDt2.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invPromocionesDt2.Results,
				"INV_PROMOCIONES_DT2",
				x => x.id,
				db => db.INV_PROMOCIONES_DT2,
				(db, keys) => new HashSet<long>(db.INV_PROMOCIONES_DT2.Where(x => keys.Contains(x.id)).Select(x => x.id))
			);
		}



		public PagedList<INV_TRANSFERENCIAS> listinvTransferencias(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_TRANSFERENCIAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvTransferencias(PagedList<INV_TRANSFERENCIAS> invTransferencias)
		{
			var errorSave = new ErrorSave { Tabla = "INV_TRANSFERENCIAS", errorExit = false };
			if (invTransferencias == null || invTransferencias.Results == null || invTransferencias.Results.Count == 0) return errorSave;

			var totalCount = invTransferencias.Results.Count;
			int chunkSize = 500;

			for (int offset = 0; offset < totalCount; offset += chunkSize)
			{
				var chunk = invTransferencias.Results.Skip(offset).Take(chunkSize).ToList();
				var distinctChunk = chunk.GroupBy(x => x.ID).Select(g => g.Key == null ? g.First() : g.Last()).ToList();
				var chunkKeys = distinctChunk.Select(x => x.ID).Where(k => k != null).Distinct().ToList();

				using (var db = new DobraConnection())
				{
					db.Configuration.AutoDetectChangesEnabled = false;
					db.Configuration.ValidateOnSaveEnabled = false;

					using (var tx = db.Database.BeginTransaction())
					{
						try
						{
							var existing = db.INV_TRANSFERENCIAS
								.Where(x => chunkKeys.Contains(x.ID))
								.Select(x => new { x.ID, x.Estado })
								.ToList();
							var estadoDict = existing.ToDictionary(x => x.ID, x => x.Estado);

							foreach (var item in distinctChunk)
							{
								if (estadoDict.TryGetValue(item.ID, out var dbEstado))
								{
									if (dbEstado != "RECIBIDO")
									{
										db.Entry(item).State = System.Data.Entity.EntityState.Modified;
									}
								}
								else
								{
									db.INV_TRANSFERENCIAS.Add(item);
								}
							}

							db.Configuration.AutoDetectChangesEnabled = true;
							db.SaveChanges();
							tx.Commit();
						}
						catch (Exception ex)
						{
							try { tx.Rollback(); } catch { }
							BatchSyncHelper.FormatError(ex, "INV_TRANSFERENCIAS", distinctChunk, x => x.ID, errorSave);
							return errorSave;
						}
					}
				}
			}
			return errorSave;
		}


		public PagedList<INV_TRANSFERENCIAS_DT> listinvTransferenciasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_TRANSFERENCIAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)));
		}

		public ErrorSave saveinvTransferenciasDt(PagedList<INV_TRANSFERENCIAS_DT> invTransferenciasDt)
		{
			if (invTransferenciasDt == null || invTransferenciasDt.Results == null || invTransferenciasDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invTransferenciasDt.Results,
				"INV_TRANSFERENCIAS_DT",
				x => x.ID,
				db => db.INV_TRANSFERENCIAS_DT,
				(db, keys) => new HashSet<string>(db.INV_TRANSFERENCIAS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<POS_TRANSFERENCIAS> listPosTransferencias(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.POS_TRANSFERENCIAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savePosTransferencias(PagedList<POS_TRANSFERENCIAS> posTransferencias)
		{
			if (posTransferencias == null || posTransferencias.Results == null || posTransferencias.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				posTransferencias.Results,
				"POS_TRANSFERENCIAS",
				x => x.ID,
				db => db.POS_TRANSFERENCIAS,
				(db, keys) => new HashSet<string>(db.POS_TRANSFERENCIAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public void encontrarError(Exception e, ErrorSave errorSave)
		{
			errorSave.errorExit = true;
			var cleanMsg = BatchSyncHelper.FormatGeneralException(e);
			if (string.IsNullOrEmpty(errorSave.errorMessage) || errorSave.errorMessage.StartsWith("ID:"))
			{
				errorSave.errorMessage = cleanMsg;
			}
			else if (!errorSave.errorMessage.Contains(cleanMsg))
			{
				errorSave.errorMessage = errorSave.errorMessage + "\n" + cleanMsg;
			}
		}

		public void encontrarError2<T>(Exception e, ErrorSave errorSave)
		{
			encontrarError(e, errorSave);
		}


		public PagedList<POS_TRANSFERENCIAS_DT> listPosTransferenciasDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.POS_TRANSFERENCIAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave savePosTransferenciasDt(PagedList<POS_TRANSFERENCIAS_DT> posTransferenciasDt)
		{
			if (posTransferenciasDt == null || posTransferenciasDt.Results == null || posTransferenciasDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				posTransferenciasDt.Results,
				"POS_TRANSFERENCIAS_DT",
				x => x.ID,
				db => db.POS_TRANSFERENCIAS_DT,
				(db, keys) => new HashSet<string>(db.POS_TRANSFERENCIAS_DT.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<CLI_CREDITOS_DEUDAS> listCliCreditosDuedas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_CREDITOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliCreditosDuedas(PagedList<CLI_CREDITOS_DEUDAS> cliCreditosDeudas)
		{
			if (cliCreditosDeudas == null || cliCreditosDeudas.Results == null || cliCreditosDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliCreditosDeudas.Results,
				"CLI_CREDITOS_DEUDAS",
				x => x.id,
				db => db.CLI_CREDITOS_DEUDAS,
				(db, keys) => new HashSet<string>(db.CLI_CREDITOS_DEUDAS.Where(x => keys.Contains(x.id)).Select(x => x.id))
			);
		}



		public PagedList<CLI_CREDITOS_RUBROS> listCliCreditosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_CREDITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliCreditosRubros(PagedList<CLI_CREDITOS_RUBROS> cliCreditosRubros)
		{
			if (cliCreditosRubros == null || cliCreditosRubros.Results == null || cliCreditosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliCreditosRubros.Results,
				"CLI_CREDITOS_RUBROS",
				x => x.ID,
				db => db.CLI_CREDITOS_RUBROS,
				(db, keys) => new HashSet<string>(db.CLI_CREDITOS_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}



		public PagedList<CLI_DEBITOS> listCliDebitos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliDebitos(PagedList<CLI_DEBITOS> cliDebitos)
		{
			if (cliDebitos == null || cliDebitos.Results == null || cliDebitos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliDebitos.Results,
				"CLI_DEBITOS",
				x => x.ID,
				db => db.CLI_DEBITOS,
				(db, keys) => new HashSet<string>(db.CLI_DEBITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<CLI_DEBITOS_RUBROS> listCliDebitosRubros(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_DEBITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliDebitosRubros(PagedList<CLI_DEBITOS_RUBROS> cliDebitosRubros)
		{
			if (cliDebitosRubros == null || cliDebitosRubros.Results == null || cliDebitosRubros.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliDebitosRubros.Results,
				"CLI_DEBITOS_RUBROS",
				x => x.ID,
				db => db.CLI_DEBITOS_RUBROS,
				(db, keys) => new HashSet<string>(db.CLI_DEBITOS_RUBROS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<CLI_RETENCIONES> listCliRetenciones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_RETENCIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliRetenciones(PagedList<CLI_RETENCIONES> cliRetenciones)
		{
			if (cliRetenciones == null || cliRetenciones.Results == null || cliRetenciones.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliRetenciones.Results,
				"CLI_RETENCIONES",
				x => x.ID,
				db => db.CLI_RETENCIONES,
				(db, keys) => new HashSet<string>(db.CLI_RETENCIONES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<CLI_RETENCIONES_DEUDAS> listCliRetencionesDeudas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_RETENCIONES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliRetencionesDeudas(PagedList<CLI_RETENCIONES_DEUDAS> cliRetencionesDeudas)
		{
			if (cliRetencionesDeudas == null || cliRetencionesDeudas.Results == null || cliRetencionesDeudas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliRetencionesDeudas.Results,
				"CLI_RETENCIONES_DEUDAS",
				x => x.DivisaID,
				db => db.CLI_RETENCIONES_DEUDAS,
				(db, keys) => new HashSet<string>(db.CLI_RETENCIONES_DEUDAS.Where(x => keys.Contains(x.DivisaID)).Select(x => x.DivisaID))
			);
		}

		public PagedList<CLI_RETENCIONES_DT> listCliRetencionesDt(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.CLI_RETENCIONES_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveCliRetencionesDt(PagedList<CLI_RETENCIONES_DT> cliRetencionesDt)
		{
			if (cliRetencionesDt == null || cliRetencionesDt.Results == null || cliRetencionesDt.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				cliRetencionesDt.Results,
				"CLI_RETENCIONES_DT",
				x => x.id,
				db => db.CLI_RETENCIONES_DT,
				(db, keys) => new HashSet<string>(db.CLI_RETENCIONES_DT.Where(x => keys.Contains(x.id)).Select(x => x.id))
			);
		}

		public PagedList<ACR_ACREEDORES> listAcrAcreedores(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ACR_ACREEDORES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveAcrAcreedores(PagedList<ACR_ACREEDORES> acrAcreedores)
		{
			if (acrAcreedores == null || acrAcreedores.Results == null || acrAcreedores.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				acrAcreedores.Results,
				"ACR_ACREEDORES",
				x => x.ID,
				db => db.ACR_ACREEDORES,
				(db, keys) => new HashSet<string>(db.ACR_ACREEDORES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}

		public PagedList<INV_GRUPOS> listInvGrupos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.INV_GRUPOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveInvGrupos(PagedList<INV_GRUPOS> invGrupos)
		{
			if (invGrupos == null || invGrupos.Results == null || invGrupos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				invGrupos.Results,
				"INV_GRUPOS",
				x => x.ID,
				db => db.INV_GRUPOS,
				(db, keys) => new HashSet<string>(db.INV_GRUPOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		/*public PagedList<PRV_FACTURAS_PAGOS> listPrvFacturasPagos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<PRV_FACTURAS_PAGOS> prvFacturasPagos = new PagedList<PRV_FACTURAS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{

				prvFacturasPagos.Results = db.PRV_FACTURAS_PAGOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

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
			return BatchSyncHelper.ExecuteList(db => db.BAN_CREDITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}


		public ErrorSave saveBanCreditos(PagedList<BAN_CREDITOS> banCreditos)
		{
			if (banCreditos == null || banCreditos.Results == null || banCreditos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banCreditos.Results,
				"BAN_CREDITOS",
				x => x.ID,
				db => db.BAN_CREDITOS,
				(db, keys) => new HashSet<string>(db.BAN_CREDITOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<BAN_CREDITOS_CUENTAS> listBanCreditosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.BAN_CREDITOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}


		public ErrorSave saveBanCreditosCuentas(PagedList<BAN_CREDITOS_CUENTAS> banCreditosCuentas)
		{
			if (banCreditosCuentas == null || banCreditosCuentas.Results == null || banCreditosCuentas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				banCreditosCuentas.Results,
				"BAN_CREDITOS_CUENTAS",
				x => x.ID,
				db => db.BAN_CREDITOS_CUENTAS,
				(db, keys) => new HashSet<string>(db.BAN_CREDITOS_CUENTAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<ORG_BUZONES> listOrgBuzones(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ORG_BUZONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}


		public ErrorSave saveOrgBuzones(PagedList<ORG_BUZONES> orgBuzones)
		{
			if (orgBuzones == null || orgBuzones.Results == null || orgBuzones.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				orgBuzones.Results,
				"ORG_BUZONES",
				x => x.ID,
				db => db.ORG_BUZONES,
				(db, keys) => new HashSet<string>(db.ORG_BUZONES.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<ORG_DOCUMENTOS> listOrgDocumentos(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ORG_DOCUMENTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveOrgDocumentos(PagedList<ORG_DOCUMENTOS> orgDocuemntos)
		{
			if (orgDocuemntos == null || orgDocuemntos.Results == null || orgDocuemntos.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				orgDocuemntos.Results,
				"ORG_DOCUMENTOS",
				x => x.ID,
				db => db.ORG_DOCUMENTOS,
				(db, keys) => new HashSet<string>(db.ORG_DOCUMENTOS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}


		public PagedList<ORG_TAREAS> listOrgTareas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			return BatchSyncHelper.ExecuteList(db => db.ORG_TAREAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)));
		}

		public ErrorSave saveOrgTareas(PagedList<ORG_TAREAS> orgTareas)
		{
			if (orgTareas == null || orgTareas.Results == null || orgTareas.Results.Count == 0) return new ErrorSave();
			return BatchSyncHelper.ExecuteBatchSave(
				orgTareas.Results,
				"ORG_TAREAS",
				x => x.ID,
				db => db.ORG_TAREAS,
				(db, keys) => new HashSet<string>(db.ORG_TAREAS.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
			);
		}





	}

}
