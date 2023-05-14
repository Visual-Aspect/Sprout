using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sprout;

public static partial class SP {
    public static string ArrayToString(SPArray spArray, int indent = -1, int tabSize = 4) {
        string indentString = "";
        if (indent > 0) indentString = new string(' ', indent * tabSize);


        int incrementIndent = indent == -1 ? -1 : indent + 1;
        char lastChar = indent == -1 ? ' ' : '\n';
        string tab = indent == -1 ? "" : new string(' ', tabSize);
        string output = "[" + lastChar;

        foreach (KeyValuePair<int, dynamic> variable in spArray.Variables) {
            int key = variable.Key;
            dynamic value = variable.Value;
            bool isLastElement = key == spArray.Variables.Count - 1;
            output += $"{indentString}{tab}{SerializeToWritable(value, incrementIndent, tabSize)}{(isLastElement ? "" : ",")}{lastChar}";
        }

        output += $"{indentString}]";

        return output;
    }

    public static string ObjectToString(SPObject spObject, int indent = -1, int tabSize = 4) {
        string indentString = "";
        if (indent > 0) indentString = new string(' ', indent * tabSize);


        int incrementIndent = indent == -1 ? -1 : indent + 1;
        char lastChar = indent == -1 ? ' ' : '\n';
        string tab = indent == -1 ? "" : new string(' ', tabSize);
        string output = "{" + lastChar;

        foreach (KeyValuePair<string, dynamic> variable in spObject.Variables) {
            string key = variable.Key;
            dynamic value = variable.Value;
            output += $"{indentString}{tab}{key} = {SerializeToWritable(value, incrementIndent, tabSize)};{lastChar}";
        }

        output += $"{indentString}}}";

        return output;
    }

    public static string SerializeToWritable(dynamic value, int indent = -1, int tabSize = 4) {
        if (value is SPObject obj) return ObjectToString(obj, indent, tabSize);
        if (value is SPArray arr) return ArrayToString(arr, indent, tabSize);
        if (value is null) return "null";
        if (value is string str) return $"\"{str}\"";
        
        return value.ToString();
    }
}
