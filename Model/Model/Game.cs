using System;
using System.IO;
using System.Threading.Tasks;
using Model.Persistence;

namespace Model.Model
{
    public class Game
    {
        public Game(Board _board, NoticeBoard _noticeBoard, MyDataAccess _dataAccess) { 
            this._board = _board; 
            this._noticeBoard = _noticeBoard;
            this._dataAccess = _dataAccess;
        }
        private Board _board;
        public Board board { get { return _board; } }
        private NoticeBoard _noticeBoard;
        public NoticeBoard noticeBoard { get { return _noticeBoard; } }
        private int turns;
        public int Turns { get { return turns; } set { turns = value; } }
        private int gameTime;
        public int GameTime { get { return gameTime; } }
        private IDataAccess _dataAccess;
        private string _filepath;
        public void AdvanceTime() {/*code*/ ; }
        public event EventHandler<GameEventArgs> GameAdvanced;
        public void OnGameAdvanced() {/*code*/ ; }
        public bool isGameOver { get { return false; } }
        public event EventHandler<GameEventArgs> GameOver;
        private void OnGameOver(bool end, Team team) {/*code*/ ; }
        public async Task NewGame(Stream _filepath) {/*code*/ ; }
        public async Task LoadGameAsync(string _filepath) {/*code*/ ; }
        public async Task SaveGameAsync(string _filepath) {/*code*/ ; }

        public void Step(int x, int v)
        {
            throw new NotImplementedException();
        }
    }
    
    public class GameEventArgs : EventArgs
    {
        public GameEventArgs(Boolean isGameOver, int winnerTeam) { _isGameOver = isGameOver; _winnerTeam = winnerTeam; }
        private Boolean _isGameOver;
        private int _winnerTeam;
        public Boolean IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }
        public int WinnerTeam { get { return _winnerTeam; } set { _winnerTeam = value; } }
    }
    

}
