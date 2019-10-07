using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Logic
{
    public class Disc
    {
        #region Private Fields

        Enums.Sides _Side;
        /// <summary>
        /// The X-axis coordinate of this disc, if it is added to the game board. Starts at a 0 value.
        /// </summary>
        int? _XCoordinate = null;
        /// <summary>
        ///  The Y-axis coordinate of this disc, if it is added to the game board. Starts at a 0 value.
        /// </summary>
        int? _YCoordinate = null;

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Side"></param>
        public Disc(Enums.Sides Side)
        {
            this._Side = Side;
        }

        #region Properties

        /// <summary>
        /// Gets the X-axis coordinate of this disc. If this disc has been added to a game board, it is the responsibility of the game board to set this value.
        /// </summary>
        public int? XCoordinate { get { return _XCoordinate; } }

        /// <summary>
        ///  Gets the Y-axis coordinate of this disc. If this disc has been added to a game board, it is the responsibility of the game board to set this value.
        /// </summary>
        public int? YCoordinate { get { return _YCoordinate; } }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the side of the player that added the disc.
        /// </summary>
        public Enums.Sides Side
        {
            get { return this._Side; }
        }

        /// <summary>
        /// Sets the coordinates of this disc on a game board.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        internal void SetCoordinates(int X, int Y)
        {
            this._XCoordinate = X;
            this._YCoordinate = Y;
        }

        #endregion

    }
}
