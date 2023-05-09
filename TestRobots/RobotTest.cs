using System;
using System.Threading.Tasks;
using Model.Model;
using Model.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection.Metadata;

namespace TestRobots
{
    [TestClass]
    public class RobotTest
    {

        private Game _model = null!; // a tesztelendõ modell
        private Board _mockedTable = null!; // mockolt játéktábla
        private Mock<IDataAccess> _mock = null!; // az adatelérés mock-ja
        private Team _team1 = null!;
        private Team _team2 = null!;
        private Robot _robot = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new Board(10, 12);

            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 10; j++)
                {
                    _mockedTable.SetValue(j, i, new Empty(j, i));
                }

            for (int i = 0; i < 10; i++)
            {
                _mockedTable.SetValue(i, 0, new Obstacle(i, 0, 1000));
                _mockedTable.SetValue(i, _mockedTable.Height - 1, new Obstacle(i, _mockedTable.Height - 1, 1000));
            }

            for (int i = 0; i < _mockedTable.Height; i++)
            {
                _mockedTable.SetValue(0, i, new Obstacle(0, i, 1000));
                _mockedTable.SetValue(_mockedTable.Width - 1, i, new Obstacle(_mockedTable.Width - 1, i, 1000));
            }

            _mockedTable.SetValue(0, 0, new Exit(0, 0));
            _mockedTable.SetValue(1, 0, new Exit(1, 0));
            _mockedTable.SetValue(2, 0, new Exit(2, 0));
            _mockedTable.SetValue(9, 4, new Exit(9, 4));
            _mockedTable.SetValue(0, 10, new Exit(0, 10));
            _mockedTable.SetValue(9, 10, new Exit(9, 10));
            _mockedTable.SetValue(8, 11, new Exit(8, 11));
            _mockedTable.SetValue(9, 11, new Exit(9, 11));

            _mockedTable.SetValue(1, 1, new Obstacle(1, 1, 2));
            _mockedTable.SetValue(3, 1, new Obstacle(3, 1, 3));
            _mockedTable.SetValue(5, 2, new Obstacle(5, 2, 3));
            _mockedTable.SetValue(5, 4, new Obstacle(5, 4, 5));
            _mockedTable.SetValue(2, 9, new Obstacle(2, 9, 4));
            _mockedTable.SetValue(3, 9, new Obstacle(3, 9, 3));
            _mockedTable.SetValue(3, 10, new Obstacle(3, 10, 2));
            _mockedTable.SetValue(7, 10, new Obstacle(7, 10, 4));
            _mockedTable.SetValue(8, 7, new Obstacle(8, 7, 3));
            _mockedTable.SetValue(7, 6, new Obstacle(7, 6, 3));

            _mockedTable.SetValue(1, 3, new Cube(1, 3, 5, Color.YELLOW));
            _mockedTable.SetValue(5, 1, new Cube(5, 1, 5, Color.YELLOW));
            _mockedTable.SetValue(8, 8, new Cube(8, 8, 5, Color.YELLOW));

            _mockedTable.SetValue(4, 1, new Cube(4, 1, 5, Color.GREEN));
            _mockedTable.SetValue(4, 6, new Cube(4, 6, 5, Color.GREEN));
            _mockedTable.SetValue(5, 7, new Cube(5, 7, 5, Color.GREEN));

            _mockedTable.SetValue(6, 1, new Cube(6, 1, 5, Color.BLUE));
            _mockedTable.SetValue(7, 1, new Cube(7, 1, 5, Color.BLUE));
            _mockedTable.SetValue(5, 8, new Cube(5, 8, 5, Color.BLUE));

            _mockedTable.SetValue(6, 2, new Cube(6, 2, 5, Color.ORANGE));
            _mockedTable.SetValue(2, 7, new Cube(2, 7, 5, Color.ORANGE));

            _mockedTable.SetValue(8, 2, new Cube(8, 2, 5, Color.GRAY));
            _mockedTable.SetValue(3, 6, new Cube(3, 6, 5, Color.GRAY));

