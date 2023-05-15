using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SproutNS;

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

    public override string ToString() => Sprout.ArrayToString(this);
    public string ToString(int indent, int tabSize) => Sprout.ArrayToString(this, indent, tabSize);
}
