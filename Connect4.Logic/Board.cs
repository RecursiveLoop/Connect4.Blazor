using System;
using System.Collections.Generic;
using System.Text;
using static Connect4.Logic.Exceptions;

namespace Connect4.Logic
{
    public class Board : IDisposable
    {
        #region Private Fields

        public Disc[,] Discs { get; set; }

        int _Height { get; set; }

        int _Width { get; set; }

        bool _IsDisposed = false;

        /// <summary>
        /// Reference back to the game object.
        /// </summary>
        Game _theGame = null;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Height">The height of the game board, must be greater than 0.</param>
        /// <param name="Width">The width of the game board, must be greater than 0.</param>
        /// <param name="theGame">Reference back to the game object.</param>
        internal Board(int Height, int Width, Game theGame)
        {

            this._Width = Width;
            this._Height = Height;
            this.Discs = new Disc[Width, Height];
            this._theGame = theGame;
        }

        #region Properties

        /// <summary>
        /// The width of the game board.
        /// </summary>
        public int Width
        {
            get { return _Width; }
        }

        /// <summary>
        /// The height of the game board.
        /// </summary>
        public int Height
        {
            get
            {
                return _Height;
            }
        }

        /// <summary>
        /// Reference back to the game object.
        /// </summary>
        public Game Game
        {
            get { return _theGame; }
        }

        /// <summary>
        /// Gets a value stating if the board is full.
        /// </summary>
        public bool IsFull
        {
            get
            {
                for (int x = 0; x <= this.Discs.GetUpperBound(0); x++)
                {
                    for (int y = 0; y <= this.Discs.GetUpperBound(1); y++)
                        if (Discs[x, y] == null)
                            return false;
                }
                return true;
            }
        }

        #endregion


        #region Methods


        /// <summary>
        /// Adds a disc to the game board.
        /// </summary>
        /// <param name="disc"></param>
        /// <param name="RowIndex">The index of the row to insert the disc. This index starts from 0 (not 1!)</param>
        /// <returns>The coordinate where the disc was inserted.</returns>
        internal Tuple<int, int> AddDisc(Disc disc, int RowIndex)
        {

            if (RowIndex < 0 || RowIndex > (this.Discs.GetUpperBound(0)))
                throw new OutOfGameBoardBoundsException("The disc must be inserted within the width of the game board.");

            for (int idx = 0; idx <= this.Discs.GetUpperBound(1); idx++)
            {
                if (this.Discs[RowIndex, idx] == null)
                {

                    this.Discs[RowIndex, idx] = disc;
                    disc.SetCoordinates(RowIndex, idx);

                    return new Tuple<int, int>(RowIndex, idx);
                }
            }

            throw new BoardFullException("Could not find an empty slot to insert the disc into the requested row.");
        }

        /// <summary>
        /// Prints out the entire board in a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = this.Discs.GetUpperBound(1); y >= 0; y--)
            {
                for (int x = 0; x <= this.Discs.GetUpperBound(0); x++)
                {
                    if (this.Discs[x, y] == null)
                        sb.Append("X");
                    else if (this.Discs[x, y].Side == Enums.Sides.Red)
                        sb.Append("R");
                    else
                        sb.Append("Y");
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                if (disposing)
                {
                    this._theGame = null;

                    // Free the discs!
                }

                _IsDisposed = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);

        }
        #endregion

        #endregion
    }
}