            _mockedTable.SetValue(1, 4, new Cube(1, 4, 5, Color.PINK));
            _mockedTable.SetValue(1, 9, new Cube(1, 9, 5, Color.PINK));

            _mockedTable.SetValue(6, 5, new Cube(6, 5, 5, Color.RED));
            _mockedTable.SetValue(6, 8, new Cube(6, 8, 5, Color.RED));
            _mockedTable.SetValue(2, 10, new Cube(2, 10, 5, Color.RED));

            _mockedTable.SetValue(1, 6, new Cube(1, 6, 5, Color.PURPLE));
            _mockedTable.SetValue(3, 8, new Cube(3, 8, 5, Color.PURPLE));

            Robot _robot1 = new Robot(1, 2, Direction.WEST,0);
            Robot _robot2 = new Robot(2, 2, Direction.WEST,1);
            Robot _robot3 = new Robot(2, 1, Direction.SOUTH,2);
            Robot _robot4 = new Robot(4, 3, Direction.SOUTH,3);
            Robot _robot5 = new Robot(6, 3, Direction.WEST,4);
            Robot _robot6 = new Robot(6, 6, Direction.EAST,5);
            Robot _robot7 = new Robot(6, 7, Direction.WEST,6);
            Robot _robot8 = new Robot(4, 8, Direction.EAST,7);

            Robot[] _robots1 = { _robot1, _robot2, _robot3, _robot4 };
            Robot[] _robots2 = { _robot5, _robot6, _robot7, _robot8 };

            _team1 = new Team(_robots1, 4, 0);
            _team2 = new Team(_robots2, 4, 1);
            _robot = _robot1;


            _mockedTable.SetValue(1, 2, _robot1);
            _mockedTable.SetValue(2, 2, _robot2);
            _mockedTable.SetValue(2, 1, _robot3);
            _mockedTable.SetValue(4, 3, _robot4);
            _mockedTable.SetValue(6, 3, _robot5);
            _mockedTable.SetValue(6, 6, _robot6);
            _mockedTable.SetValue(6, 7, _robot7);
            _mockedTable.SetValue(4, 8, _robot8);
            // elõre definiálunk egy játéktáblát a perzisztencia mockolt teszteléséhez

