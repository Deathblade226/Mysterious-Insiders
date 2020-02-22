using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mysterious_Insiders.Models {
public interface IMessageDAL {

public void AddMessage(UserMessage message);
public void RemoveMessage(UserMessage message);
public IEnumerable<UserMessage> GetMessages();

}

}
