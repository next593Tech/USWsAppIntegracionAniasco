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
			PagedList<CLI_CLIENTES> cli = new PagedList<CLI_CLIENTES>();
			using (DobraConnection db = new DobraConnection())
			{
				cli.Results = db.CLI_CLIENTES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
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
				products.Results = db.INV_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				products.Total = products.Results.Count;
				products.Count = products.Results.Count;
			}
			return products;
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
			PagedList<INV_PRODUCTOS> products = new PagedList<INV_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				products.Results = db.INV_PRODUCTOS.AsNoTracking().Where(e => errorSave.Listid.Contains(e.ID)).ToList();
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
				products.Results = db.INV_PRODUCTOS_EMPAQUES.AsNoTracking().Where(e => errorSave.Listid.Contains(e.ProductoID)).ToList();
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
				products.Results = db.INV_PRODUCTOS_PRECIOS.AsNoTracking().Where(e => errorSave.Listid.Contains(e.ProductoID)).ToList();
				products.Total = products.Results.Count;
				products.Count = products.Results.Count;
			}
			return products;
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
			PagedList<INV_PRODUCTOS_EMPAQUES> packages = new PagedList<INV_PRODUCTOS_EMPAQUES>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRODUCTOS_EMPAQUES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_PRODUCTOS_EMPAQUES>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<INV_PRODUCTOS_PRECIOS> packages = new PagedList<INV_PRODUCTOS_PRECIOS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRODUCTOS_PRECIOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_PRODUCTOS_PRECIOS>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<INV_COMBOS> packages = new PagedList<INV_COMBOS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_COMBOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_COMBOS>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<INV_COMBOS_COMPONENTES> packages = new PagedList<INV_COMBOS_COMPONENTES>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_COMBOS_COMPONENTES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || e.ExportadoDate > lastUpdate).ToList<INV_COMBOS_COMPONENTES>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<INV_PRECIOS> packages = new PagedList<INV_PRECIOS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRECIOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<INV_PRECIOS_DT> packages = new PagedList<INV_PRECIOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRECIOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<INV_PRODUCTOS_STOCK> packages = new PagedList<INV_PRODUCTOS_STOCK>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_PRODUCTOS_STOCK.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList<INV_PRODUCTOS_STOCK>();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<INV_RUBROS> packages = new PagedList<INV_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.INV_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();
				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<ACC_CUENTAS> packages = new PagedList<ACC_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				packages.Results = db.ACC_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				packages.Total = packages.Results.Count;
				packages.Count = packages.Results.Count;
			}
			return packages;
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
			PagedList<EMP_EMPLEADOS> employess = new PagedList<EMP_EMPLEADOS>();
			using (DobraConnection db = new DobraConnection())
			{
				employess.Results = db.EMP_EMPLEADOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				employess.Total = employess.Results.Count;
				employess.Count = employess.Results.Count;
			}
			return employess;
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
			PagedList<BAN_BANCOS> bancos = new PagedList<BAN_BANCOS>();
			using (DobraConnection db = new DobraConnection())
			{
				bancos.Results = db.BAN_BANCOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<BAN_BANCOS>();

				bancos.Total = bancos.Results.Count;
				bancos.Count = bancos.Results.Count;
			}
			return bancos;
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
			PagedList<CLI_RUBROS> rubros = new PagedList<CLI_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				rubros.Results = db.CLI_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<CLI_RUBROS>();

				rubros.Total = rubros.Results.Count;
				rubros.Count = rubros.Results.Count;
			}
			return rubros;
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
			PagedList<INV_EMPAQUES> empques = new PagedList<INV_EMPAQUES>();
			using (DobraConnection db = new DobraConnection())
			{
				empques.Results = db.INV_EMPAQUES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList<INV_EMPAQUES>();

				empques.Total = empques.Results.Count;
				empques.Count = empques.Results.Count;
			}
			return empques;

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
			PagedList<SEG_PERFILES> perfiles = new PagedList<SEG_PERFILES>();
			using (DobraConnection db = new DobraConnection())
			{
				perfiles.Results = db.SEG_PERFILES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				perfiles.Total = perfiles.Results.Count;
				perfiles.Count = perfiles.Results.Count;
			}
			return perfiles;

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
			PagedList<SEG_RECURSOS> recursos = new PagedList<SEG_RECURSOS>();
			using (DobraConnection db = new DobraConnection())
			{
				recursos.Results = db.SEG_RECURSOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				recursos.Total = recursos.Results.Count;
				recursos.Count = recursos.Results.Count;
			}
			return recursos;

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
			PagedList<SEG_USUARIOS> usuarios = new PagedList<SEG_USUARIOS>();
			using (DobraConnection db = new DobraConnection())
			{
				usuarios.Results = db.SEG_USUARIOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				usuarios.Total = usuarios.Results.Count;
				usuarios.Count = usuarios.Results.Count;
			}
			return usuarios;

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
			PagedList<SIS_DIVISIONES> divisiones = new PagedList<SIS_DIVISIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				divisiones.Results = db.SIS_DIVISIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				divisiones.Total = divisiones.Results.Count;
				divisiones.Count = divisiones.Results.Count;
			}
			return divisiones;

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

			PagedList<SIS_PARAMETROS> parametros = new PagedList<SIS_PARAMETROS>();
			using (DobraConnection db = new DobraConnection())
			{
				parametros.Results = db.SIS_PARAMETROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				parametros.Total = parametros.Results.Count;
				parametros.Count = parametros.Results.Count;
			}
			return parametros;

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
			PagedList<SIS_SUCURSALES> sucursales = new PagedList<SIS_SUCURSALES>();
			using (DobraConnection db = new DobraConnection())
			{
				sucursales.Results = db.SIS_SUCURSALES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				sucursales.Total = sucursales.Results.Count;
				sucursales.Count = sucursales.Results.Count;
			}
			return sucursales;

		}


		public PagedList<SIS_ZONAS> listSisZonas(DateTime lastUpdate, DateTime lastUpdate2)
		{
			PagedList<SIS_ZONAS> sucursales = new PagedList<SIS_ZONAS>();
			using (DobraConnection db = new DobraConnection())
			{
				sucursales.Results = db.SIS_ZONAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				sucursales.Total = sucursales.Results.Count;
				sucursales.Count = sucursales.Results.Count;
			}
			return sucursales;

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
			PagedList<SRI_SECUENCIAL> sriSecuencial = new PagedList<SRI_SECUENCIAL>();
			using (DobraConnection db = new DobraConnection())
			{
				sriSecuencial.Results = db.SRI_SECUENCIAL.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				sriSecuencial.Total = sriSecuencial.Results.Count;
				sriSecuencial.Count = sriSecuencial.Results.Count;
			}
			return sriSecuencial;

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
			PagedList<SEG_PERFILES_RECURSOS> perfilesRecuros = new PagedList<SEG_PERFILES_RECURSOS>();
			using (DobraConnection db = new DobraConnection())
			{
				perfilesRecuros.Results = db.SEG_PERFILES_RECURSOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				perfilesRecuros.Total = perfilesRecuros.Results.Count;
				perfilesRecuros.Count = perfilesRecuros.Results.Count;
			}
			return perfilesRecuros;

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
			PagedList<ACC_ASIENTOS> accCuentas = new PagedList<ACC_ASIENTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				accCuentas.Results = db.ACC_ASIENTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				accCuentas.Total = accCuentas.Results.Count;
				accCuentas.Count = accCuentas.Results.Count;
			}
			return accCuentas;

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
			PagedList<ACC_ASIENTOS_DT> accAsientos = new PagedList<ACC_ASIENTOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				accAsientos.Results = db.ACC_ASIENTOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || e.ExportadoDate > lastUpdate).ToList();

				accAsientos.Total = accAsientos.Results.Count;
				accAsientos.Count = accAsientos.Results.Count;
			}
			return accAsientos;

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
			PagedList<BAN_INGRESOS> banIngresos = new PagedList<BAN_INGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresos.Results = db.BAN_INGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				banIngresos.Total = banIngresos.Results.Count;
				banIngresos.Count = banIngresos.Results.Count;
			}
			return banIngresos;

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
			PagedList<BAN_INGRESOS_DT> banIngresosDt = new PagedList<BAN_INGRESOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosDt.Results = db.BAN_INGRESOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosDt.Total = banIngresosDt.Results.Count;
				banIngresosDt.Count = banIngresosDt.Results.Count;
			}
			return banIngresosDt;

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
			PagedList<CLI_CLIENTES_DEUDAS> clienteDeduaas = new PagedList<CLI_CLIENTES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				clienteDeduaas.Results = db.CLI_CLIENTES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				clienteDeduaas.Total = clienteDeduaas.Results.Count;
				clienteDeduaas.Count = clienteDeduaas.Results.Count;
			}
			return clienteDeduaas;

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
			PagedList<ModelDobraDatabase.CLI_CREDITOS> cliCreditos = new PagedList<ModelDobraDatabase.CLI_CREDITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditos.Results = db.CLI_CREDITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				cliCreditos.Total = cliCreditos.Results.Count;
				cliCreditos.Count = cliCreditos.Results.Count;
			}
			return cliCreditos;

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
			PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> cliCreditosProductos = new PagedList<ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditosProductos.Results = db.CLI_CREDITOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliCreditosProductos.Total = cliCreditosProductos.Results.Count;
				cliCreditosProductos.Count = cliCreditosProductos.Results.Count;
			}
			return cliCreditosProductos;

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
			PagedList<INV_PRODUCTOS_CARDEX> invProductosCardex = new PagedList<INV_PRODUCTOS_CARDEX>();
			using (DobraConnection db = new DobraConnection())
			{
				invProductosCardex.Results = db.INV_PRODUCTOS_CARDEX.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invProductosCardex.Total = invProductosCardex.Results.Count;
				invProductosCardex.Count = invProductosCardex.Results.Count;
			}
			return invProductosCardex;

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
			PagedList<POS_CIERRES_CAJA> posCierresCaja = new PagedList<POS_CIERRES_CAJA>();
			using (DobraConnection db = new DobraConnection())
			{
				posCierresCaja.Results = db.POS_CIERRES_CAJA.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				posCierresCaja.Total = posCierresCaja.Results.Count;
				posCierresCaja.Count = posCierresCaja.Results.Count;
			}
			return posCierresCaja;

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
			PagedList<POS_CIERRES> posCierres = new PagedList<POS_CIERRES>();
			using (DobraConnection db = new DobraConnection())
			{
				posCierres.Results = db.POS_CIERRES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				posCierres.Total = posCierres.Results.Count;
				posCierres.Count = posCierres.Results.Count;
			}
			return posCierres;

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
			PagedList<VEN_FACTURAS> venFacturas = new PagedList<VEN_FACTURAS>();
			using (DobraConnection db = new DobraConnection())
			{

				venFacturas.Results = db.VEN_FACTURAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				venFacturas.Total = venFacturas.Results.Count;
				venFacturas.Count = venFacturas.Results.Count;
			}
			return venFacturas;

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
			PagedList<VEN_FACTURAS_DT> venFacturas = new PagedList<VEN_FACTURAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				venFacturas.Results = db.VEN_FACTURAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				venFacturas.Total = venFacturas.Results.Count;
				venFacturas.Count = venFacturas.Results.Count;
			}
			return venFacturas;
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
			PagedList<BAN_INGRESOS_DEUDAS> banIngresosDeudas = new PagedList<BAN_INGRESOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosDeudas.Results = db.BAN_INGRESOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosDeudas.Total = banIngresosDeudas.Results.Count;
				banIngresosDeudas.Count = banIngresosDeudas.Results.Count;
			}
			return banIngresosDeudas;

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
			PagedList<BAN_BANCOS_CARDEX> banBancoCardex = new PagedList<BAN_BANCOS_CARDEX>();
			using (DobraConnection db = new DobraConnection())
			{
				banBancoCardex.Results = db.BAN_BANCOS_CARDEX.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banBancoCardex.Total = banBancoCardex.Results.Count;
				banBancoCardex.Count = banBancoCardex.Results.Count;
			}
			return banBancoCardex;

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
			PagedList<BAN_DEPOSITOS> banDepositos = new PagedList<BAN_DEPOSITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDepositos.Results = db.BAN_DEPOSITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDepositos.Total = banDepositos.Results.Count;
				banDepositos.Count = banDepositos.Results.Count;
			}
			return banDepositos;

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
			PagedList<BAN_DEPOSITOS_DT> banDepositosDt = new PagedList<BAN_DEPOSITOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banDepositosDt.Results = db.BAN_DEPOSITOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDepositosDt.Total = banDepositosDt.Results.Count;
				banDepositosDt.Count = banDepositosDt.Results.Count;
			}
			return banDepositosDt;

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
			PagedList<BAN_DEPOSITOS_PAPELETAS> banDepositosDt = new PagedList<BAN_DEPOSITOS_PAPELETAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDepositosDt.Results = db.BAN_DEPOSITOS_PAPELETAS.AsNoTracking().Where(e => e.CreadoDAte >= lastUpdate).ToList();
				//banDepositosDt.Results = db.BAN_DEPOSITOS_PAPELETAS.AsNoTracking().Where(e => e.CreadoDate >= lastUpdate).ToList();
				banDepositosDt.Total = banDepositosDt.Results.Count;
				banDepositosDt.Count = banDepositosDt.Results.Count;
			}
			return banDepositosDt;

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
			PagedList<COM_FACTURAS> comFacturas = new PagedList<COM_FACTURAS>();
			using (DobraConnection db = new DobraConnection())
			{
				comFacturas.Results = db.COM_FACTURAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				comFacturas.Total = comFacturas.Results.Count;
				comFacturas.Count = comFacturas.Results.Count;
			}
			return comFacturas;

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
			PagedList<COM_FACTURAS_DT> comFacturasDt = new PagedList<COM_FACTURAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				comFacturasDt.Results = db.COM_FACTURAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				comFacturasDt.Total = comFacturasDt.Results.Count;
				comFacturasDt.Count = comFacturasDt.Results.Count;
			}
			return comFacturasDt;

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
			PagedList<COM_FACTURAS_PAGOS> comFacturasPagos = new PagedList<COM_FACTURAS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{
				comFacturasPagos.Results = db.COM_FACTURAS_PAGOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				comFacturasPagos.Total = comFacturasPagos.Results.Count;
				comFacturasPagos.Count = comFacturasPagos.Results.Count;
			}
			return comFacturasPagos;

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
			PagedList<ACR_RETENCIONES> acrRetenciones = new PagedList<ACR_RETENCIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRetenciones.Results = db.ACR_RETENCIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRetenciones.Total = acrRetenciones.Results.Count;
				acrRetenciones.Count = acrRetenciones.Results.Count;
			}
			return acrRetenciones;

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
			PagedList<ACR_RETENCIONES_DT> acrRetencionesDt = new PagedList<ACR_RETENCIONES_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRetencionesDt.Results = db.ACR_RETENCIONES_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRetencionesDt.Total = acrRetencionesDt.Results.Count;
				acrRetencionesDt.Count = acrRetencionesDt.Results.Count;
			}
			return acrRetencionesDt;

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
			PagedList<ACR_RETENCIONES_DEUDAS> acrRetencionesDeudas = new PagedList<ACR_RETENCIONES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRetencionesDeudas.Results = db.ACR_RETENCIONES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRetencionesDeudas.Total = acrRetencionesDeudas.Results.Count;
				acrRetencionesDeudas.Count = acrRetencionesDeudas.Results.Count;
			}
			return acrRetencionesDeudas;

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
			PagedList<ACR_ACREEDORES_DEUDAS> acrAcreedoresDeudas = new PagedList<ACR_ACREEDORES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrAcreedoresDeudas.Results = db.ACR_ACREEDORES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrAcreedoresDeudas.Total = acrAcreedoresDeudas.Results.Count;
				acrAcreedoresDeudas.Count = acrAcreedoresDeudas.Results.Count;
			}
			return acrAcreedoresDeudas;

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
			PagedList<PRV_FACTURAS> pvrFacturas = new PagedList<PRV_FACTURAS>();
			using (DobraConnection db = new DobraConnection())
			{
				pvrFacturas.Results = db.PRV_FACTURAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				pvrFacturas.Total = pvrFacturas.Results.Count;
				pvrFacturas.Count = pvrFacturas.Results.Count;
			}
			return pvrFacturas;

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
			PagedList<PRV_FACTURAS_DT> pvrFacturasDt = new PagedList<PRV_FACTURAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				pvrFacturasDt.Results = db.PRV_FACTURAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				pvrFacturasDt.Total = pvrFacturasDt.Results.Count;
				pvrFacturasDt.Count = pvrFacturasDt.Results.Count;
			}
			return pvrFacturasDt;

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
			PagedList<PRV_FACTURASCTA_DT> pvrFacturasCtaDt = new PagedList<PRV_FACTURASCTA_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				pvrFacturasCtaDt.Results = db.PRV_FACTURASCTA_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				pvrFacturasCtaDt.Total = pvrFacturasCtaDt.Results.Count;
				pvrFacturasCtaDt.Count = pvrFacturasCtaDt.Results.Count;
			}
			return pvrFacturasCtaDt;

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
			PagedList<EMP_ROLES> empRoles = new PagedList<EMP_ROLES>();
			using (DobraConnection db = new DobraConnection())
			{
				empRoles.Results = db.EMP_ROLES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empRoles.Total = empRoles.Results.Count;
				empRoles.Count = empRoles.Results.Count;
			}
			return empRoles;
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
			PagedList<EMP_ROLES_EMPLEADOS> empRolesEmpleados = new PagedList<EMP_ROLES_EMPLEADOS>();
			using (DobraConnection db = new DobraConnection())
			{
				empRolesEmpleados.Results = db.EMP_ROLES_EMPLEADOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empRolesEmpleados.Total = empRolesEmpleados.Results.Count;
				empRolesEmpleados.Count = empRolesEmpleados.Results.Count;
			}
			return empRolesEmpleados;

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
			PagedList<EMP_ROLES_RUBROS> empRolesRubros = new PagedList<EMP_ROLES_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				empRolesRubros.Results = db.EMP_ROLES_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empRolesRubros.Total = empRolesRubros.Results.Count;
				empRolesRubros.Count = empRolesRubros.Results.Count;
			}
			return empRolesRubros;

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
			PagedList<EMP_EMPLEADOS_DEUDAS> empEmpleadosDeudas = new PagedList<EMP_EMPLEADOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				empEmpleadosDeudas.Results = db.EMP_EMPLEADOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empEmpleadosDeudas.Total = empEmpleadosDeudas.Results.Count;
				empEmpleadosDeudas.Count = empEmpleadosDeudas.Results.Count;
			}
			return empEmpleadosDeudas;

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
			PagedList<EMP_EMPLEADOS_HORAS> empEmpleadosHoras = new PagedList<EMP_EMPLEADOS_HORAS>();
			using (DobraConnection db = new DobraConnection())
			{
				empEmpleadosHoras.Results = db.EMP_EMPLEADOS_HORAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empEmpleadosHoras.Total = empEmpleadosHoras.Results.Count;
				empEmpleadosHoras.Count = empEmpleadosHoras.Results.Count;
			}
			return empEmpleadosHoras;

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
			PagedList<EMP_DEBITOS> empDebitos = new PagedList<EMP_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				empDebitos.Results = db.EMP_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empDebitos.Total = empDebitos.Results.Count;
				empDebitos.Count = empDebitos.Results.Count;
			}
			return empDebitos;

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
			PagedList<EMP_DEBITOS_RUBROS> empDebitosRubros = new PagedList<EMP_DEBITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				empDebitosRubros.Results = db.EMP_DEBITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				empDebitosRubros.Total = empDebitosRubros.Results.Count;
				empDebitosRubros.Count = empDebitosRubros.Results.Count;
			}
			return empDebitosRubros;

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
			PagedList<CLI_GRUPOS> cliGrupos = new PagedList<CLI_GRUPOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliGrupos.Results = db.CLI_GRUPOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliGrupos.Total = cliGrupos.Results.Count;
				cliGrupos.Count = cliGrupos.Results.Count;
			}
			return cliGrupos;

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
			PagedList<INV_BODEGAS> invBodegas = new PagedList<INV_BODEGAS>();
			using (DobraConnection db = new DobraConnection())
			{
				invBodegas.Results = db.INV_BODEGAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invBodegas.Total = invBodegas.Results.Count;
				invBodegas.Count = invBodegas.Results.Count;
			}
			return invBodegas;

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
			PagedList<ACR_CREDITOS> empDebitosRubros = new PagedList<ACR_CREDITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				empDebitosRubros.Results = db.ACR_CREDITOS.AsNoTracking().Where(e => e.Fecha >= lastUpdate).ToList();

				empDebitosRubros.Total = empDebitosRubros.Results.Count;
				empDebitosRubros.Count = empDebitosRubros.Results.Count;
			}
			return empDebitosRubros;

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
			PagedList<ACR_CREDITOS_DEUDAS> acrCreditosDeudas = new PagedList<ACR_CREDITOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrCreditosDeudas.Results = db.ACR_CREDITOS_DEUDAS.AsNoTracking().Where(e => e.CreadoDate >= lastUpdate).ToList();

				acrCreditosDeudas.Total = acrCreditosDeudas.Results.Count;
				acrCreditosDeudas.Count = acrCreditosDeudas.Results.Count;
			}
			return acrCreditosDeudas;

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
			PagedList<ACR_CREDITOS_RUBROS> acrCreditosRubros = new PagedList<ACR_CREDITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrCreditosRubros.Results = db.ACR_CREDITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrCreditosRubros.Total = acrCreditosRubros.Results.Count;
				acrCreditosRubros.Count = acrCreditosRubros.Results.Count;
			}
			return acrCreditosRubros;

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
			PagedList<ACR_DEBITOS> acrDebitos = new PagedList<ACR_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitos.Results = db.ACR_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitos.Total = acrDebitos.Results.Count;
				acrDebitos.Count = acrDebitos.Results.Count;
			}
			return acrDebitos;

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
			PagedList<ACR_DEBITOS_DEUDAS> acrDebitosDeudas = new PagedList<ACR_DEBITOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitosDeudas.Results = db.ACR_DEBITOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitosDeudas.Total = acrDebitosDeudas.Results.Count;
				acrDebitosDeudas.Count = acrDebitosDeudas.Results.Count;
			}
			return acrDebitosDeudas;

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
			PagedList<ACR_DEBITOS_RUBROS> acrDebitosRubros = new PagedList<ACR_DEBITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitosRubros.Results = db.ACR_DEBITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitosRubros.Total = acrDebitosRubros.Results.Count;
				acrDebitosRubros.Count = acrDebitosRubros.Results.Count;
			}
			return acrDebitosRubros;

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
			PagedList<ACR_DEBITOS_PRODUCTOS> acrDebitosProductos = new PagedList<ACR_DEBITOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrDebitosProductos.Results = db.ACR_DEBITOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrDebitosProductos.Total = acrDebitosProductos.Results.Count;
				acrDebitosProductos.Count = acrDebitosProductos.Results.Count;
			}
			return acrDebitosProductos;

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
			PagedList<ACR_RECIBOS> acrRecibos = new PagedList<ACR_RECIBOS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRecibos.Results = db.ACR_RECIBOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRecibos.Total = acrRecibos.Results.Count;
				acrRecibos.Count = acrRecibos.Results.Count;
			}
			return acrRecibos;

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
			PagedList<ACR_RECIBOS_DT> acrRecibosDt = new PagedList<ACR_RECIBOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRecibosDt.Results = db.ACR_RECIBOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRecibosDt.Total = acrRecibosDt.Results.Count;
				acrRecibosDt.Count = acrRecibosDt.Results.Count;
			}
			return acrRecibosDt;

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
			PagedList<ACR_RECIBOS_DEUDAS> acrRecibosDeudas = new PagedList<ACR_RECIBOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				acrRecibosDeudas.Results = db.ACR_RECIBOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrRecibosDeudas.Total = acrRecibosDeudas.Results.Count;
				acrRecibosDeudas.Count = acrRecibosDeudas.Results.Count;
			}
			return acrRecibosDeudas;

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
			PagedList<BAN_DEBITOS> banDebitos = new PagedList<BAN_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDebitos.Results = db.BAN_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDebitos.Total = banDebitos.Results.Count;
				banDebitos.Count = banDebitos.Results.Count;
			}
			return banDebitos;

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
			PagedList<BAN_DEBITOS_CUENTAS> banDebitosCuentas = new PagedList<BAN_DEBITOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banDebitosCuentas.Results = db.BAN_DEBITOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banDebitosCuentas.Total = banDebitosCuentas.Results.Count;
				banDebitosCuentas.Count = banDebitosCuentas.Results.Count;
			}
			return banDebitosCuentas;

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
			PagedList<BAN_EGRESOS> banEgresos = new PagedList<BAN_EGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresos.Results = db.BAN_EGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresos.Total = banEgresos.Results.Count;
				banEgresos.Count = banEgresos.Results.Count;
			}
			return banEgresos;

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
			PagedList<BAN_EGRESOS_ANEXOS> banEgresosAnexos = new PagedList<BAN_EGRESOS_ANEXOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosAnexos.Results = db.BAN_EGRESOS_ANEXOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosAnexos.Total = banEgresosAnexos.Results.Count;
				banEgresosAnexos.Count = banEgresosAnexos.Results.Count;
			}
			return banEgresosAnexos;

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
			PagedList<BAN_EGRESOS_ANTICIPOS> banEgresosAnticipos = new PagedList<BAN_EGRESOS_ANTICIPOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosAnticipos.Results = db.BAN_EGRESOS_ANTICIPOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosAnticipos.Total = banEgresosAnticipos.Results.Count;
				banEgresosAnticipos.Count = banEgresosAnticipos.Results.Count;
			}
			return banEgresosAnticipos;

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
			PagedList<BAN_EGRESOS_CUENTAS> banEgresosCuentas = new PagedList<BAN_EGRESOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosCuentas.Results = db.BAN_EGRESOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosCuentas.Total = banEgresosCuentas.Results.Count;
				banEgresosCuentas.Count = banEgresosCuentas.Results.Count;
			}
			return banEgresosCuentas;

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
			PagedList<BAN_EGRESOS_DEUDAS> banEgresosDeudas = new PagedList<BAN_EGRESOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosDeudas.Results = db.BAN_EGRESOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosDeudas.Total = banEgresosDeudas.Results.Count;
				banEgresosDeudas.Count = banEgresosDeudas.Results.Count;
			}
			return banEgresosDeudas;

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
			PagedList<BAN_EGRESOS_DT> banEgresosDt = new PagedList<BAN_EGRESOS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosDt.Results = db.BAN_EGRESOS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banEgresosDt.Total = banEgresosDt.Results.Count;
				banEgresosDt.Count = banEgresosDt.Results.Count;
			}
			return banEgresosDt;

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
			PagedList<BAN_EGRESOS_PAGOS> banEgresosPagos = new PagedList<BAN_EGRESOS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{
				banEgresosPagos.Results = db.BAN_EGRESOS_PAGOS.AsNoTracking().Where(e => e.ExportadoDate > lastUpdate).ToList();

				banEgresosPagos.Total = banEgresosPagos.Results.Count;
				banEgresosPagos.Count = banEgresosPagos.Results.Count;
			}
			return banEgresosPagos;

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
			PagedList<BAN_INGRESOS_CUENTAS> banIngresosCuentas = new PagedList<BAN_INGRESOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosCuentas.Results = db.BAN_INGRESOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosCuentas.Total = banIngresosCuentas.Results.Count;
				banIngresosCuentas.Count = banIngresosCuentas.Results.Count;
			}
			return banIngresosCuentas;
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
			PagedList<BAN_INGRESOS_PINPAD> banIngresoPinPad = new PagedList<BAN_INGRESOS_PINPAD>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresoPinPad.Results = db.BAN_INGRESOS_PINPAD.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresoPinPad.Total = banIngresoPinPad.Results.Count;
				banIngresoPinPad.Count = banIngresoPinPad.Results.Count;
			}
			return banIngresoPinPad;
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
			PagedList<BAN_INGRESOS_TARJETAS> banIngresosTarjetas = new PagedList<BAN_INGRESOS_TARJETAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banIngresosTarjetas.Results = db.BAN_INGRESOS_TARJETAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banIngresosTarjetas.Total = banIngresosTarjetas.Results.Count;
				banIngresosTarjetas.Count = banIngresosTarjetas.Results.Count;
			}
			return banIngresosTarjetas;

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
			PagedList<BAN_PAPELETAS> banPapeletas = new PagedList<BAN_PAPELETAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banPapeletas.Results = db.BAN_PAPELETAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banPapeletas.Total = banPapeletas.Results.Count;
				banPapeletas.Count = banPapeletas.Results.Count;
			}
			return banPapeletas;

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
			PagedList<BAN_TRANSFERENCIAS> banTransferencias = new PagedList<BAN_TRANSFERENCIAS>();
			using (DobraConnection db = new DobraConnection())
			{
				banTransferencias.Results = db.BAN_TRANSFERENCIAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banTransferencias.Total = banTransferencias.Results.Count;
				banTransferencias.Count = banTransferencias.Results.Count;
			}
			return banTransferencias;

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
			PagedList<BAN_TRANSFERENCIAS_DT> banTransferenciasDt = new PagedList<BAN_TRANSFERENCIAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				banTransferenciasDt.Results = db.BAN_TRANSFERENCIAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banTransferenciasDt.Total = banTransferenciasDt.Results.Count;
				banTransferenciasDt.Count = banTransferenciasDt.Results.Count;
			}
			return banTransferenciasDt;

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
			PagedList<VEN_FACTURAS_PAGOS> venFacturasPagos = new PagedList<VEN_FACTURAS_PAGOS>();
			using (DobraConnection db = new DobraConnection())
			{
				venFacturasPagos.Results = db.VEN_FACTURAS_PAGOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				venFacturasPagos.Total = venFacturasPagos.Results.Count;
				venFacturasPagos.Count = venFacturasPagos.Results.Count;
			}
			return venFacturasPagos;

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
			PagedList<INV_EGRESOS> invEgresos = new PagedList<INV_EGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invEgresos.Results = db.INV_EGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invEgresos.Total = invEgresos.Results.Count;
				invEgresos.Count = invEgresos.Results.Count;
			}
			return invEgresos;

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
			PagedList<INV_EGRESOS_RUBROS> invEgresosRubros = new PagedList<INV_EGRESOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				invEgresosRubros.Results = db.INV_EGRESOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invEgresosRubros.Total = invEgresosRubros.Results.Count;
				invEgresosRubros.Count = invEgresosRubros.Results.Count;
			}
			return invEgresosRubros;

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
			PagedList<INV_EGRESOS_PRODUCTOS> invEgresosProductos = new PagedList<INV_EGRESOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invEgresosProductos.Results = db.INV_EGRESOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invEgresosProductos.Total = invEgresosProductos.Results.Count;
				invEgresosProductos.Count = invEgresosProductos.Results.Count;
			}
			return invEgresosProductos;

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
			PagedList<ModelDobraDatabase.INV_INGRESOS> invIngresos = new PagedList<ModelDobraDatabase.INV_INGRESOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invIngresos.Results = db.INV_INGRESOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invIngresos.Total = invIngresos.Results.Count;
				invIngresos.Count = invIngresos.Results.Count;
			}
			return invIngresos;

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
			PagedList<INV_INGRESOS_RUBROS> invIngresosRubros = new PagedList<INV_INGRESOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				invIngresosRubros.Results = db.INV_INGRESOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invIngresosRubros.Total = invIngresosRubros.Results.Count;
				invIngresosRubros.Count = invIngresosRubros.Results.Count;
			}
			return invIngresosRubros;

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
			PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> invIngresosProductos = new PagedList<ModelDobraDatabase.INV_INGRESOS_PRODUCTOS>();
			using (DobraConnection db = new DobraConnection())
			{
				invIngresosProductos.Results = db.INV_INGRESOS_PRODUCTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invIngresosProductos.Total = invIngresosProductos.Results.Count;
				invIngresosProductos.Count = invIngresosProductos.Results.Count;
			}
			return invIngresosProductos;

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
			PagedList<INV_PROMOCIONES> invPromociones = new PagedList<INV_PROMOCIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				invPromociones.Results = db.INV_PROMOCIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				invPromociones.Total = invPromociones.Results.Count;
				invPromociones.Count = invPromociones.Results.Count;
			}
			return invPromociones;

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
			PagedList<INV_PROMOCIONES_DT> invPromocionesDt = new PagedList<INV_PROMOCIONES_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				invPromocionesDt.Results = db.INV_PROMOCIONES_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invPromocionesDt.Total = invPromocionesDt.Results.Count;
				invPromocionesDt.Count = invPromocionesDt.Results.Count;
			}
			return invPromocionesDt;

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
			PagedList<INV_PROMOCIONES_DT2> invPromocionesDt2 = new PagedList<INV_PROMOCIONES_DT2>();
			using (DobraConnection db = new DobraConnection())
			{
				invPromocionesDt2.Results = db.INV_PROMOCIONES_DT2.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invPromocionesDt2.Total = invPromocionesDt2.Results.Count;
				invPromocionesDt2.Count = invPromocionesDt2.Results.Count;
			}
			return invPromocionesDt2;

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
			PagedList<INV_TRANSFERENCIAS> invTransferencias = new PagedList<INV_TRANSFERENCIAS>();
			using (DobraConnection db = new DobraConnection())
			{
				invTransferencias.Results = db.INV_TRANSFERENCIAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				invTransferencias.Total = invTransferencias.Results.Count;
				invTransferencias.Count = invTransferencias.Results.Count;
			}
			return invTransferencias;

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
			PagedList<INV_TRANSFERENCIAS_DT> invTransferenciasDt = new PagedList<INV_TRANSFERENCIAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				invTransferenciasDt.Results = db.INV_TRANSFERENCIAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2)).ToList();

				invTransferenciasDt.Total = invTransferenciasDt.Results.Count;
				invTransferenciasDt.Count = invTransferenciasDt.Results.Count;
			}
			return invTransferenciasDt;

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
			PagedList<POS_TRANSFERENCIAS> posTransferencias = new PagedList<POS_TRANSFERENCIAS>();
			using (DobraConnection db = new DobraConnection())
			{
				posTransferencias.Results = db.POS_TRANSFERENCIAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				posTransferencias.Total = posTransferencias.Results.Count;
				posTransferencias.Count = posTransferencias.Results.Count;
			}
			return posTransferencias;

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
			PagedList<POS_TRANSFERENCIAS_DT> posTransferenciasDt = new PagedList<POS_TRANSFERENCIAS_DT>();
			using (DobraConnection db = new DobraConnection())
			{
				posTransferenciasDt.Results = db.POS_TRANSFERENCIAS_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();
				posTransferenciasDt.Total = posTransferenciasDt.Results.Count;
				posTransferenciasDt.Count = posTransferenciasDt.Results.Count;
			}
			return posTransferenciasDt;

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
			PagedList<CLI_CREDITOS_DEUDAS> cliCreditosDeudas = new PagedList<CLI_CREDITOS_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditosDeudas.Results = db.CLI_CREDITOS_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliCreditosDeudas.Total = cliCreditosDeudas.Results.Count;
				cliCreditosDeudas.Count = cliCreditosDeudas.Results.Count;
			}
			return cliCreditosDeudas;

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
			PagedList<CLI_CREDITOS_RUBROS> cliCreditosRubros = new PagedList<CLI_CREDITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliCreditosRubros.Results = db.CLI_CREDITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliCreditosRubros.Total = cliCreditosRubros.Results.Count;
				cliCreditosRubros.Count = cliCreditosRubros.Results.Count;
			}
			return cliCreditosRubros;

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
			PagedList<CLI_DEBITOS> cliDebitos = new PagedList<CLI_DEBITOS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliDebitos.Results = db.CLI_DEBITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliDebitos.Total = cliDebitos.Results.Count;
				cliDebitos.Count = cliDebitos.Results.Count;
			}
			return cliDebitos;

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
			PagedList<CLI_DEBITOS_RUBROS> cliDebitosRubros = new PagedList<CLI_DEBITOS_RUBROS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliDebitosRubros.Results = db.CLI_DEBITOS_RUBROS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliDebitosRubros.Total = cliDebitosRubros.Results.Count;
				cliDebitosRubros.Count = cliDebitosRubros.Results.Count;
			}
			return cliDebitosRubros;

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
			PagedList<CLI_RETENCIONES> cliRetenciones = new PagedList<CLI_RETENCIONES>();
			using (DobraConnection db = new DobraConnection())
			{
				cliRetenciones.Results = db.CLI_RETENCIONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliRetenciones.Total = cliRetenciones.Results.Count;
				cliRetenciones.Count = cliRetenciones.Results.Count;
			}
			return cliRetenciones;

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
			PagedList<CLI_RETENCIONES_DEUDAS> cliRetencionesDeudas = new PagedList<CLI_RETENCIONES_DEUDAS>();
			using (DobraConnection db = new DobraConnection())
			{
				cliRetencionesDeudas.Results = db.CLI_RETENCIONES_DEUDAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliRetencionesDeudas.Total = cliRetencionesDeudas.Results.Count;
				cliRetencionesDeudas.Count = cliRetencionesDeudas.Results.Count;
			}
			return cliRetencionesDeudas;

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
			PagedList<CLI_RETENCIONES_DT> cliRetencionesDt = new PagedList<CLI_RETENCIONES_DT>();
			using (DobraConnection db = new DobraConnection())
			{

				cliRetencionesDt.Results = db.CLI_RETENCIONES_DT.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				cliRetencionesDt.Total = cliRetencionesDt.Results.Count;
				cliRetencionesDt.Count = cliRetencionesDt.Results.Count;
			}
			return cliRetencionesDt;

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
			PagedList<ACR_ACREEDORES> acrAcreedores = new PagedList<ACR_ACREEDORES>();
			using (DobraConnection db = new DobraConnection())
			{

				acrAcreedores.Results = db.ACR_ACREEDORES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				acrAcreedores.Total = acrAcreedores.Results.Count;
				acrAcreedores.Count = acrAcreedores.Results.Count;
			}
			return acrAcreedores;

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
			PagedList<INV_GRUPOS> invGrupos = new PagedList<INV_GRUPOS>();
			using (DobraConnection db = new DobraConnection())
			{

				invGrupos.Results = db.INV_GRUPOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				invGrupos.Total = invGrupos.Results.Count;
				invGrupos.Count = invGrupos.Results.Count;
			}
			return invGrupos;

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
			PagedList<BAN_CREDITOS> banCreditos = new PagedList<BAN_CREDITOS>();
			using (DobraConnection db = new DobraConnection())
			{

				banCreditos.Results = db.BAN_CREDITOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banCreditos.Total = banCreditos.Results.Count;
				banCreditos.Count = banCreditos.Results.Count;
			}
			return banCreditos;

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
			PagedList<BAN_CREDITOS_CUENTAS> banCreditosCuentas = new PagedList<BAN_CREDITOS_CUENTAS>();
			using (DobraConnection db = new DobraConnection())
			{

				banCreditosCuentas.Results = db.BAN_CREDITOS_CUENTAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				banCreditosCuentas.Total = banCreditosCuentas.Results.Count;
				banCreditosCuentas.Count = banCreditosCuentas.Results.Count;
			}
			return banCreditosCuentas;

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
			PagedList<ORG_BUZONES> orgBuzones = new PagedList<ORG_BUZONES>();
			using (DobraConnection db = new DobraConnection())
			{

				orgBuzones.Results = db.ORG_BUZONES.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				orgBuzones.Total = orgBuzones.Results.Count;
				orgBuzones.Count = orgBuzones.Results.Count;
			}
			return orgBuzones;

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
			PagedList<ORG_DOCUMENTOS> orgDocumentos = new PagedList<ORG_DOCUMENTOS>();
			using (DobraConnection db = new DobraConnection())
			{

				orgDocumentos.Results = db.ORG_DOCUMENTOS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				orgDocumentos.Total = orgDocumentos.Results.Count;
				orgDocumentos.Count = orgDocumentos.Results.Count;
			}
			return orgDocumentos;

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
			PagedList<ORG_TAREAS> orgTareas = new PagedList<ORG_TAREAS>();
			using (DobraConnection db = new DobraConnection())
			{

				orgTareas.Results = db.ORG_TAREAS.AsNoTracking().Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2)).ToList();

				orgTareas.Total = orgTareas.Results.Count;
				orgTareas.Count = orgTareas.Results.Count;
			}
			return orgTareas;

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
