using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Board
    {
        private int height;
        private int width;
        private Field[][] fields;
        private Team[] teams;
        public Board(int _height, int _width, Field[][] _fields)
        {
            height = _height;
            width = _width;
            fields = _fields;
        }
        public bool moveRobot(Robot robot, Direction dir) { /*code*/ return true; }
        public bool rotateRobot(Robot robot, Angle angle) { /*code*/ return true; }
        public bool connectRobot(Robot robot, Direction dir) { /*code*/ return true; }
        public bool disConnectRobot(Robot robot, Direction dir) { /*code*/ return true; }
        public bool connectCubes(Robot robot, RelDistance distance) { /*code*/ return true; }
        public bool disConnectCubes(Robot robot, RelDistance distance) { /*code*/ return true; }
        public bool clean(Robot robot, Direction dir) { /*code*/ return true; }
        public event EventHandler<RobotEventArgs> MoveRobot_;
        public event EventHandler<RobotEventArgs> RotateRobot_;
        public event EventHandler<RobotEventArgs> ConnectRobot_;
        public event EventHandler<RobotEventArgs> DisConnectRobot_;
        public event EventHandler<RobotEventArgs> ConnectCubes_;
        public event EventHandler<RobotEventArgs> DisConnectCubes_;
        public event EventHandler<RobotEventArgs> Clean_;
        private void MoveRobot(int x, int y) { /*code*/ ; }
        private void RotateRobot(Direction dir) { /*code*/ ; }
        private void ConnectRobot(Direction dir) { /*code*/ ; }
        private void DisConnectRobot(Direction dir) { /*code*/ ; }
        private void ConnectCubes(int x, int y) { /*code*/ ; }
        private void DisConnectCubes(int x, int y) { /*code*/ ; }
        private void Clean(int x, int y) { /*code*/ ; }
    }

    public struct RelDistance
    {
        int x;
        int y;
    }

    public enum Angle
    {
        _90, _180, _270, _360
    }
}
