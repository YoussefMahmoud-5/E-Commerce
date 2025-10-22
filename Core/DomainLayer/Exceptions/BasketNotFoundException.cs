using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class BasketNotFoundException(string key) : NotFoundException($"Basket With Keu {key} is Not Found")
    {
    }
}
