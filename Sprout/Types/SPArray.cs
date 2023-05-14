using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sprout;

public class SPArray {
    public Dictionary<int, dynamic> Variables = new Dictionary<int, dynamic>();

    public dynamic this[int key] {
        get {
            if (Variables.ContainsKey(key)) return Variables[key];
            else return null;
        } set {
            Variables[key] = value;
        }
    }

    public override string ToString() {
        return ToString(0);
    }

    public string ToString(int indent) {
        string indentString = new string(' ', indent * 4);
        string output = "[\n";

        foreach (KeyValuePair<int, dynamic> variable in Variables) {
            int key = variable.Key;
            dynamic value = variable.Value;

            if (value is SPObject) {
                output += $"{indentString}    {((SPObject)value).ToString(indent + 1)},\n";
            } else if (value is SPArray) {
                output += $"{indentString}    {((SPArray)value).ToString(indent + 1)},\n";
            } else {
                output += $"{indentString}    {value},\n";
            }
        }

        output += $"{indentString}]";

        return output;
    }
}
