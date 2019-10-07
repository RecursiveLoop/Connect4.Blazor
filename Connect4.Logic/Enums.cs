using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Logic
{
    public class Enums
    {
        public enum GameStates
        {

            YellowsTurn = 1,
            RedsTurn = 2,
            Draw = 3,
            RedWins = 4,
            YellowWins = 5

        }

        public enum Sides
        {
            Red = 0,
            Yellow = 1
        }
    }
}
