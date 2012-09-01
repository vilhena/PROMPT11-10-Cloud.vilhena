using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;
using Ana.Web.Models;

namespace Ana.Contracts.Business
{
    public interface IBoardManager : IManager
    {

        BoardModel GetBoard(string id);
        BoardCreateOrEditModel GetBoardForEdit(string id);

        BoardModel CreateNewBoard(BoardCreateOrEditModel newBoard);
        void UpdateBoard(BoardCreateOrEditModel board);
        void DeleteBoard(string id);

        void ShareBoardWithUser(string id, string userId);
        void RemoveUserShare(string id, string userId);
        ShareBoardModel GetBoardShares(string id);

        IEnumerable<BoardModel> GetMyBoards();
        IEnumerable<BoardResumeModel> GetMySharedBoards();
    }
}
