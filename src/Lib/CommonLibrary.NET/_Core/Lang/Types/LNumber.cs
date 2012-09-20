﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LNumber : LObject
    {   
        /// <summary>
        /// Initialize bool value.
        /// </summary>
        /// <param name="value"></param>
        public LNumber(bool value)
        {
            _value = value;
            DataType = typeof(double);
        }


        /// <summary>
        /// Get boolean value.
        /// </summary>
        /// <returns></returns>
        public double ToValue()
        {
            return (double)_value;
        }
    }
}