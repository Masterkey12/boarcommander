using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BoardCommander.Exceptions
{
    public class BoardNotFoundException : BaseApiException
    {
        public static void ThrowNewException(string boardId)
            => throw new BoardNotFoundException(boardId);
        public BoardNotFoundException(string boardID):base(400, $"Board {boardID} Not Found")
        {

        }
    }
}
