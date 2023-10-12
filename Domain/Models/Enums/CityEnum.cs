using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Enums {
	public enum CityEnum {
        [EnumMember(Value = "Breda")]
        Breda,
        [EnumMember(Value = "DenBosch")]
        DenBosch,
        [EnumMember(Value = "Tilburg")]
        Tilburg,
        [EnumMember(Value = "Unknown")]
        Unknown
	}
}
