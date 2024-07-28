using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BoardCommander.Exceptions
{
    public class ColorNotFoundException : BaseApiException
    {
        public static void ThrowNewException(string color)
            => throw new ColorNotFoundException(color);
        public ColorNotFoundException(string color):base(400, $"Color {color} not found. Only R,G,Y,B,C,P,W are accepted.")
        {

        }
    }
}
