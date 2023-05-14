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

            if (value is SPObject obj) {
                output += $"{indentString}{tab}{ObjectToString(obj, incrementIndent, tabSize)},{lastChar}";
            } else if (value is SPArray arr) {
                output += $"{indentString}{tab}{ArrayToString(arr, incrementIndent, tabSize)},{lastChar}";
            } else {
                output += $"{indentString}{tab}{value ?? "null"},{lastChar}";
            }
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

            if (value is SPObject obj) {
                output += $"{indentString}{tab}{key} = {ObjectToString(obj, incrementIndent, tabSize)};{lastChar}";
            } else if (value is SPArray arr) {
                output += $"{indentString}{tab}{key} = {ArrayToString(arr, incrementIndent, tabSize)};{lastChar}";
            } else {
                output += $"{indentString}{tab}{key} = {value ?? "null"};{lastChar}";
            }
        }

        output += $"{indentString}}}";

        return output;
    }
}
