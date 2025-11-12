using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
	public class BusinessException : Exception
	{
		/// <summary>
		/// Excepcion controlada
		/// </summary>
		/// <param name="message"></param>
		public BusinessException(string message) : base(message) { }
	}
}
