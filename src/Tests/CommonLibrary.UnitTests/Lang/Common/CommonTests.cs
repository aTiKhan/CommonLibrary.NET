﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;
using ComLib.Lang.Extensions;

namespace ComLib.Lang.Tests.Common
{
    public class TestCases
    {
        public string Name { get; set; }
        public Type[] RequiredTypes { get; set; }
        public Type[] RequiredPlugins { get; set; }
        public List<Tuple<string, Type, object, string>> Positive { get; set; }
    }


    public enum TestType
    {
        Unit,

        Component,

        Integration,

        System
    }


    public class CommonTestCases_Plugins
    {
        protected static new Tuple<string, Type, object, string> TestCase(string resultVarName, Type resultType, object resultValue, string script)
        {
            return new Tuple<string, Type, object, string>
                (resultVarName, resultType, resultValue, script);                
        }


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases AliasInScript = new TestCases()
        {
            Name = "AliasInScript Plugin",
            RequiredPlugins = new[] { typeof(AliasPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {                
                TestCase("result", typeof(double), 1,     "alias set to var; set result = 1;"),
                TestCase("result", typeof(double), 1,     "alias set to var; set result = 0; function test()     {  return 1;         } result = test();"),
                TestCase("result", typeof(double), 2,     "alias set to var; set result = 1; function test(a)    { return a + 1;      } result = test(1);"),
                TestCase("result", typeof(double), 4,     "alias set to var; set result = 2; function test(a)    { return a + result; } result = test(2);"),
                TestCase("result", typeof(bool), true,    "alias set to var; set result = 1; function test(a)    { return true;       } result = test(1);"),
                TestCase("result", typeof(bool), false,   "alias set to var; set result = 1; function test(a)    { return false;      } result = test(1);"),
                TestCase("result", typeof(double), 5,     "alias set to var; set result = 1; function test(a, b) { return a + b;      } result = test(2,3);"),
                TestCase("result", typeof(double), 3,     "alias set to var; set result = 1; function test(a, b) { return a - b;      } result = test(4,1);"),
                TestCase("result", typeof(string), "com", "alias set to var; set result = 1; function test()     { return 'com';      } result = test();"),                
                TestCase("result", typeof(double), 1,     "alias def to function; var result = 0; def test()     {  return 1;         } result = test();"),
                TestCase("result", typeof(double), 2,     "alias def to function; var result = 1; def test(a)    { return a + 1;      } result = test(1);"),
                TestCase("result", typeof(double), 4,     "alias def to function; var result = 2; def test(a)    { return a + result; } result = test(2);"),
                TestCase("result", typeof(bool), true,    "alias def to function; var result = 1; def test(a)    { return true;       } result = test(1);"),
                TestCase("result", typeof(bool), false,   "alias def to function; var result = 1; def test(a)    { return false;      } result = test(1);"),
                TestCase("result", typeof(double), 5,     "alias def to function; var result = 1; def test(a, b) { return a + b;      } result = test(2,3);"),
                TestCase("result", typeof(double), 3,     "alias def to function; var result = 1; def test(a, b) { return a - b;      } result = test(4,1);"),
                TestCase("result", typeof(string), "com", "alias def to function; var result = 1; def test()     { return 'com';      } result = test();"),                
            }
        };



        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        private static string Aggregate_List = "var items = [1, 2, 3, 4, 5, 6, 7, 8, 9];";
        public static TestCases Aggregate = new TestCases()
        {
            Name = "Aggregate Plugin",
            RequiredPlugins = new[] { typeof(AggregatePlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {                
                TestCase("a", typeof(double), 44, Aggregate_List + "var a = sum(items) - min(items);"), 
                TestCase("a", typeof(double), 10,                  "var a = sum of [4, 3, 1, 2];"), 
                TestCase("a", typeof(double), 1,  Aggregate_List + "var a = min( items );"), 
                TestCase("a", typeof(double), 9,  Aggregate_List + "var a = max( items );"), 
                TestCase("a", typeof(double), 45, Aggregate_List + "var a = sum( items );"), 
                TestCase("a", typeof(double), 5,  Aggregate_List + "var a = avg( items );"), 
                TestCase("a", typeof(double), 9,  Aggregate_List + "var a = count( items );"), 
                TestCase("a", typeof(double), 9,  Aggregate_List + "var a = number( items );"),
                TestCase("a", typeof(double), 1,  Aggregate_List + "var a = min    of items ;"), 
                TestCase("a", typeof(double), 9,  Aggregate_List + "var a = max    of items ;"), 
                TestCase("a", typeof(double), 45, Aggregate_List + "var a = sum    of items ;"), 
                TestCase("a", typeof(double), 5,  Aggregate_List + "var a = avg    of items ;"), 
                TestCase("a", typeof(double), 9,  Aggregate_List + "var a = count  of items ;"), 
                TestCase("a", typeof(double), 9,  Aggregate_List + "var a = number of items ;"),
                TestCase("a", typeof(double), 3,  Aggregate_List  + "var a = 1; a = min    of items + 2;"), 
                TestCase("a", typeof(double), 11, Aggregate_List  + "var a = 1; a = max    of items + 2;"), 
                TestCase("a", typeof(double), 47, Aggregate_List  + "var a = 1; a = sum    of items + 2;"), 
                TestCase("a", typeof(double), 7,  Aggregate_List  + "var a = 1; a = avg    of items + 2;"), 
                TestCase("a", typeof(double), 11, Aggregate_List  + "var a = 1; a = count  of items + 2;"), 
                TestCase("a", typeof(double), 11, Aggregate_List  + "var a = 1; a = number of items + 2;") 
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Bool = new TestCases()
        {
            Name = "Bool Plugin",
            RequiredPlugins = new[] { typeof(BoolPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("a", typeof(bool), true, "var a = yes;"), 
                TestCase("a", typeof(double), 2, "var a = 1; var b = true;  if ( b == on  ) a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 1; var b = true;  if ( b == yes ) a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 1; var b = false; if ( b == off ) a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 1; var b = false; if ( b == no  ) a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 1; var b = true;  if ( b == On  ) a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 1; var b = true;  if ( b == Yes ) a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 1; var b = false; if ( b == Off ) a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 1; var b = false; if ( b == No  ) a = 2;")
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Compare = new TestCases()
        {
            Name = "Compare Plugin",
            RequiredPlugins = new[] { typeof(ComparePlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("a", typeof(double), 2, "var a = 3; if ( a is 3           )    a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 3; if ( a is equal 3     )    a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 3; if ( a is equal to 3  )    a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 3; if ( a equals 3       )    a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 3; if ( a equal to 3     )    a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 4; if ( a not 3          )    a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 4; if ( a not equal 3    )    a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 4; if ( a not equal to 3 )    a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 4; if ( a is not 3       )    a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 4; if ( a is not equal to 3 ) a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 1; if ( a less than 4 )       a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 1; if ( a is before 4 )       a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 1; if ( a is below  4 )       a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 1; if ( a less than equal 4 ) a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 5; if ( a more than 4 )       a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 5; if ( a is after  4 )       a = 2;"), 
                TestCase("a", typeof(double), 2, "var a = 5; if ( a is above  4 )       a = 2;"),
                TestCase("a", typeof(double), 2, "var a = 5; if ( a more than equal 4 ) a = 2;"),
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        private static string Date_YearNow = DateTime.Now.Year.ToString();
        public static TestCases Date = new TestCases()
        {
            Name = "Date Plugin",
            RequiredPlugins = new[] { typeof(DatePlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2 2011 at 9:30 am;     if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2 2011 at 10:45:20 pm; if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2 2011 at 09:45:20 pm; if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2 2011 at 12pm;        if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = oct 2;              if (date.getFullYear() == " + Date_YearNow + " && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = oct 2nd;            if (date.getFullYear() == " + Date_YearNow + " && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2;          if (date.getFullYear() == " + Date_YearNow + " && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2nd;        if (date.getFullYear() == " + Date_YearNow + " && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = oct 2 2011;         if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = oct 2nd 2011;       if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = oct 2, 2011;        if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = oct 2nd, 2011;      if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2 2011;     if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2nd 2011;   if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2, 2011;    if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = october 2nd, 2011;  if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;")
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        private static string   DateNumber_YearNow = DateTime.Now.Year.ToString();
        private static DateTime DateNumber_Dt = new DateTime(2011, 10, 2);
        public static TestCases DateNumber = new TestCases()
        {
            Name = "DateNumber Plugin",
            RequiredPlugins = new[] { typeof(DateNumberPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("i", typeof(DateTime), DateNumber_Dt, "var i = 10/2/2011"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = 10/2/2011;    if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = 10\\2\\2011;  if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var date = 10-2-2011;    if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;")
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Def = new TestCases()
        {
            Name = "Def Plugin",
            RequiredPlugins = new[] { typeof(DefPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {                
                TestCase("result", typeof(double), 1,     "var result = 0; def test()     {  return 1;         } result = test();"),
                TestCase("result", typeof(double), 2,     "var result = 1; def test(a)    { return a + 1;      } result = test(1);"),
                TestCase("result", typeof(double), 4,     "var result = 2; def test(a)    { return a + result; } result = test(2);"),
                TestCase("result", typeof(bool), true,    "var result = 1; def test(a)    { return true;       } result = test(1);"),
                TestCase("result", typeof(bool), false,   "var result = 1; def test(a)    { return false;      } result = test(1);"),
                TestCase("result", typeof(double), 5,     "var result = 1; def test(a, b) { return a + b;      } result = test(2,3);"),
                TestCase("result", typeof(double), 3,     "var result = 1; def test(a, b) { return a - b;      } result = test(4,1);"),
                TestCase("result", typeof(string), "com", "var result = 1; def test()     { return 'com';      } result = test();")
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Email = new TestCases()
        {
            Name = "Email Plugin",
            RequiredPlugins = new[] { typeof(EmailPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), "john.doe@company.com", "var result = john.doe@company.com"),
                TestCase("result", typeof(string), "bat.man.2002@gotham.com", "var result = bat.man.2002@gotham.com"),
                TestCase("result", typeof(string), "super_man@metropolis.com", "var result = super_man@metropolis.com"),
                TestCase("result", typeof(string), "superman@metropolis.com", "var result = superman@metropolis.com")
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases HashComment = new TestCases()
        {
            Name = "HashComment Plugin",
            RequiredPlugins = new[] { typeof(HashCommentPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double), 1,  "# comment here\r\n var result = 1;"),
                TestCase("result", typeof(double), 2,  "# comment here\r var result = 2;")
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Holiday = new TestCases()
        {
            Name = "Holiday Plugin",
            RequiredPlugins = new[] { typeof(HolidayPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("a", typeof(DateTime), new DateTime(2010, 12, 31),              "var a = New Years Eve 2010;"),
                TestCase("a", typeof(DateTime), new DateTime(DateTime.Now.Year, 12, 31), "var a = New Years Eve;"),
                TestCase("a", typeof(DateTime), new DateTime(DateTime.Now.Year, 12, 25), "var a = Christmas;"), 
                TestCase("a", typeof(DateTime), new DateTime(DateTime.Now.Year, 1, 1),   "var a = New Years;"),                
                TestCase("a", typeof(DateTime), new DateTime(DateTime.Now.Year, 7, 4),   "var a = Independence Day;"), 
                TestCase("a", typeof(DateTime), new DateTime(DateTime.Now.Year, 12, 24), "var a = Christmas Eve;")
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        private static string MachineInfo_username = Environment.UserName;
        private static string MachineInfo_compname = Environment.MachineName;
        private static string MachineInfo_domain   = Environment.UserDomainName;
        private static string MachineInfo_osver    = Environment.OSVersion.Version.ToString();
        public static TestCases MachineInfo = new TestCases()
        {
            Name = "MachineInfo Plugin",
            RequiredPlugins = new[] { typeof(MachineInfoPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), MachineInfo_compname,   "var result = @machine" ),
                TestCase("result", typeof(string), MachineInfo_domain,     "var result = @domain" ),
                TestCase("result", typeof(string), MachineInfo_username,   "var result = @user" ),
                TestCase("result", typeof(string), MachineInfo_compname,   "var result = mac.machine" ),
                TestCase("result", typeof(string), MachineInfo_domain,     "var result = mac.domain" ),
                TestCase("result", typeof(string), MachineInfo_username,   "var result = mac.user" ),
                TestCase("result", typeof(string), MachineInfo_osver,      "var result = mac.osversion" ),
                TestCase("result", typeof(string), MachineInfo_compname,   "var result = mac machine" ),
                TestCase("result", typeof(string), MachineInfo_domain,     "var result = mac domain" ),
                TestCase("result", typeof(string), MachineInfo_username,   "var result = mac user" ),
            }
        };
            


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Money = new TestCases()
        {
            Name = "Money Plugin",
            RequiredPlugins = new[] { typeof(MoneyPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("a", typeof(double), 12.45, "var a = 2; function add(b){ return b + 2; } a = add($10.45);"), 
                TestCase("a", typeof(double), 10.45, "var a = $10.45;"),               
                TestCase("a", typeof(double), 24.50, "var a = 0; var salary = $24.50; if salary > $16.85 then a = $24.50;"),   
            }
        };
            


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Percent = new TestCases()
        {
            Name = "Percent Plugin",
            RequiredPlugins = new[] { typeof(PercentPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double), .25,  "var result = 25%;"),
                TestCase("result", typeof(double), .25,  "var result = 25 %;"),
                TestCase("result", typeof(double),  2,   "var result = 20 % 6;"),
                TestCase("result", typeof(double),  2,   "var b = 3; var result = 20 % b;"),
                TestCase("result", typeof(double), .25,  "var items = [0, 25%, 5]; var result = items[1];")
                
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Repeat = new TestCases()
        {
            Name = "Repeat Plugin",
            RequiredPlugins = new[] { typeof(RepeatPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(double), 10, "result = 0; repeat to 10                 { result = it;  }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 9,  "result = 0; repeat to 10 by 2            { result = it;  }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 9,  "result = 0; repeat to < 10               { result = it;  }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 9,  "result = 0; repeat to < 10 by 2          { result = it;  }"), 
                                                                                                                     
                new Tuple<string, Type, object, string>("result", typeof(double), 15, "result = 0; repeat 1 to 15               { result = it;  }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 14, "result = 0; repeat 2 to 15 by 2          { result = it;  }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 16, "result = 0; repeat 3 to < 17             { result = it;  }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 18, "result = 0; repeat 4 to < 19 by 2        { result = it;  }"),

                new Tuple<string, Type, object, string>("result", typeof(double), 20, "result = 0; repeat ndx to 20             { result = ndx; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 22, "result = 0; repeat ndx to 23 by 2        { result = ndx; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 24, "result = 0; repeat ndx to < 25           { result = ndx; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 28, "result = 0; repeat ndx to < 30 by 4      { result = ndx; }"), 
           
                new Tuple<string, Type, object, string>("result", typeof(double), 20, "result = 0; repeat ndx = 10 to 20        { result = ndx; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 22, "result = 0; repeat ndx = 12 to 22 by 2   { result = ndx; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 24, "result = 0; repeat ndx = 14 to < 25      { result = ndx; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 26, "result = 0; repeat ndx = 18 to < 30 by 4 { result = ndx; }"),

                new Tuple<string, Type, object, string>("result", typeof(double), 26, "result = 0; var a = 18; var b = 30; var c = 4; repeat ndx = a to < b by c { result = ndx; }"),

                
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Round = new TestCases()
        {
            Name = "Round Plugin",
            RequiredPlugins = new[] { typeof(RoundPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("a", typeof(double), 2.0, "var a = round 2.3;"),
                TestCase("a", typeof(double), 2.0, "var a = round 2.4;"),
                TestCase("a", typeof(double), 3.0, "var a = round 2.5;"),
                TestCase("a", typeof(double), 3.0, "var a = round up 2.3;"),
                TestCase("a", typeof(double), 2.0, "var a = round down 2.3;"), 
                TestCase("a", typeof(double), 3.0, "var b = 2.3; var a = round up   b;"),
                TestCase("a", typeof(double), 2.0, "var b = 2.3; var a = round down b;"),
                TestCase("a", typeof(double), 3.0, "var b = [1.5, 2.3]; var a = round up   b[1];"),
                TestCase("a", typeof(double), 2.0, "var b = { val: 2.3}; var a = round down b.val;"),
            }
        };
            

        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Run = new TestCases()
        {
            Name = "Run Plugin",
            RequiredPlugins = new[] { typeof(RunPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double), 2, "var result = 1; function 'add 1'    { result = result + 1; } run 'add 1'; "),
                TestCase("result", typeof(double), 2, "var result = 1; function  add1      { result = result + 1; } run  add1; "),
                TestCase("result", typeof(double), 2, "var result = 1; function 'add 1'    { result = result + 1; } run 'add 1'(); "),
                TestCase("result", typeof(double), 3, "var result = 1; function 'add 2'    { result = result + 2; } run function 'add 2'(); "),
                TestCase("result", typeof(double), 4, "var result = 1; function 'add 3'    { return result + 3; } result = run 'add 3'(); "),
                TestCase("result", typeof(double), 5, "var result = 1; function 'add 4'    { return result + 4; } result = run function 'add 4'(); "),
                
                TestCase("result", typeof(double), 4, "var result = 0; function 'add 3'(a) { return a + 3;   } result = run 'add 3'(1); "),
                TestCase("result", typeof(double), 4, "var result = 0; function 'add 3'(a) { return a + 3;   } result = run function 'add 3'(1); "),
                TestCase("result", typeof(double), 2, "var result = 0; function 'add 1'(a) { result = a + 1; } run 'add 1'(1); "),
                TestCase("result", typeof(double), 4, "var result = 0; function 'add 3'(a) { result = a + 3; } run function 'add 3'(1); ")                               
            }
        };
        

        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Set = new TestCases()
        {
            Name = "Set Plugin",
            RequiredPlugins = new[] { typeof(SetPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double), 1,     "set result = 0; function test()     {  return 1;         } result = test();"),
                TestCase("result", typeof(double), 2,     "set result = 1; function test(a)    { return a + 1;      } result = test(1);"),
                TestCase("result", typeof(double), 4,     "set result = 2; function test(a)    { return a + result; } result = test(2);"),
                TestCase("result", typeof(bool), true,    "set result = 1; function test(a)    { return true;       } result = test(1);"),
                TestCase("result", typeof(bool), false,   "set result = 1; function test(a)    { return false;      } result = test(1);"),
                TestCase("result", typeof(double), 5,     "set result = 1; function test(a, b) { return a + b;      } result = test(2,3);"),
                TestCase("result", typeof(double), 3,     "set result = 1; function test(a, b) { return a - b;      } result = test(4,1);"),
                TestCase("result", typeof(string), "com", "set result = 1; function test()     { return 'com';      } result = test();")
            }
        };
        
        
        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases TypeOf = new TestCases()
        {
            Name = "TypeOf Plugin",
            RequiredTypes = new[] { typeof(User) },
            RequiredPlugins = new[] { typeof(TypeOfPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), "string",                                  "var result = typeof 'fluentscript'"),
                TestCase("result", typeof(string), "number",                                  "var result = typeof 12"),
                TestCase("result", typeof(string), "number",                                  "var result = typeof 12.34"),
                TestCase("result", typeof(string), "boolean",                                 "var result = typeof true"),
                TestCase("result", typeof(string), "boolean",                                 "var result = typeof false"),
                TestCase("result", typeof(string), "datetime",                                "var result = typeof new Date(2012, 8, 10)"),
                TestCase("result", typeof(string), "time",                                    "var result = typeof new Time(8, 30, 10)"),                
                TestCase("result", typeof(string), "object:list",                             "var result = typeof [0, 1, 2]"),
                TestCase("result", typeof(string), "object:map",                              "var result = typeof { one: 1, two: 2 }"),                
                TestCase("result", typeof(string), "function:inc",                            "function inc(a){ return a + 1; } result = typeof inc"),
                TestCase("result", typeof(string), "object:ComLib.Lang.Tests.Common.User",    "var user = new User('john'); result = typeof user"),

                new Tuple<string, Type, object, string>("result", typeof(string), "string",     		                    "var item = 'fluentscript'			; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "number",     		                    "var item = 12						; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "number",     		                    "var item = 12.34					; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "boolean",    		                    "var item = true					; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "boolean",    		                    "var item = false					; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "datetime",   		                    "var item = new Date(2012, 8, 10)	; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "time",       		                    "var item = new Time(8, 30, 10)		; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "object:list",		                    "var item = [0, 1, 2]				; var result = typeof item"),
                new Tuple<string, Type, object, string>("result", typeof(string), "object:map", 		                    "var item = { one: 1, two: 2 }		; var result = typeof item"),
            }
        }; 
      

        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Time = new TestCases()
        {
            Name = "Time Plugin",
            RequiredPlugins = new[] { typeof(TimePlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("i", typeof(double), 1, "var i = 0; var time = 730 am;      if (time.Hours == 7  && time.Minutes == 30  && time.Seconds == 0 ) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time = 1030 am;     if (time.Hours == 10 && time.Minutes == 30  && time.Seconds == 0 ) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time = 1030pm;      if (time.Hours == 22 && time.Minutes == 30  && time.Seconds == 0 ) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time = 12pm;        if (time.Hours == 12 && time.Minutes == 0   && time.Seconds == 0 ) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time = 9:30 am;     if (time.Hours == 9  && time.Minutes == 30  && time.Seconds == 0 ) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time = 10:45:20 pm; if (time.Hours == 22 && time.Minutes == 45  && time.Seconds == 20) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time = 09:45:32 pm; if (time.Hours == 21 && time.Minutes == 45  && time.Seconds == 32) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time = noon;        if (time.Hours == 12 && time.Minutes == 0   && time.Seconds == 0 ) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time1 = 9am; var time2 = 11am;  if (time1 < time2 ) i = 1;"),
                TestCase("i", typeof(double), 1, "var i = 0; var time1 = 12:30pm; if (time1 == 12:30pm ) i = 1;"),
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases TypeOps = new TestCases()
        {
            Name = "Time Plugin",
            RequiredPlugins = new[] { typeof(TypeOperationsPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("i", typeof(bool),   true,  "var i = is_number( 123 );"),
                TestCase("i", typeof(bool),   false, "var i = is_number( '123' );"),
                TestCase("i", typeof(bool),   true,  "var i = is_number( 123.45 );"),
                TestCase("i", typeof(bool),   false, "var i = is_number( '123.45' );"),                
                
                TestCase("i", typeof(bool),   true,  "var i = is_number_like( 123   );"),
                TestCase("i", typeof(bool),   true,  "var i = is_number_like( '123' );"),
                TestCase("i", typeof(bool),   true,  "var i = is_number_like( 123.45);"),
                TestCase("i", typeof(bool),   true,  "var i = is_number_like( true  );"),
                TestCase("i", typeof(bool),   true,  "var i = is_number_like( false );"),
                
                TestCase("i", typeof(double), 123,    "var i = to_number( 123   );"),
                TestCase("i", typeof(double), 123,    "var i = to_number( '123' );"),
                TestCase("i", typeof(double), 123.45, "var i = to_number( 123.45);"),
                TestCase("i", typeof(double), 1,      "var i = to_number( true  );"),
                TestCase("i", typeof(double), 0,      "var i = to_number( false );"),

                TestCase("i", typeof(bool),   true,  "var i = is_bool( true   );"),
                TestCase("i", typeof(bool),   false, "var i = is_bool( 'true' );"),
                TestCase("i", typeof(bool),   true,  "var i = is_bool( false  );"),
                TestCase("i", typeof(bool),   false, "var i = is_bool( 'false');"),

                TestCase("i", typeof(bool),   true,  "var i = is_bool_like( 123   );"),
                TestCase("i", typeof(bool),   true,  "var i = is_bool_like( '123' );"),
                TestCase("i", typeof(bool),   true,  "var i = is_bool_like( 123.45);"),
                TestCase("i", typeof(bool),   true,  "var i = is_bool_like( true  );"),
                TestCase("i", typeof(bool),   true,  "var i = is_bool_like( false );"),
                
                TestCase("i", typeof(bool),   true,  "var i = to_bool( true   );"),
                TestCase("i", typeof(bool),   true,  "var i = to_bool( 'true' );"),
                TestCase("i", typeof(bool),   false, "var i = to_bool( false  );"),
                TestCase("i", typeof(bool),   false, "var i = to_bool( 'false');"),
                TestCase("i", typeof(bool),   true,  "var i = to_bool( 'yes');"),
                TestCase("i", typeof(bool),   true,  "var i = to_bool( 'on');"),
                TestCase("i", typeof(bool),   true,  "var i = to_bool( 'ok');"),
                TestCase("i", typeof(bool),   true,  "var i = to_bool( '1');"),
                TestCase("i", typeof(bool),   false, "var i = to_bool( 'no');"),
                TestCase("i", typeof(bool),   false, "var i = to_bool( 'off');"),
                TestCase("i", typeof(bool),   false, "var i = to_bool( 'not ok');"),
                TestCase("i", typeof(bool),   false, "var i = to_bool( '0');"),
                TestCase("i", typeof(bool),   false, "var i = to_bool( 0);"),


                TestCase("i", typeof(bool),       true,  "var i = is_date( new Date(2012, 9, 1)   );"),
                TestCase("i", typeof(bool),       false, "var i = is_date( '9/1/2012' );"),
                TestCase("i", typeof(bool),       false, "var i = is_date( '09/01/2012' );"),
                
                TestCase("i", typeof(bool),   false,  "var i = is_date_like( 123   );"),
                TestCase("i", typeof(bool),   false,  "var i = is_date_like( '123' );"),
                TestCase("i", typeof(bool),   false,  "var i = is_date_like( 123.45);"),
                TestCase("i", typeof(bool),   false,  "var i = is_date_like( true  );"),
                TestCase("i", typeof(bool),   false,  "var i = is_date_like( false );"),
                
                TestCase("i", typeof(DateTime),   new DateTime(2012, 9, 1),  "var i = to_date( new Date(2012, 9, 1)   );"),
                TestCase("i", typeof(DateTime),   new DateTime(2012, 9, 1),  "var i = to_date( '9/1/2012' );"),
                TestCase("i", typeof(DateTime),   new DateTime(2012, 9, 1),  "var i = to_date( '09/01/2012' );"),

                TestCase("i", typeof(bool),       true,  "var i = is_time( new Time(8, 30, 0)   );"),
                TestCase("i", typeof(bool),       false, "var i = is_time( '8:30' );"),
                TestCase("i", typeof(TimeSpan),   new TimeSpan(8, 30, 0),  "var i = to_time( new Time(8, 30, 0)   );"),
                TestCase("i", typeof(TimeSpan),   new TimeSpan(8, 30, 0),  "var i = to_time( '8:30' );"),
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases Uri = new TestCases()
        {
            Name = "Uri Plugin",
            RequiredTypes = new[] { typeof(User) },
            RequiredPlugins = new[] { typeof(UriPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), "c:\\user\\settings.xml",  "var result = c:\\user\\settings.xml;"),
                TestCase("result", typeof(string), "c:/user/settings.xml",  "var result = c:/user/settings.xml;"),
                TestCase("result", typeof(string), "www.yahoo.com",         "var result = www.yahoo.com;"),
                TestCase("result", typeof(string), "http://www.yahoo.com",  "var result = http://www.yahoo.com;"),
                TestCase("result", typeof(string), "https://www.yahoo.com",  "var result = https://www.yahoo.com;"),
                TestCase("result", typeof(string), "ftp://www.yahoo.com",  "var result = ftp://www.yahoo.com;"),
                TestCase("result", typeof(string), "http://www.yahoo.com?user=kishore%20&id=123",  "var result = http://www.yahoo.com?user=kishore%20&id=123 ;"),                
            }
        };


        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases VarPath = new TestCases()
        {
            Name = "VarPath Plugin",
            RequiredPlugins = new[] { typeof(VariablePathPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), "c:\\myapp\\version\\build\\fs.xml",               "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version\\build\\@script.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version\\build\\fs-build.xml",         "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version\\build\\@script-build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version\\build\\fs.build.xml",         "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version\\build\\@script.build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version\\build\\fsbuild.xml",          "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version\\build\\@{script}build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version\\build\\fs-build.xml",         "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version\\build\\@{script}-build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version\\build\\fs.build.xml",         "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version\\build\\@{script}.build.xml"),

                TestCase("result", typeof(string), "c:\\myapp\\2.0\\build\\fsbuild.xml",              "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\@version\\build\\@{script}build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\2.0-version\\build\\fs-build.xml",     "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\@version-version\\build\\@{script}-build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\2.0.version\\build\\fs.build.xml",     "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\@version.version\\build\\@{script}.build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version-2.0\\build\\fs-build.xml",     "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version-@version\\build\\@{script}-build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version.2.0\\build\\fs.build.xml",     "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version.@version\\build\\@{script}.build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version2.0\\build\\fs.build.xml",      "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\version@version\\build\\@{script}.build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\2.0-version\\build\\fs-build.xml",     "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\@{version}-version\\build\\@{script}-build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\2.0.version\\build\\fs.build.xml",     "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\@{version}.version\\build\\@{script}.build.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\2.0version\\build\\fs.build.xml",      "var script = 'fs'; version = '2.0'; var home = 'c:\\\\myapp'; var result = @home\\@{version}version\\build\\@{script}.build.xml"),
                
                TestCase("result", typeof(string), "c:\\myapp\\build\\script.xml",         "var home = 'c:\\\\myapp'; var result = home\\build\\script.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\app-build\\script.xml",     "var home = 'c:\\\\myapp'; var result = home\\app-build\\script.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\app_build\\script.xml",     "var home = 'c:\\\\myapp'; var result = home\\app_build\\script.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\app-build12\\script.xml",   "var home = 'c:\\\\myapp'; var result = home\\app-build12\\script.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version_1.2.3\\script.xml", "var home = 'c:\\\\myapp'; var result = home\\version_1.2.3\\script.xml"),
                TestCase("result", typeof(string), "c:\\myapp\\version_1.2.3\\script.xml", "var home = 'c:\\\\myapp'; var result = @home\\version_1.2.3\\script.xml"),
            }
        };



        private static string Records_Extra = "var num = 20; var name = 'catcher'; var active = true; function add(a, b) { return a + b; }";
        private static string Records_List1 = Records_Extra + " var books = [ name | pages | isactive \r\n name + ' rye' | num + 100 | true \r\n 'book 1' | add( num, 110) | active \r\n 'book 1' | ( 20 * 5 ) + add(20, num) | !active \r\n ];";
        public static TestCases Records = new TestCases()
        {
            Name = "Records Plugin",
            RequiredPlugins = new[] { typeof(RecordsPlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double), 120,             Records_List1 + "\r var result = books[0].pages; "),                
                TestCase("result", typeof(double), 130,             Records_List1 + "\r var result = books[1].pages; "),                
                TestCase("result", typeof(double), 140,             Records_List1 + "\r var result = books[2].pages; "),
                TestCase("result", typeof(bool),   true,            Records_List1 + "\r var result = books[0].isactive; "),                
                TestCase("result", typeof(bool),   true,            Records_List1 + "\r var result = books[1].isactive; "),                
                TestCase("result", typeof(bool),   false,           Records_List1 + "\r var result = books[2].isactive; "),
                TestCase("result", typeof(string), "catcher rye",   Records_List1 + "\r var result = books[0].name; ")
            }
        };
    }
}
