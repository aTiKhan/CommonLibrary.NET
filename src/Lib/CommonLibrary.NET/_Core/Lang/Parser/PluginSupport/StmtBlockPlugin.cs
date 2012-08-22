﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang
{
    /// <summary>
    /// A combinator to extend the parser
    /// </summary>
    public class StmtBlockPlugin : StmtPlugin
    {
        /// <summary>
        /// Parses a block by first pushing symbol scope and then popping after completion.
        /// </summary>
        public virtual void ParseBlock(BlockStmt stmt)
        {
            this.Ctx.Symbols.Push(new SymbolsNested(string.Empty), true);
            stmt.SymScope = this.Ctx.Symbols.Current;
            _parser.ParseBlock(stmt);
            this.Ctx.Symbols.Pop();
        }


        /// <summary>
        /// Parses a conditional block by first pushing symbol scope and then popping after completion.
        /// </summary>
        /// <param name="stmt"></param>
        public virtual void ParseConditionalBlock(ConditionalBlockStmt stmt)
        {
            this.Ctx.Symbols.Push(new SymbolsNested(string.Empty), true);
            stmt.SymScope = this.Ctx.Symbols.Current;
            _parser.ParseConditionalStatement(stmt);
            this.Ctx.Symbols.Pop();
        }
    }
}
