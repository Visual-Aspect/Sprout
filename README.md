# Sprout
A subset of JSON and YAML with speed in mind

## Usage

To use Sprout, create a new .SP file (Short for .sprout; you can use either) and add the data you want to add.

Here is an example of a Sprout file:

```js
{
    {
        myString = "abcd";
        myInteger = 1234;
    };

    [
        1,
        2,
        { three = 3 },
        "four"
    ];

    string = "abcdefg";
    int = 123456;
    bool = true;
    double = 1.0;
    null = null;
}
```

The above example is valid Sprout syntax.

## Syntax

In Sprout, there are two main types of data: Objects and Arrays.

### Objects

Objects are declared with curly brackets `{}`, and contain keys with a value.

```js
{
    key = value;
}
```

A key can be a string or an integer, and a value can be a string, integer, double, boolean, array, null, or object.

```js
{
    string = "example";
    int = 1;
    double = 1.0;
    bool = true;
    array = [ 1, 2, 3 ];
    null = null;
    object = {
        ... = ...;
    }
}
```

Keys are seperated from values with an equals sign `=`, and each definition is seperated with a semicolon `;`.

### Arrays

Arrays are declared with square brackets `[]`, and contain values.

```js
[
    value
]
```

Arrays consist of the same value rules:

```js
[
    "example",
    1,
    1.0,
    true,
    [ 1, 2, 3 ],
    null,
    {
        ... = ...;
    }
]
```

Values are seperated with a comma `,`.
