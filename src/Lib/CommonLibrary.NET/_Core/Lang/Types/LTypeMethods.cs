﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.Helpers;
// </lang:using>

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Base class for methods on types.
    /// </summary>
    public class LTypeMethods : ITypeMethods
    {
        /// <summary>
        /// A mapping between the method for the datatype in the language to the 
        /// implemented method in this host language.
        /// </summary>
        public class MappedMethod
        {
            /// <summary>
            /// The method name in the language
            /// </summary>
            public string DataTypeMethod;


            /// <summary>
            /// The method name in the host language.
            /// </summary>
            public string HostLanguageMethod;


            /// <summary>
            /// The function metadata for the language types method.
            /// </summary>
            public FunctionMetaData FuncDef;


            /// <summary>
            /// Whether or not this supports a getter property if this is a property
            /// </summary>
            public bool AllowGet;


            /// <summary>
            /// Whether or not this supports a setter property if this is a property
            /// </summary>
            public bool AllowSet;
        }



        /// <summary>
        /// A mapping of the defined members including properties for this type
        /// </summary>
        protected IDictionary<string, MemberTypes> _allMembersMap = new Dictionary<string, MemberTypes>();


        /// <summary>
        /// A mapping of the all the method names associated with this type.
        /// </summary>
        protected IDictionary<string, MappedMethod> _methodMap = new Dictionary<string, MappedMethod>();



        /// <summary>
        /// The datatype this methods class supports.
        /// </summary>
        public LType DataType { get; set; }


        /// <summary>
        /// Initailize the function mappings.
        /// </summary>
        public virtual void Init()
        {
        }


        /// <summary>
        /// Creates functionmetadata object with the supplied inputs.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="implementationMethod">The method that implements the funcion in the methods implementation class.</param>
        /// <param name="returnType">The return values type</param>
        /// <param name="description">Description of the function.</param>
        /// <returns></returns>
        public FunctionMetaData AddMethod(string name, string implementationMethod, Type returnType, string description)
        {            
            return this.AddMethodInfo(MemberTypes.Method, name, implementationMethod, returnType, description);
        }


        /// <summary>
        /// Adds a new property
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="implementationMethod">The method implementing this property in the methods implementation class</param>
        /// <param name="returnType">The return type of the property</param>
        /// <param name="description">A description of the property</param>
        public void AddProperty(bool allowGet, bool allowSet, string name, string implementationMethod, Type returnType, string description)
        {            
            this.AddMethodInfo(MemberTypes.Property, name, implementationMethod, returnType, description);
        }


        /// <summary>
        /// Initailizes
        /// </summary>
        /// <param name="funcName">The name of the function to add the argument info to.</param>
        /// <param name="name">Name of the arg</param>
        /// <param name="desc">Description of the arg</param>
        /// <param name="alias">Alias for the arg</param>
        /// <param name="type">Datatype of the arg</param>
        /// <param name="required">Whether or not arg is required</param>
        /// <param name="defaultVal">Default value of arg</param>
        /// <param name="examples">Examples of arg</param>
        public void AddArg(string funcName, string name, string type, bool required, string alias, object defaultVal, string examples, string desc)
        {
            this._methodMap[funcName].FuncDef.AddArg(name, type, required, alias, defaultVal, examples, desc);
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied member.
        /// </summary>
        /// <param name="type">The data type to check for the member</param>
        /// <param name="memberName">The name of the member to check for.</param>
        /// <returns></returns>
        public virtual bool HasMember(LTypeValue type, string memberName)
        {
            return _allMembersMap.ContainsKey(memberName);
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied method.
        /// </summary>
        /// <param name="type">The data type to check for the method</param>
        /// <param name="methodName">The name of the method to check for.</param>
        /// <returns></returns>
        public virtual bool HasMethod(LTypeValue type, string methodName)
        {
            if (!_allMembersMap.ContainsKey(methodName)) return false;
            var member = _allMembersMap[methodName];
            return member == MemberTypes.Method;
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied property.
        /// </summary>
        /// <param name="type">The data type to check for the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        public virtual bool HasProperty(LTypeValue type, string propertyName)
        {
            if (!_allMembersMap.ContainsKey(propertyName)) return false;
            var member = _allMembersMap[propertyName];
            return member == MemberTypes.Property;
        }


        /// <summary>
        /// Gets the property value for the specified propertyname.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public virtual object GetProperty(LObject type, string propName)
        {
            return null;
        }


        /// <summary>
        /// Sets the property value for the specified propertyname.
        /// </summary>
        /// <param name="type">The object to set the property value on</param>
        /// <param name="propName">The name of the property</param>
        /// <param name="val">The value to set on the property</param>
        /// <returns></returns>
        public virtual void SetProperty(LObject type, string propName, object val)
        {
        }



        /// <summary>
        /// Validates the method call.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual ComLib.Lang.Core.BoolMsgObj ValidateCall(LTypeValue type, string methodName, object[] parameters)
        {
            // 1. Valid method/member name?
            if (!this._methodMap.ContainsKey(methodName))
                return new BoolMsgObj(type, false, "The method name : " + methodName + " does not exist for this type");

            // 2. Valid method parameters?
            var mappedMethod = _methodMap[methodName];
            var funcDef = mappedMethod.FuncDef;
            var ndx = 0;
            var isValid = true;
            var message = string.Empty;
            foreach (var arg in funcDef.Arguments)
            {
                if (arg.Required && ndx >= parameters.Length)
                {
                    isValid = false;
                    message = "Required argument : " + arg.Name + " was not supplied";
                    break;
                }
                var param = parameters[ndx];
                ndx++;
            }
            return new BoolMsgObj(type, isValid, message);
        }


        /// <summary>
        /// Executes the method supplied on the the type.
        /// </summary>
        /// <param name="type">The language type</param>
        /// <param name="methodName">The method name</param>
        /// <param name="parameters">The parameters to the method.</param>
        /// <returns></returns>
        public virtual object ExecuteMethod(LTypeValue type, string methodName, object[] parameters)
        {
            var mappedMethod = _methodMap[methodName];
            var args = new ArgsFetcher(parameters);

            // total required = 
            var funcDef = mappedMethod.FuncDef;
            int total = funcDef.GetTotalRequiredArgs();
            var methodArgs = new List<object>();

            methodArgs.Add(type.Result);

            // TODO: Figure out the total required args when AddArg is called.
            if (total > 0)
            {
                var ndx = 0;
                var totalParamsGiven = parameters.Length;

                // Go through all the argument definitions.
                foreach(var arg in funcDef.Arguments)
                {
                    var isRequired = arg.Required;
                    // 1. Required and provided?
                    if (isRequired && ndx < parameters.Length)
                    {
                        // Positional arg.
                        if (arg.Type != "params")
                        {
                            var param = parameters[ndx];
                            methodArgs.Add(param);
                        }
                        // End of list arguments.
                        else
                        {
                            var remainder = new List<object>();
                            while (ndx < totalParamsGiven)
                            {
                                remainder.Add(parameters[ndx]);
                                ndx++;
                            }
                            methodArgs.Add(remainder.ToArray());
                        }
                    }
                    // 2. Not required but supplied.
                    else if (!isRequired && ndx < parameters.Length)
                        methodArgs.Add(parameters[ndx]);

                    // 3. Not required but there is a default.
                    else if (!isRequired && arg.DefaultValue != null && ndx >= parameters.Length)
                        methodArgs.Add(arg.DefaultValue);
                    ndx++;
                }                    
            }  
          
            
            var methodParams = methodArgs.ToArray();
            var method = this.GetType().GetMethod(mappedMethod.HostLanguageMethod);
            object result = method.Invoke(this, methodParams);
            return result;
        }


        /// <summary>
        /// Creates functionmetadata object with the supplied inputs.
        /// </summary>
        /// <param name="memberType">What type of member e.g. property,function.</param>
        /// <param name="name">The name of the function</param>
        /// <param name="implementationMethod">The method that implements the funcion in this host language.</param>
        /// <param name="returnType">The return values type</param>
        /// <param name="description">Description of the function.</param>
        /// <returns></returns>
        private FunctionMetaData AddMethodInfo(MemberTypes memberType, string name, string implementationMethod, Type returnType, string description)
        {
            var funcdef = new FunctionMetaData(name, null);
            funcdef.Doc = new Docs.DocTags();
            funcdef.ReturnType = returnType;
            funcdef.Doc.Summary = description;

            var mappedMethod = new MappedMethod();
            mappedMethod.DataTypeMethod = name;
            mappedMethod.HostLanguageMethod = implementationMethod;
            mappedMethod.FuncDef = funcdef;              
            _methodMap[name] = mappedMethod;

            _allMembersMap[name] = memberType;
            return funcdef;
        }
    }
}