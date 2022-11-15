// See https://aka.ms/new-console-template for more information
using System.Diagnostics.CodeAnalysis;
using System.Text;

Console.WriteLine("Hello, World!");

RawStringLiterals();
NewLinesInStringInterpolation();
AnUnsignedRightShiftOperator();
ListPatterns();

void RawStringLiterals()
{
    const string name = "Milan";
    const string lastName = "Martiniak";

    string json =
    $$"""
    {
        "Name": "{{name}}",
        "LastName": "{{lastName}}"
    }
    """;

    Console.WriteLine(json);
}

void NewLinesInStringInterpolation()
{
    const DayOfWeek day = DayOfWeek.Monday;

    string dayInfo = $"Today is {day switch
    {
        DayOfWeek.Saturday or DayOfWeek.Sunday => "weekend",
        _ => "working day"
    }}.";

    Console.WriteLine(dayInfo);
}

void Utf8StringLiterals()
{
    // C# 10
    //byte[] array = Encoding.UTF8.GetBytes("Hello UTF-8 String Literals");

    // C# 11
    ReadOnlySpan<byte> span = "Hello UTF-8 String Literals"u8;
    byte[] array = "Hello UTF-8 String Literals"u8.ToArray();
}

void PatternMatchCharSpan()
{
    ReadOnlySpan<char> str = "World".AsSpan();

    if (str is "World")
    {
        Console.WriteLine("Hello world");
    }
}

void AnUnsignedRightShiftOperator()
{
    int n = -32;
    Console.WriteLine($"Before shift: bin = {Convert.ToString(n, 2),32}, dec = {n}");

    int a = n >> 2;
    Console.WriteLine($"After     >>: bin = {Convert.ToString(a, 2),32}, dec = {a}");

    int b = n >>> 2;
    Console.WriteLine($"After    >>>: bin = {Convert.ToString(b, 2),32}, dec = {b}");

    // Output:
    // Before shift: bin = 11111111111111111111111111100000, dec = -32
    // After     >>: bin = 11111111111111111111111111111000, dec = -8
    // After    >>>: bin =   111111111111111111111111111000, dec = 1073741816
}

void ListPatterns()
{
    var numbers = new[] { 1, 2, 3, 4 };

    // List and constant patterns
    Console.WriteLine(numbers is [1, 2, 3, 4]); // True
    Console.WriteLine(numbers is [1, 2, 4]);    // False

    // List and discard patterns
    Console.WriteLine(numbers is [_, 2, _, 4]); // True
    Console.WriteLine(numbers is [.., 3, _]);   // True

    // List and logical patterns
    Console.WriteLine(numbers is [_, >= 2, _, _]); // True
}

#region Static Abstract Members in Interfaces

void NumberOfLegs<T>(T animal) where T : IAnimal
{
    Console.WriteLine(T.NumberOfLegs);
}

NumberOfLegs(new Dog());
NumberOfLegs(new Snake());

public record Dog : IAnimal
{
    public static int NumberOfLegs => 4;
}

public record Snake : IAnimal
{
    public static int NumberOfLegs => 0;
}

public interface IAnimal
{
    static abstract int NumberOfLegs { get; }
}

#endregion

#region Extended nameof Scope

public class MyAttribute : Attribute
{
    private readonly string _paramName;
    public MyAttribute(string paramName)
    {
        _paramName = paramName;
    }
}
public class MyClass
{
    [My(nameof(param))]
    public void Method(int param, [My(nameof(param))] int anotherParam)
    { }
}

#endregion

#region Required members

public class Person
{
    public Person() { }

    [SetsRequiredMembers]
    public Person(string name, string lastName)
    {
        Name = name;
        LastName = lastName;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string LastName { get; set; }
}

// Initializations with required properties - valid
// var p1 = new Person { Name = "Milan", LastName = "Martiniak" };
// Person p2 = new("Milan", "Martiniak");

// Initializations with missing required properties - compilation error
// var p3 = new Person { Name = "Milan" };
// Person p4 = new();

#endregion

#region Auto-default Structs

struct PersonStruct
{
    public PersonStruct(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public int Age { get; set; }
}

#endregion

#region Generic attributes

// Before C# 11:
public class TypeAttribute : Attribute
{
    public TypeAttribute(Type type) => ParamType = type;

    public Type ParamType { get; }
}

public class GenericAttribute<T> : Attribute
{
}

// After C#
[Generic<int>]
public class MyType
{
    [Generic<int>()]
    public void Method() {}
}

#endregion