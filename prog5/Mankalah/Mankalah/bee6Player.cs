using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; // new Stopwatch()

namespace Mankalah
{
    class bee6Player : Player
    {
        public bee6Player(Position pos, int timeLimit) : base(pos, "BEK", timeLimit) { }

        // chooseMove calls minimaxValue
        public override int chooseMove(Board b)
        {
            // Initialize and start stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int i = 1;
            moveResult move = new moveResult(0, 0);
            while (stopwatch.ElapsedMilliseconds < getTimePerMove())
            {
                move = minimaxValue(b, i++, Int32.MinValue, Int32.MaxValue);
            }
            return move.getMove();
        }

        // minimax -> time limit
        private moveResult minimaxValue(Board b, int d, int alpha, int beta)
        {
            int best_move = 0;
            int best_value;

            if (b.gameOver() || d== 0)
            {
                return new moveResult(0, evaluate(b));
            }

            // Top of Mankalah board is MAX
            if (b.whoseMove() == Position.Top)
            {
                best_value = Int32.MinValue;
                for (int move = 7; move <= 12; move++)
                {
                    if (b.legalMove(move)) // [and time not expired]
                    {
                        // duplicate board
                        Board b1 = new Board(b);

                        // make the move
                        b1.makeMove(move, false);

                        // find its value
                        moveResult value = minimaxValue(b1, d - 1, alpha, beta); 
                        
                        // remember if best
                        if (value.getScore() > best_value) {         
                            best_value = value.getScore();
                            best_move = move;
                        }

                        if (best_value > alpha) {
                            alpha = best_value;
                        }
                    }
                }
                return new moveResult(best_move, best_value);
            }

            // Bottom of Mankalah board is MIN
            else
            {
                best_value = Int32.MaxValue;
                for (int move = 0; move <= 5; move++)
                {
                    if (b.legalMove(move))
                    {
                        // duplicate board
                        Board b1 = new Board(b);

                        // make the move
                        b1.makeMove(move, false);

                        // find its value
                        moveResult value = minimaxValue(b1, d - 1, alpha, beta);

                        // remember if ...
                        if (value.getScore() < best_value)
                        {
                            best_value = value.getScore();
                            best_move = move;
                        }

                        if (best_value < beta)
                        {
                            beta = best_value; 
                        }
                    }
                }
                return new moveResult(best_move, best_value);
            }
        }

        /*
        * Return a number saying how much we like this board; positive or negative value
        * TOP is MAX, so positive scores should be better for TOP
        * BOTTOM is MIN, so negative scores should be better for BOTTOM
        * Consider at least three more factors; such as how many potential go-again moves each side has, captures, numbers of stones on each side, etc.
        */
        public override int evaluate(Board b)
        {
            int score = b.stonesAt(13) - b.stonesAt(6);

            int go_again_moves = 0;
            int total_captures = 0;
            int total_stones = 0;

            // Top of Mankalah (Positive)
            if (b.whoseMove() == Position.Top)
            {
                for (int move = 7; move <= 12; move++)
                {
                    // add the total stones in the top row
                    total_stones += b.stonesAt(move);

                    // add the total go agains
                    if (b.stonesAt(move) - (13 - move) == 0)
                    {
                        go_again_moves++;
                    }

                    // The total stones of the targeted/end location after I have placed all the stones I have, depending on which board I choose
                    int next_target = b.stonesAt((b.stonesAt(move) + move) % 13);

                    // add the total captures
                    if ((next_target == 0) && (b.stonesAt(12 - next_target) != 0))
                    {
                        total_captures += b.stonesAt(12 - next_target);
                    }
                }

            }

            // Bottom of Mankalah (Negative)
            else
            {
                for (int move = 0; move <= 5; move++)
                {
                    // sub the total stones in the bottom row
                    total_stones -= b.stonesAt(move);

                    // sub the total go agains
                    if (b.stonesAt(move) - (6 - move) == 0)
                    {
                        go_again_moves--;
                    }

                    // The total stones of the targeted/end location after I have placed all the stones I have, depending on which board I choose
                    int next_target = b.stonesAt((b.stonesAt(move) + move) % 6);

                    // sub the total captures
                    if ((next_target == 0) && (b.stonesAt(12 - next_target) != 0))
                    {
                        total_captures -= b.stonesAt(12 - next_target);
                    }
                }
            }

            // Calculates the total MAX or MIN depending on the user position 
            score += go_again_moves + total_captures + total_stones;

            return score;
        }

        // gloat
        public override String gloat()
        {
            return "I win. Good game.";
        }

        // image
        public override String getImage()
        {
            return "benedicto.jpg";
        }
    }

    class moveResult
    {
        private int move;
        private int score;

        public moveResult(int next_move, int next_score)
        {
            move = next_move;
            score = next_score;
        }

        // Getters
        public int getMove()
        {
            return move;
        }

        public int getScore()
        {
            return score;
        }

        // Setter
        public void setMove(int next_move)
        {
            move = next_move;
        }

        public void setScore(int next_score)
        {
            score = next_score;
        }
    }
}

