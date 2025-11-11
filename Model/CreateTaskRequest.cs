using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class CreateTaskRequest
	{
		[Required(ErrorMessage = "El título es requerido")]
		[StringLength(255, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 255 caracteres")]
		public string Title { get; set; } = string.Empty;

		[StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
		public string? Description { get; set; }
	}
}
