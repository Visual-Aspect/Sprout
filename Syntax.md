# General planning for syntax

Base object:

<!-- TODO: Replace JSON markdown styling -->

```json
string = "example";
int = 1;
double = 1.0;
bool = true;
array = [ 1; 2; 3 ];
object = {
    ... = ...;
}
```

Base array:
```json
[
    {
        string = "abcd";
        int = 1;
    }

    {
        string = "abc";
        int = 123;
    }

    string = "abcdefg";
    int = 123456;
]
```

There is a single base object or array:
Objects start with the keys and values,
whereas an array will be declared with square brackets at the start of the file.

```json
[
    {
        // Object content
    }
]
```

## Valid type list

```json
string = "example";
int = 1;
double = 1.0;
bool = true;
array = [ 1; 2; 3 ];
object = {
    ... = ...;
}
```

More types may be added later.