            _mock = new Mock<IDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => _mockedTable);
            // a mock a LoadAsync mûveletben bármilyen paraméterre az elõre beállított játéktáblát fogja visszaadni

            _model = new Game(_mock.Object);
            // példányosítjuk a modellt a mock objektummal

            //_model.GameAdvanced += new EventHandler<MaciLaciEventArgs>(Model_GameAdvanced);
            //_model.GameOver += new EventHandler<MaciLaciEventArgs>(Model_GameOver);
        }


        [TestMethod]
        public void RobotsNewGameTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;

            Int32 emptyFields = 0;
            for (Int32 i = 0; i < 12; i++)
                for (Int32 j = 0; j < 10; j++)
                    if (_model.Board.GetFieldValue(j, i) is Empty)
                        emptyFields++;

            Assert.AreEqual(42, emptyFields); // szabad mezõk száma is megfelelõ
            Assert.AreEqual(30, _model.GameTime);
            Assert.AreEqual(1, _model.Round);
            Assert.AreEqual(0, _model.Team1Points);
            Assert.AreEqual(0, _model.Team2Points);
            Assert.AreEqual(1, _model.NextPlayerFromTeam1);
            Assert.AreEqual(0, _model.NextPlayerFromTeam2);
            Assert.IsTrue(_model.NextTeam1);

        }



        [TestMethod]
        public void GameMoveRightTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(1);


            Assert.AreEqual(2, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.EAST); // jobbra léptünk

            Assert.AreEqual(3, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(3, 2).GetType());
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(2, 2).GetType());

        }

        [TestMethod]
        public void GameMoveLeftTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(3);


            Assert.AreEqual(4, _model.Robot.X);
            Assert.AreEqual(3, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.WEST); // balra léptünk

            Assert.AreEqual(3, _model.Robot.X);
            Assert.AreEqual(3, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(3, 3).GetType());
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(4, 3).GetType());

        }

        [TestMethod]
        public void GameMoveDownTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(3);


            Assert.AreEqual(4, _model.Robot.X);
            Assert.AreEqual(3, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.SOUTH); // lefele léptünk

            Assert.AreEqual(4, _model.Robot.X);
            Assert.AreEqual(4, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(4, 4).GetType());
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(4, 3).GetType());

        }

        [TestMethod]
        public void GameMoveUpTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(3);


            Assert.AreEqual(4, _model.Robot.X);
            Assert.AreEqual(3, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.NORTH); // felfele léptünk

            Assert.AreEqual(4, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(4, 2).GetType());
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(4, 3).GetType());

        }

        [TestMethod]
        public void GameMoveFailedBecauseOfCubeTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(1, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.SOUTH); // kockára léptünk

            Assert.AreEqual(1, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(1, 2).GetType());
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());

        }

        [TestMethod]
        public void GameMoveFailedBecauseOfObstacleTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(1, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.WEST); // akadályra léptünk

            Assert.AreEqual(1, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(1, 2).GetType());
            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(0, 2).GetType());

        }

        [TestMethod]
        public void GameMoveFailedBecauseOfAnotherRobotTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(1, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.EAST); // másik robotra léptünk

            Assert.AreEqual(1, _model.Robot.X);
            Assert.AreEqual(2, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(1, 2).GetType());
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());

        }

        [TestMethod]
        public void GameMoveToExit()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(2);


            Assert.AreEqual(2, _model.Robot.X);
            Assert.AreEqual(1, _model.Robot.Y);

            _model.MoveRobot(_model.Robot, Direction.NORTH); // másik robotra léptünk

            Assert.AreEqual(2, _model.Robot.X);
            Assert.AreEqual(0, _model.Robot.Y);
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 0).GetType());
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(2, 1).GetType());

        }

        [TestMethod]
        public void GameRotateClockwiseTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.RotateRobot(_model.Robot, Angle.Clockwise); // forgás órával megegyezõ irányba

            Assert.AreEqual(Direction.NORTH, _model.Robot.Direction);

        }

        [TestMethod]
        public void GameRotateCounterClockwiseTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.RotateRobot(_model.Robot, Angle.CounterClockwise); // forgás órával ellentétes irányba

            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);

        }

        [TestMethod]
        public void GameRotateCounterClockwiseCubeTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(3);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(5, 8).GetType());
            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot
            _model.RotateRobot(_model.Robot, Angle.CounterClockwise); // forgás órával ellentétes irányba

            Assert.AreEqual(Direction.NORTH, _model.Robot.Direction);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(4, 7).GetType());
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(5, 8).GetType());

        }

        [TestMethod]
        public void GameRotateClockwiseCubeTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(3);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(3, 8).GetType());
            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);

            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);
            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot
            _model.RotateRobot(_model.Robot, Angle.Clockwise); // forgás órával megegyezõ irányba

            Assert.AreEqual(Direction.NORTH, _model.Robot.Direction);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(4, 7).GetType());
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(3, 8).GetType());

        }


        [TestMethod]
        public void GameRotateFailedClockwiseCubeTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.RotateRobot(_model.Robot, Angle.CounterClockwise);
            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot
            _model.RotateRobot(_model.Robot, Angle.Clockwise); // forgás órával ellentétes irányba

            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());
            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(0, 2).GetType());

        }

        [TestMethod]
        public void GameRotateFailedCounterClockwiseCubeTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.RotateRobot(_model.Robot, Angle.CounterClockwise);
            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot
            _model.RotateRobot(_model.Robot, Angle.CounterClockwise); // forgás órával ellentétes irányba

            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());

        }

        [TestMethod]
        public void GameConnectRobotRightTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(3);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(5, 8).GetType());
            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(5, 8).GetType());

        }

        [TestMethod]
        public void GameConnectRobotLeftTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(3);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(3, 8).GetType());

            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(3, 8).GetType());

        }

        [TestMethod]
        public void GameConnectRobotDownTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());

            _model.RotateRobot(_model.Robot, Angle.CounterClockwise);
            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());

        }

        [TestMethod]
        public void GameConnectRobotUpTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            Assert.AreEqual(Direction.NORTH, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

        }

        [TestMethod]
        public void GameConnectFailedToEmptyFieldTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(5, 3).GetType());
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(5, 3).GetType());

        }

        [TestMethod]
        public void GameConnectFailedToObstacleFieldTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(1);


            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(7, 6).GetType());
            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(7, 6).GetType());

        }

        [TestMethod]
        public void GameDisconnectRobotRightTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(3);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(5, 8).GetType());
            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(5, 8).GetType());

            _model.DisconnectRobot(_model.Robot); // lecsatoljuk a kockáról a robotot
            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(5, 8).GetType());
        }

        [TestMethod]
        public void GameDisconnectRobotLeftTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(3);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(3, 8).GetType());

            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(3, 8).GetType());

            _model.DisconnectRobot(_model.Robot); // lecsatoljuk a kockáról a robotot

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(3, 8).GetType());

        }

        [TestMethod]
        public void GameDisconnectRobotDownTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());

            _model.RotateRobot(_model.Robot, Angle.CounterClockwise);
            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());

            _model.DisconnectRobot(_model.Robot); // lecsatoljuk a kockáról a robotot

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());

        }

        [TestMethod]
        public void GameDisconnectRobotUpTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            Assert.AreEqual(Direction.NORTH, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot); // hozzácsatoljuk a kockához a robotot

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

            _model.DisconnectRobot(_model.Robot); // lecsatoljuk a kockáról a robotot

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

        }

        [TestMethod]
        public void GameDisconnectRobotFromNothingTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

            _model.DisconnectRobot(_model.Robot); // lecsatoljuk a kockáról a robotot

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(6, 2).GetType());

        }

        [TestMethod]
        public void GameCleanRightTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(1);


            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(7, 6).GetType());
            Assert.AreEqual(3, ((Obstacle)_model.Board.GetFieldValue(7, 6)).Health);
            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);


            _model.Clean(_model.Robot); // akadályt tisztitunk jobbra

            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(7, 6).GetType());
            Assert.AreEqual(2, ((Obstacle)_model.Board.GetFieldValue(7, 6)).Health);

        }

        [TestMethod]
        public void GameCleanLeftTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 4).GetType());
            Assert.AreEqual(5, ((Obstacle)_model.Board.GetFieldValue(5, 4)).Health);

            _model.MoveRobot(_model.Robot, Direction.SOUTH);
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);


            _model.Clean(_model.Robot); // akadályt tisztitunk balra

            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 4).GetType());
            Assert.AreEqual(4, ((Obstacle)_model.Board.GetFieldValue(5, 4)).Health);

        }

        [TestMethod]
        public void GameCleanDownTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 4).GetType());
            Assert.AreEqual(5, ((Obstacle)_model.Board.GetFieldValue(5, 4)).Health);

            _model.MoveRobot(_model.Robot, Direction.WEST);
            _model.RotateRobot(_model.Robot, Angle.CounterClockwise);
            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);


            _model.Clean(_model.Robot); // akadályt tisztitunk lefele

            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 4).GetType());
            Assert.AreEqual(4, ((Obstacle)_model.Board.GetFieldValue(5, 4)).Health);

        }

        [TestMethod]
        public void GameCleanUpTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 2).GetType());
            Assert.AreEqual(3, ((Obstacle)_model.Board.GetFieldValue(5, 2)).Health);

            _model.MoveRobot(_model.Robot, Direction.WEST);
            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            Assert.AreEqual(Direction.NORTH, _model.Robot.Direction);


            _model.Clean(_model.Robot); // akadályt tisztitunk felfele

            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 2).GetType());
            Assert.AreEqual(2, ((Obstacle)_model.Board.GetFieldValue(5, 2)).Health);

        }

        [TestMethod]
        public void GameCleanEmptyFieldTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);

            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(5, 3).GetType());
            Assert.AreEqual(Direction.WEST, _model.Robot.Direction);

            _model.Clean(_model.Robot); // akadályt tisztitunk felfele

            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(5, 3).GetType());
        }

        [TestMethod]
        public void GameCleanUntilObstacleBecomesEmptyTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team2.GetRobot(0);


            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 2).GetType());
            Assert.AreEqual(3, ((Obstacle)_model.Board.GetFieldValue(5, 2)).Health);

            _model.MoveRobot(_model.Robot, Direction.WEST);
            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            Assert.AreEqual(Direction.NORTH, _model.Robot.Direction);


            _model.Clean(_model.Robot); // akadályt tisztitunk felfele

            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 2).GetType());
            Assert.AreEqual(2, ((Obstacle)_model.Board.GetFieldValue(5, 2)).Health);

            _model.Clean(_model.Robot); // akadályt tisztitunk felfele

            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(5, 2).GetType());
            Assert.AreEqual(1, ((Obstacle)_model.Board.GetFieldValue(5, 2)).Health);

            _model.Clean(_model.Robot); // akadályt tisztitunk felfele

            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(5, 2).GetType());
        }

        [TestMethod]
        public void GameCleanRobotTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());
            Assert.AreEqual(3, ((Robot)_model.Board.GetFieldValue(2, 2)).Health);

            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            _model.RotateRobot(_model.Robot, Angle.Clockwise);

            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);


            _model.Clean(_model.Robot); // akadályt tisztitunk jobbra

            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());
            Assert.AreEqual(2, ((Robot)_model.Board.GetFieldValue(2, 2)).Health);

            _model.Clean(_model.Robot); // akadályt tisztitunk jobbra

            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());
            Assert.AreEqual(1, ((Robot)_model.Board.GetFieldValue(2, 2)).Health);

        }

        [TestMethod]
        public void GameCleanRobotUntilNoHealthTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());
            Assert.AreEqual(3, ((Robot)_model.Board.GetFieldValue(2, 2)).Health);

            _model.RotateRobot(_model.Robot, Angle.Clockwise);
            _model.RotateRobot(_model.Robot, Angle.Clockwise);

            Assert.AreEqual(Direction.EAST, _model.Robot.Direction);


            _model.Clean(_model.Robot); // akadályt tisztitunk jobbra
            _model.Clean(_model.Robot); // akadályt tisztitunk jobbra
            _model.Clean(_model.Robot); // akadályt tisztitunk jobbra

            Assert.AreEqual(typeof(Empty), _model.Board.GetFieldValue(2, 2).GetType());

            Assert.AreEqual(3, _model.Team1.Robots.Length);

        }

        [TestMethod]
        public void GameConnectCubesVerticallyTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            _model.RotateRobot(_model.Robot, Angle.CounterClockwise);
            Assert.AreEqual(Direction.SOUTH, _model.Robot.Direction);

            _model.ConnectRobot(_model.Robot);

            _model.Board.SetValue(1, 5, new Robot(1, 5, Direction.NORTH, 1));
            _model.Robot = (Robot)_model.Board.GetFieldValue(1, 5);
            _model.ConnectRobot(_model.Robot);


            _model.ConnectCubes(_team1.GetRobot(0), new XYcoordinates(1, 3), new XYcoordinates(1, 4));
            _model.ConnectCubes(_model.Robot, new XYcoordinates(1, 4), new XYcoordinates(1, 3));
          

            //Assert.AreEqual(2, _team1.GetRobot(0).AllConnections().Count);

        }


        [TestMethod]
        public void GameWaitTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(1, 2).GetType());
            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(0, 2).GetType());
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());
            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(1, 1).GetType());

            _model.Wait(_model.Robot);

            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(1, 2).GetType());
            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(0, 2).GetType());
            Assert.AreEqual(typeof(Cube), _model.Board.GetFieldValue(1, 3).GetType());
            Assert.AreEqual(typeof(Robot), _model.Board.GetFieldValue(2, 2).GetType());
            Assert.AreEqual(typeof(Obstacle), _model.Board.GetFieldValue(1, 1).GetType());

        }

        [TestMethod]
        public void GameNextPlayerTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(1,_model.NextPlayerFromTeam1);
            Assert.AreEqual(0, _model.NextPlayerFromTeam2);
            Assert.IsTrue(_model.NextTeam1);

            _model.NextPlayer();

            Assert.AreEqual(2, _model.NextPlayerFromTeam1);
            Assert.AreEqual(0, _model.NextPlayerFromTeam2);
            Assert.IsTrue(_model.NextTeam1);


        }


        [TestMethod]
        public void GameNextPlayerUntilNextTeamTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(1, _model.NextPlayerFromTeam1);
            Assert.AreEqual(0, _model.NextPlayerFromTeam2);
            Assert.IsTrue(_model.NextTeam1);

            _model.NextPlayer();
            _model.NextPlayer();
            _model.NextPlayer();


            Assert.AreEqual(0, _model.NextPlayerFromTeam1);
            Assert.AreEqual(0, _model.NextPlayerFromTeam2);
            Assert.IsFalse(_model.NextTeam1);


        }

        [TestMethod]
        public void GameNextPlayerUntilNextRoundTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);


            Assert.AreEqual(1, _model.NextPlayerFromTeam1);
            Assert.AreEqual(0, _model.NextPlayerFromTeam2);
            Assert.IsTrue(_model.NextTeam1);

            _model.NextPlayer();
            _model.NextPlayer();
            _model.NextPlayer();
            _model.NextPlayer();
            _model.NextPlayer();
            _model.NextPlayer();
            _model.NextPlayer();
            _model.NextPlayer();


            Assert.AreEqual(1, _model.NextPlayerFromTeam1);
            Assert.AreEqual(0, _model.NextPlayerFromTeam2);
            Assert.IsTrue(_model.NextTeam1);
            Assert.AreEqual(2, _model.Round);

        }

        [TestMethod]
        public void GameAdvanceTimeTest()
        {
            _model.NewGame();

            Int32 time = _model.GameTime;
            for (int i = 30; i > 0; i--)
            {
                _model.AdvanceTime();

                time--;

                Assert.AreEqual(time, _model.GameTime); // az idõ csökken
            }

        }

        [TestMethod]
        public void GameModelAdvanceTimeTillRoundOverTest()
        {
            _model.NewGame();

            for (int i = 30; i > 0; i--)
            {
                _model.AdvanceTime();

            }
            Assert.AreEqual(0, _model.GameTime);

            _model.AdvanceTime();

            Assert.AreEqual(30, _model.GameTime);

        }


        [TestMethod]
        public void RobotModelAddConnectionTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

           _model.Robot.AddConnection(new XYcoordinates(1, 3));

            Assert.AreEqual(1, _model.Robot.AllConnections().Count);

        }

        [TestMethod]
        public void RobotModelAddMoreConnectionsTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

            _model.Robot.AddConnection(new XYcoordinates(1, 3));
            _model.Robot.AddConnection(new XYcoordinates(1, 1));
            _model.Robot.AddConnection(new XYcoordinates(2, 2));

            Assert.AreEqual(3, _model.Robot.AllConnections().Count);

        }

        [TestMethod]
        public void RobotModelDeleteConnectionTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

            _model.Robot.AddConnection(new XYcoordinates(1, 3));
            _model.Robot.AddConnection(new XYcoordinates(1, 1));
            _model.Robot.AddConnection(new XYcoordinates(2, 2));

            Assert.AreEqual(3, _model.Robot.AllConnections().Count);

            _model.Robot.DeleteConnection(new XYcoordinates(1, 3));

            Assert.AreEqual(2, _model.Robot.AllConnections().Count);

        }

        [TestMethod]
        public void RobotModelFailedDeleteConnectionTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

            _model.Robot.AddConnection(new XYcoordinates(1, 3));
            _model.Robot.AddConnection(new XYcoordinates(1, 1));
            _model.Robot.AddConnection(new XYcoordinates(2, 2));

            Assert.AreEqual(3, _model.Robot.AllConnections().Count);

            _model.Robot.DeleteConnection(new XYcoordinates(2, 3));

            Assert.AreEqual(3, _model.Robot.AllConnections().Count);

        }

        [TestMethod]
        public void RobotModelClearConnectionsTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

            _model.Robot.AddConnection(new XYcoordinates(1, 3));
            _model.Robot.AddConnection(new XYcoordinates(1, 1));
            _model.Robot.AddConnection(new XYcoordinates(2, 2));

            Assert.AreEqual(3, _model.Robot.AllConnections().Count);

            _model.Robot.ClearConnections();

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

        }

        [TestMethod]
        public void RobotModelIsConnectedTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

            _model.Robot.AddConnection(new XYcoordinates(1, 3));
            _model.Robot.AddConnection(new XYcoordinates(1, 1));
            _model.Robot.AddConnection(new XYcoordinates(2, 2));

            Assert.AreEqual(3, _model.Robot.AllConnections().Count);

            Assert.IsTrue(_model.Robot.IsConnected(new XYcoordinates(1, 3)));
            Assert.IsTrue(_model.Robot.IsConnected(new XYcoordinates(1, 1)));
            Assert.IsTrue(_model.Robot.IsConnected(new XYcoordinates(2, 2)));

        }


        [TestMethod]
        public void RobotModelIsNotConnectedTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(0, _model.Robot.AllConnections().Count);

            _model.Robot.AddConnection(new XYcoordinates(1, 3));
            _model.Robot.AddConnection(new XYcoordinates(1, 1));
            _model.Robot.AddConnection(new XYcoordinates(2, 2));

            Assert.AreEqual(3, _model.Robot.AllConnections().Count);

            Assert.IsFalse(_model.Robot.IsConnected(new XYcoordinates(2, 3)));
            Assert.IsFalse(_model.Robot.IsConnected(new XYcoordinates(0, 1)));
            Assert.IsFalse(_model.Robot.IsConnected(new XYcoordinates(0, 2)));

        }

        [TestMethod]
        public void RobotModelDecreaseHealthTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(3, _model.Robot.Health);

            _model.Robot.DecreaseHealth();

            Assert.AreEqual(2, _model.Robot.Health);

            _model.Robot.DecreaseHealth();

            Assert.AreEqual(1, _model.Robot.Health);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException),
        "The health can't be less than 0.")]
        public void RobotModelInvalidDecreaseHealthTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;
            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(3, _model.Robot.Health);

            _model.Robot.DecreaseHealth();

            Assert.AreEqual(2, _model.Robot.Health);

            _model.Robot.DecreaseHealth();

            Assert.AreEqual(1, _model.Robot.Health);

            _model.Robot.DecreaseHealth();

            Assert.AreEqual(0, _model.Robot.Health);

            _model.Robot.DecreaseHealth();

        }

        [TestMethod]
        public void TeamModelGetRobotByNumTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            _model.Robot = _team1.GetRobotByNum(2)!;

            Assert.AreEqual(_model.Team1.GetRobot(2), _model.Robot);

            _model.Robot = _team2.GetRobotByNum(7)!;

            Assert.AreEqual(_model.Team2.GetRobot(3), _model.Robot);

        }

        [TestMethod]
        public void TeamModelInvalidGetRobotByNumTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            _model.Robot = _team1.GetRobotByNum(8)!;

            Assert.AreEqual(null, _model.Robot);

        }

        [TestMethod]
        public void TeamModelIsInThisTeamTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            _model.Robot = _team1.GetRobot(0);

            Assert.IsTrue(_team1.IsInThisTeam(_model.Robot));

        }

        [TestMethod]
        public void TeamModelFalseIsInThisTeamTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            _model.Robot = _team1.GetRobot(0);

            Assert.IsFalse(_team2.IsInThisTeam(_model.Robot));

        }

        [TestMethod]
        public void TeamModelRemoveRobotFromTeamTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(4,_model.Team1.Robots.Length);

            _model.Team1.RemoveRobotFromTeam(_model.Robot);

            Assert.AreEqual(3, _model.Team1.Robots.Length);

        }

        [TestMethod]
        public void TeamModelInvalidRemoveRobotFromTeamTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            _model.Robot = _team1.GetRobot(0);

            Assert.AreEqual(4, _model.Team2.Robots.Length);

            _model.Team2.RemoveRobotFromTeam(_model.Robot);

            Assert.AreEqual(4, _model.Team2.Robots.Length);

        }

        [TestMethod]
        public void TeamModelFalseIsEmptyTeamTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            Assert.AreEqual(4, _model.Team1.Robots.Length);

            _model.Team1.RemoveRobotFromTeam(_team1.GetRobot(0));

            Assert.AreEqual(3, _model.Team1.Robots.Length);

            _model.Team1.RemoveRobotFromTeam(_team1.GetRobot(0));

            Assert.AreEqual(2, _model.Team1.Robots.Length);

            Assert.IsFalse(_model.Team1.IsEmptyTeam());

        }

        [TestMethod]
        public void TeamModelIsEmptyTeamTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            Assert.AreEqual(4, _model.Team1.Robots.Length);

            _model.Team1.RemoveRobotFromTeam(_team1.GetRobot(0));

            Assert.AreEqual(3, _model.Team1.Robots.Length);

            _model.Team1.RemoveRobotFromTeam(_team1.GetRobot(0));

            Assert.AreEqual(2, _model.Team1.Robots.Length);

            _model.Team1.RemoveRobotFromTeam(_team1.GetRobot(0));

            Assert.AreEqual(1, _model.Team1.Robots.Length);

            _model.Team1.RemoveRobotFromTeam(_team1.GetRobot(0));

            Assert.AreEqual(0, _model.Team1.Robots.Length);

            Assert.IsTrue(_model.Team1.IsEmptyTeam());

        }

        [TestMethod]
        public void NoticeBoardModelGenerateTasksTest()
        {
            _model.NewGame();
            _model.Board = _mockedTable;
            _model.Team1 = _team1;
            _model.Team2 = _team2;

            Assert.IsTrue("Hard" == _model.NoticeBoard.TaskName || "Easy" == _model.NoticeBoard.TaskName);

            Assert.IsTrue(1 == _model.NoticeBoard.Deadline);

            Assert.IsTrue(3 <= _model.NoticeBoard.TaskReward && 5 >= _model.NoticeBoard.TaskReward);

            Assert.IsTrue(9 == _model.NoticeBoard.Fields.Length);

        }



        /*

        private void Model_GameAdvanced(Object? sender, MaciLaciEventArgs e)
        {
            Assert.IsTrue(_model.GameTime >= 0); // a játékidõ nem lehet negatív
            Assert.AreEqual(_model.Table.NoMoreBaskets() || _model.Busted(), _model.IsGameOver);
            // a tesztben a játéknak csak akkor lehet vége, ha elfogytak a kosarak vagy elkapták a játékost

            Assert.AreEqual(e.BasketCount, _model.BasketCount); // a két értéknek egyeznie kell
            Assert.AreEqual(e.GameTime, _model.GameTime); // a két értéknek egyeznie kell
            Assert.IsFalse(e.IsWon); // még nem nyertek 
            Assert.IsFalse(e.IsLost); // még nem vesztettek 
        }

        private void Model_GameOver(Object? sender, MaciLaciEventArgs e)
        {
            Assert.IsTrue(_model.IsGameOver); // biztosan vége van a játéknak
            Assert.IsFalse(e.IsWon);
            Assert.IsTrue(e.IsLost); // a tesztben csak vereség esetén váltódik ki
        }*/
    }
}
