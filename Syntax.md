# General planning for syntax

Base object:

```
string: "example"
int: 1
double: 1.0
bool: true
array: [ 1, 2, 3 ]
object:
    ...: ...
```

Base array:
```
[
    :
        string: "abcd"
        int: 1

    string: "abcdefg"
    int: 123

    :
        string: "abc"
        int: 123
]
```

There is a single base object or array:
Objects start with the keys and values,
whereas an array will be declared with square brackets at the start of the file.

To declare an object in an array, use a colon:
```
[
    :
        // Object content
]
```

To declare an object in an object, use a key followed by a colon:
```
object:
    key:
        // Object content
```

## Valid type list

```
string: "example"
int: 1
double: 1.0
bool: true
array: [ 1, 2, 3 ]
object:
    ...: ...
```

More types may be added later.
