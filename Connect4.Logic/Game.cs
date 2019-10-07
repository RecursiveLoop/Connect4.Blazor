using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static Connect4.Logic.Exceptions;

namespace Connect4.Logic
{
    public class Game
    {
        #region Private Fields

        Board _Board = null;
        Enums.GameStates _CurrentState;
        int _Height;
        int _Width;
        /// <summary>
        /// The number of discs required in a line/diagonal before a win can be granted. 
        /// Yes, it's Connect4, but you never know....
        /// </summary>
        public const int cConnectionsRequired = 4;

        /// <summary>
        /// Algorithms for checking for winning state.
        /// </summary>
        List<IWinCheckAlgorithm> winningCheckAlgorithms = null;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Height">The height of the board to be played. Must be greater than 0.</param>
        /// <param name="Width">The width of the board to be played. Must be greater than 0.</param>
        public Game(int Height, int Width)
        {
            if (Height <= 0 || Width <= 0)
                throw new InvalidBoardDimensionsException("The height and width of the board must be greater than 0.");

            if (Height < cConnectionsRequired || Width < cConnectionsRequired)
                throw new InvalidBoardDimensionsException("The board is too small, players will not be able to win.");

            this.winningCheckAlgorithms = new List<IWinCheckAlgorithm>();
            this.winningCheckAlgorithms.Add(new DiagonalWinCheckAlgorithm());
            this.winningCheckAlgorithms.Add(new HorizontalWinCheckAlgorithm());
            this.winningCheckAlgorithms.Add(new VerticalWinCheckAlgorithm());

            this._Height = Height;
            this._Width = Width;
            this._CurrentState = Enums.GameStates.YellowsTurn;
            _Board = new Board(Height, Width, this);

        }

        #region Properties

        /// <summary>
        /// Event which gets fired when the game state changes.
        /// </summary>
        public event EventHandler<GameStateChangedEventArgs> GameStateChanged;

        /// <summary>
        /// Gets a reference to the board currently in play.
        /// </summary>
        public Board Board
        {
            get { return _Board; }
        }

        /// <summary>
        /// Gets the current state of the game.
        /// </summary>
        public Enums.GameStates CurrentState
        {
            get
            {
                return _CurrentState;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the game state changed event.
        /// </summary>
        private void FireGameStateChangedEvent()
        {
            if (this.GameStateChanged != null)
                GameStateChanged(this, new GameStateChangedEventArgs(this._CurrentState));
        }

        /// <summary>
        /// Resets the board and re-initializes the game. WARNING: All existing game state will be lost!
        /// </summary>
        public void Reset()
        {
            _Board.Dispose();
            _Board = null;
            _Board = new Board(this._Height, this._Width, this);
            this._CurrentState = Enums.GameStates.YellowsTurn;
            FireGameStateChangedEvent();
        }

        /// <summary>
        /// Checks the board for a winner or a draw, and updates the game state accordingly.
        /// </summary>
        /// <returns>True if there was a win or draw, false otherwise.</returns>
        private bool CheckForWinOrDraw()
        {

            if (this.Board != null)
            {
                for (int X = 0; X <= this._Board.Discs.GetUpperBound(0); X++)
                {
                    for (int Y = 0; Y <= this._Board.Discs.GetUpperBound(1); Y++)
                    {
                        var discCurrentlyChecking = this._Board.Discs[X, Y];
                        if (discCurrentlyChecking != null)
                        {
                            if (this.winningCheckAlgorithms != null &&
                                this.winningCheckAlgorithms.Any(algo => algo.CheckForWinningCondition(this._Board, discCurrentlyChecking)))
                            {
                                if (discCurrentlyChecking.Side == Enums.Sides.Red)
                                    this._CurrentState = Enums.GameStates.RedWins;
                                else
                                    this._CurrentState = Enums.GameStates.YellowWins;
                                return true;
                            }
                        }
                    }

                }
                // Check for a draw
                if (this._Board.IsFull)
                {
                    this._CurrentState = Enums.GameStates.Draw;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the current game state as a readable string.
        /// </summary>
        /// <returns></returns>
        public string GetGameStateString()
        {
            switch (this._CurrentState)
            {
                case Enums.GameStates.Draw:
                    return "Draw";
                    break;
                case Enums.GameStates.RedsTurn:
                    return "Red's Turn";
                    break;
                case Enums.GameStates.RedWins:
                    return "Red Wins";
                    break;
                case Enums.GameStates.YellowsTurn:
                    return "Yellow's Turn";
                    break;
                case Enums.GameStates.YellowWins:
                    return "Yellow Wins";
                    break;
            }
            return "Unknown";
        }



        /// <summary>
        /// Adds a disc to the game board.
        /// </summary>
        /// <param name="disc"></param>
        /// <param name="RowIndex"></param>
        public void AddDisc(Disc disc, int RowIndex)
        {
            if (this._CurrentState != Enums.GameStates.RedsTurn && this._CurrentState != Enums.GameStates.YellowsTurn)
                throw new Exception("The game has already ended! " + this.GetGameStateString());

            if (this._CurrentState == Enums.GameStates.YellowsTurn && disc.Side == Enums.Sides.Red)
                throw new WrongPlayerMoveException("It is currently yellow's turn. Play a yellow disc.");
            else if (this._CurrentState == Enums.GameStates.RedsTurn && disc.Side == Enums.Sides.Yellow)
                throw new WrongPlayerMoveException("It is currently red's turn. Play a red disc.");

            this._Board.AddDisc(disc, RowIndex);


            if (!CheckForWinOrDraw()) // Need to switch the sides that are currently playing around
            {
                if (this._CurrentState == Enums.GameStates.RedsTurn)
                    this._CurrentState = Enums.GameStates.YellowsTurn;
                else
                    this._CurrentState = Enums.GameStates.RedsTurn;
            }

            // Either way, the game state has changed
            FireGameStateChangedEvent();
        }

        #endregion 

    }

    public class GameStateChangedEventArgs : EventArgs
    {
        Enums.GameStates NewState;

        public GameStateChangedEventArgs(Enums.GameStates newState) : base()
        {
            this.NewState = newState;
        }
    }
}
