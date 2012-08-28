﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;


namespace ComLib.Lang.Extensions
{

    /* *************************************************************************
    <doc:example>	
    // Round plugin provides functions to round, round up or round down numbers.
    
    var a = 2.3;
    
    // Rounds the number using standing round technique of .4
    var b = round 2.3;
    
    // Gets rounded up to 3
    var c = round up 2.3; 
    
    // Gets rounded down to 2
    var d = round down 2.3;
    
    </doc:example>
    ***************************************************************************/

    /// <summary>
    /// Combinator for handling swapping of variable values. swap a and b.
    /// </summary>
    public class RoundPlugin : ExprPlugin
    {
        private static string[] _tokens = new string[] { "Round", "round" };

        /// <summary>
        /// How to round
        /// </summary>
        public enum RoundMode
        {
            /// <summary>
            /// Normal rounding technique.
            /// </summary>
            Round,


            /// <summary>
            /// Rounds up to the nearest integer ( ceil )
            /// </summary>
            RoundUp,

            
            /// <summary>
            /// Rounds down to the nearest integer ( floor )
            /// </summary>
            RoundDown
        }


        /// <summary>
        /// Intialize.
        /// </summary>
        public RoundPlugin()
        {
            IsStatement = false;
            IsAutoMatched = true;
            _startTokens = _tokens;
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get
            {
                return "round ( up | down )? <expression>";
            }
        }


        /// <summary>
        /// Examples
        /// </summary>
        public override string[] Examples
        {
            get
            {
                return new string[]
                {
                    "round 2.4",
                    "round up 2.4",
                    "round down 2.4"
                };
            }
        }


        /// <summary>
        /// run step 123.
        /// </summary>
        /// <returns></returns>
        public override Expr Parse()
        {            
            RoundMode mode = RoundMode.Round;
            Token t = _tokenIt.Peek().Token;
            if (string.Compare(t.Text, "up", StringComparison.InvariantCultureIgnoreCase) == 0) 
            {
                mode = RoundMode.RoundUp;
                _tokenIt.Advance();
            }
            else if (string.Compare(t.Text, "down", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                mode = RoundMode.RoundDown;
                _tokenIt.Advance();
            }
            _tokenIt.Advance();

            // The expression to round.
            var exp = _parser.ParseExpression(null, true, true);
            return new RoundExpr(mode, exp);
        }
    }


    /// <summary>
    /// Variable expression data
    /// </summary>
    public class RoundExpr : Expr
    {

        private RoundPlugin.RoundMode _mode;
        private Expr _exp;

        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="mode">How to round</param>
        /// <param name="exp">The expression value to round</param>
        public RoundExpr(RoundPlugin.RoundMode mode, Expr exp)
        {
            _mode = mode;
            _exp = exp;
        }


        /// <summary>
        /// Evaluate
        /// </summary>
        /// <returns></returns>
        public override object DoEvaluate()
        {
            var result = _exp.Evaluate();
            double val = 0;
            if (result is double)
                val = Convert.ToDouble(result);
            else if (result is int)
                val = Convert.ToDouble(result);
            else
                throw new LangException("runtime", "Unexpected type to round", this.Ref.ScriptName, this.Ref.Line, this.Ref.CharPos);

            if (_mode == RoundPlugin.RoundMode.Round)
            {
                var d = Convert.ToDouble(val + .5);
                d = Math.Floor(d);
                val = Convert.ToDouble(d);
            }
            else if (_mode == RoundPlugin.RoundMode.RoundDown)
                val = Math.Floor(val);
            else if (_mode == RoundPlugin.RoundMode.RoundUp)
                val = Math.Ceiling(val);

            return val;
        }
    }
}
