using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models {
public class UserMessage {

public string Name { get; set; }
public string Message { get; set; }
public override string ToString() {
return $"{Name}: {Message}";
}

}

}
