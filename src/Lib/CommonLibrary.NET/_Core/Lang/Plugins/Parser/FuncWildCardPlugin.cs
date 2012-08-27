﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ComLib.Lang;
using ComLib.Lang.Helpers;

namespace ComLib.Lang.Extensions
{

    /* *************************************************************************
    <doc:example>	
    // FuncWildCard plugin allows using functions with changing names allowing
    // a single function to handle function calls with different names.
     
    
    // @summary: A single function can be called using wildcards
    // @arg name: wildcard,  type: string, example: "by name password email role"
    // @arg name: wildcardParts, type: list, example: ['by', 'name' 'password', 'email', 'role' ]
    // @arg name: args, type: list, example: [ 'kreddy', 'admin' ]
    function "create user by" * ( wildcard, wildcardParts, args ) 
    {
	    person = new Person()
		
	    // Option 1: Use the full name to determine what to do
	    if wildcard== "name password email role" 
	    {
		    person.Name = args[0]
		    person.Password = args[1]
		    person.Email = args[2]
		    person.Role = args[3]
	    }
		
	    // Option 2: Use the individual name parts
	    for( var ndx = 0; ndx < wildcardParts.length; ndx++)
	    {
                    part = wildcardParts[ndx]

		    if part == "name" then person.Name = args[ndx]
		    else if part == "password" then person.Password = args[ndx]
		    else if part == "email"    then person.Email = args[ndx]
		    else if part == "role"     then person.Role = args[ndx]
	    }
	    person.Save()
    }


    create user by name email ( "user02", "user02@abc.com" )
    create user by name password email role ( "user01", "password", "user01@abc.com", "user" )
    
    </doc:example>
    ***************************************************************************/


    /// <summary>
    /// Combinator for handles method/function calls in a more fluent way.
    /// </summary>
    public class FuncWildCardPlugin : ExprPlugin
    {
        private static string[] _tokens = new string[] { "$IdToken" };        


        /// <summary>
        /// Initialize.
        /// </summary>
        public FuncWildCardPlugin()
        {
            Precedence = 10;
            IsStatement = true;
            _startTokens = _tokens;
            IsContextFree = false;
        }


        /// <summary>
        /// This can not handle all idtoken based expressions.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool CanHandle(Token current)
        {            
            if (!(current.Kind == TokenKind.Ident)) return false;

            var next = _tokenIt.Peek(1, false);
            if (!(next.Token.Kind == TokenKind.Ident)) return false;

            // Check if multi-word function name.
            // e.g. "refill inventory"
            // 1. Is it a function call?
            var tokens = _tokenIt.PeekConsequetiveIdsAppended();
            var result = FluentHelper.MatchFunctionWildCard(_parser.Context.Symbols, _parser.Context.Functions, tokens);
            return result.Exists;
        }


        /// <summary>
        /// Parses the fluent function call.
        /// </summary>
        /// <returns></returns>
        public override Expr Parse()
        {
            // 1. Is it a function call?
            var ids = _tokenIt.PeekConsequetiveIdsAppended();
            var result = FluentHelper.MatchFunctionWildCard(_parser.Context.Symbols, _parser.Context.Functions, ids);
            var fnameToken = _tokenIt.NextToken;

            _tokenIt.Advance(result.TokenCount + 1);

            string remainderOfFuncName = string.Empty;
            var parts = new List<Expr>();
            TokenData firstPart = null;

            // NOTES:
            // Given: function "Find user by" *
            // And: called via Find use by name role 
            // wildcard part 1: name
            // wildcard part 2: role
            // full wildcard: "name role"

            // 1. Capture all the remaining parts of the wild card.
            while (_tokenIt.NextToken.Token.Kind == TokenKind.Ident)
            {
                string part = _tokenIt.NextToken.Token.Text;
                
                // a. Store the token of the first wildcard part
                if(firstPart == null)
                    firstPart = _tokenIt.NextToken;

                // b. Build up the full name from all wildcards
                remainderOfFuncName += " " + part;

                // c. Create a constant expr from the wildcard
                // as it will be part of an array of strings passed to function
                var partExp = new ConstantExpr(part);
                _parser.SetScriptPosition(partExp, _tokenIt.NextToken);
                parts.Add(partExp);

                // d. Move to the next token for another possible wildcard.
                _tokenIt.Advance();

                // e. Check for end of statement.
                if (_tokenIt.IsEndOfStmtOrBlock())
                    break;
            }

            var exp = new FunctionCallExpr();
            remainderOfFuncName = remainderOfFuncName.Trim();
            ConstantExpr fullWildCard = new ConstantExpr(string.Empty);
            _parser.SetScriptPosition(fullWildCard, fnameToken);

            // 2. Create a constant expr representing the full wildcard              
            if(!string.IsNullOrEmpty(remainderOfFuncName))
            {
                fullWildCard.Value = remainderOfFuncName;
                _parser.SetScriptPosition(fullWildCard, firstPart);
            }   

            if (_tokenIt.NextToken.Token == Tokens.LeftParenthesis)
            {
                _parser.ParseParameters(exp, true, false, false);
            }
            exp.NameExp = new VariableExpr(result.Name);
            
            // Have to restructure the arguments.
            // 1. const expr     , fullwildcard,   "name role"
            // 2. list<constexpr>, wildcard parts, ["name", "role"]
            // 3. list<expr>,      args,           "kishore", "admin"
            var args = new List<Expr>();
            args.Add(fullWildCard);
            args.Add(new DataTypeExpr(parts));
            args.Add(new DataTypeExpr(exp.ParamListExpressions));

            // Finally reset the parameters expr on the function call.
            exp.ParamListExpressions = args;
            return exp;
        }
    }
}