using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
	public class ErrorSave
	{
		public String Tabla { get; set; }
		public String errorMessage { get; set; }

		public List<String> Listid { get; set; }

		public bool errorExit { get; set; }

	
		public List<ErrorClienteCedula> errorClienteCedulas { get; set; }
		public ErrorSave()
		{
			errorClienteCedulas = new List<ErrorClienteCedula>();
			errorExit = false;
		}
	}
}
