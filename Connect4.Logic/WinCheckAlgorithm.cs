using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4.Logic
{
    /// <summary>
    /// Interface for defining an algorithm for checking for a winning condition.
    /// </summary>
    public interface IWinCheckAlgorithm
    {
        bool CheckForWinningCondition(Board board, Disc disc);
    }

    /// <summary>
    /// Performs a horizontal winning condition check.
    /// </summary>
    public class HorizontalWinCheckAlgorithm : IWinCheckAlgorithm
    {
        public bool CheckForWinningCondition(Board board, Disc disc)
        {
            int countOfSameColour = 0; // Number of discs with the same colour

            // Check forward. We need to take into consideration the width of the board when looping
            for (int x = disc.XCoordinate.Value; x <= Math.Min((disc.XCoordinate.Value + Game.cConnectionsRequired), board.Discs.GetUpperBound(0)); x++)
            {

                if (board.Discs[x, disc.YCoordinate.Value] != null && board.Discs[x, disc.YCoordinate.Value].Side == disc.Side)
                    countOfSameColour++;
                else

                    break;
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true; // Win condition

            countOfSameColour = 0; // Re-initialise to start the backward count

            // Check backward. We need to make sure we don't go lower than 0 in our check.
            for (int x = disc.XCoordinate.Value; x >= Math.Max(disc.XCoordinate.Value - Game.cConnectionsRequired, 0); x--)
            {

                if (board.Discs[x, disc.YCoordinate.Value] != null && board.Discs[x, disc.YCoordinate.Value].Side == disc.Side)
                    countOfSameColour++;
                else

                    break;
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true;
            else
                return false;

        }
    }

    /// <summary>
    /// Performs vertical checks for winning conditions.
    /// </summary>
    public class VerticalWinCheckAlgorithm : IWinCheckAlgorithm
    {
        public bool CheckForWinningCondition(Board board, Disc disc)
        {
            int countOfSameColour = 0; // Number of discs with the same colour

            // Check upwards. We need to take into consideration the height of the board when looping
            for (int y = disc.YCoordinate.Value; y <= Math.Min((disc.YCoordinate.Value + Game.cConnectionsRequired), board.Discs.GetUpperBound(1)); y++)
            {

                if (board.Discs[disc.XCoordinate.Value, y] != null && board.Discs[disc.XCoordinate.Value, y].Side == disc.Side)
                    countOfSameColour++;
                else

                    break;
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true; // Win condition

            countOfSameColour = 0; // Re-initialise to start the backward count

            // Check downward. We need to make sure we don't go lower than 0 in our check.
            for (int y = disc.YCoordinate.Value; y >= Math.Max(disc.YCoordinate.Value - Game.cConnectionsRequired, 0); y--)
            {

                if (board.Discs[disc.XCoordinate.Value, y] != null && board.Discs[disc.XCoordinate.Value, y].Side == disc.Side)
                    countOfSameColour++;
                else

                    break;
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Performs diagonal checks for winning conditions.
    /// </summary>
    public class DiagonalWinCheckAlgorithm : IWinCheckAlgorithm
    {
        public bool CheckForWinningCondition(Board board, Disc disc)
        {
            int countOfSameColour = 0; // Number of discs with the same colour

            // There are 4 diagonal win conditions, we have to check for all of them. It seems complex but really it is merely a combination
            // of positive, negative, X and Y

            // 1) Let's tackle positive X, positive Y first - this means we're checking for "up and right" in the 2d space (left hand rule)
            // We are incrementing both X and Y axis values
            for (int i = 0; i < Game.cConnectionsRequired; i++)
            {
                if (disc.XCoordinate.Value + i <= board.Discs.GetUpperBound(0) && disc.YCoordinate.Value + i <= board.Discs.GetUpperBound(1))
                {
                    if (board.Discs[disc.XCoordinate.Value + i, disc.YCoordinate.Value + i] != null && board.Discs[disc.XCoordinate.Value + i, disc.YCoordinate.Value + i].Side == disc.Side)
                        countOfSameColour++;
                    else
                        break; // No point continuing
                }
                else
                    break; // No point continuing
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true; // Win condition

            countOfSameColour = 0; // Re-initialise

            // 2) Next, positive X, negative Y - this is checking "down and right" in the 2d space
            // We are incrementing X axis values but decrementing Y axis values

            for (int i = 0; i < Game.cConnectionsRequired; i++)
            {
                if (disc.XCoordinate.Value + i <= board.Discs.GetUpperBound(0) && disc.YCoordinate.Value - i >= 0)
                {
                    if (board.Discs[disc.XCoordinate.Value + i, disc.YCoordinate.Value - i] != null && board.Discs[disc.XCoordinate.Value + i, disc.YCoordinate.Value - i].Side == disc.Side)
                        countOfSameColour++;
                    else
                        break; // No point continuing
                }
                else
                    break; // No point continuing
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true; // Win condition

            countOfSameColour = 0; // Re-initialise

            // 3) Next, negative X, positive Y - this is checking "up and left" in the 2d space
            // Decrement X axis values but increment Y axis values

            for (int i = 0; i < Game.cConnectionsRequired; i++)
            {
                if (disc.XCoordinate.Value - i >= 0 && disc.YCoordinate.Value + i <= board.Discs.GetUpperBound(1))
                {
                    if (board.Discs[disc.XCoordinate.Value - i, disc.YCoordinate.Value + i] != null && board.Discs[disc.XCoordinate.Value - i, disc.YCoordinate.Value + i].Side == disc.Side)
                        countOfSameColour++;
                    else
                        break; // No point continuing
                }
                else
                    break; // No point continuing
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true; // Win condition

            countOfSameColour = 0; // Re-initialise

            // 4) Lastly, negative X, negative Y - this is checking "down and left" in the 2d space
            // Decrement both X and Y axis values
            for (int i = 0; i < Game.cConnectionsRequired; i++)
            {
                if (disc.XCoordinate.Value - i >= 0 && disc.YCoordinate.Value - i >= 0)
                {
                    if (board.Discs[disc.XCoordinate.Value - i, disc.YCoordinate.Value - i] != null && board.Discs[disc.XCoordinate.Value - i, disc.YCoordinate.Value - i].Side == disc.Side)
                        countOfSameColour++;
                    else
                        break; // No point continuing
                }
                else
                    break; // No point continuing
            }

            if (countOfSameColour >= Game.cConnectionsRequired)
                return true; // Win condition
            else
                return false;
        }
    }
}
