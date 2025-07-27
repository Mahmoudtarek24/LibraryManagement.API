using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Constants.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum UserOrdering
	{
		CreateOn,
		Email,
		Id
	}
}
