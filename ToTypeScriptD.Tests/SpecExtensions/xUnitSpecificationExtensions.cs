using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;


public static class xUnitSpecificationExtensions
{
    public static void ShouldBeFalse(this bool condition)
    {
        ShouldBeFalse(condition, string.Empty);
    }
    public static void ShouldBeFalse(this bool condition, string message)
    {
        Assert.False(condition, message);
    }




    public static void ShouldBeTrue(this bool condition)
    {
        ShouldBeTrue(condition, string.Empty);
    }
    public static void ShouldBeTrue(this bool condition, string message)
    {
        Assert.True(condition, message);
    }



    public static T ShouldEqual<T>(this T actual, T expected)
    {
        Assert.Equal(expected, actual);
        return actual;
    }

    public static T ShouldEqual<T>(this T actual, T expected, IEqualityComparer<T> comparer)
    {
        Assert.Equal(expected, actual, comparer);
        return actual;
    }

    public static string ShouldEqual(this string actual, string expected, StringComparer comparer)
    {
        Assert.Equal(expected, actual, comparer);
        return actual;
    }



    public static T ShouldNotEqual<T>(this T actual, T expected)
    {
        Assert.NotEqual(expected, actual);
        return actual;
    }

    public static string ShouldNotEqual(this string actual, string expected, StringComparer comparer)
    {
        Assert.NotEqual(expected, actual, comparer);
        return actual;
    }

    public static T ShouldNotEqual<T>(this T actual, T expected, IEqualityComparer<T> comparer)
    {
        Assert.NotEqual(expected, actual, comparer);
        return actual;
    }



    public static void ShouldBeNull(this object anObject)
    {
        Assert.Null(anObject);
    }



    public static T ShouldNotBeNull<T>(this T anObject)
    {
        Assert.NotNull(anObject);
        return anObject;
    }


    public static object ShouldBeTheSameAs(this object actual, object expected)
    {
        Assert.Same(expected, actual);
        return actual;
    }



    public static object ShouldNotBeTheSameAs(this object actual, object expected)
    {
        Assert.NotSame(expected, actual);
        return actual;
    }



    public static T ShouldBeOfType<T>(this T actual, Type expected)
    {
        Assert.IsType(expected, actual);
        return actual;
    }


    public static T ShouldNotBeOfType<T>(this T actual, Type expected)
    {
        Assert.IsNotType(expected, actual);
        return actual;
    }


    public static IEnumerable ShouldBeEmpty(this IEnumerable collection)
    {
        Assert.Empty(collection);
        return collection;
    }



    public static IEnumerable ShouldNotBeEmpty(this IEnumerable collection)
    {
        Assert.NotEmpty(collection);
        return collection;
    }



    public static string ShouldContain(this string actualString, string expectedSubString)
    {
        Assert.Contains(expectedSubString, actualString);
        return actualString;
    }

    public static string ShouldContain(this string actualString, string expectedSubString, StringComparison comparisonType)
    {
        Assert.Contains(expectedSubString, actualString, comparisonType);
        return actualString;
    }

    public static IEnumerable<string> ShouldContain(this IEnumerable<string> collection, string expected, StringComparer comparer)
    {
        Assert.Contains(expected, collection, comparer);
        return collection;
    }

    public static IEnumerable<T> ShouldContain<T>(this IEnumerable<T> collection, T expected)
    {
        Assert.Contains(expected, collection);
        return collection;
    }

    public static IEnumerable<T> ShouldContain<T>(this IEnumerable<T> collection, T expected, IEqualityComparer<T> equalityComparer)
    {
        Assert.Contains(expected, collection, equalityComparer);
        return collection;
    }




    public static string ShouldNotContain(this string actualString, string expectedSubString)
    {
        Assert.DoesNotContain(expectedSubString, actualString);
        return actualString;
    }

    public static string ShouldNotContain(this string actualString, string expectedSubString, StringComparison comparisonType)
    {
        Assert.DoesNotContain(expectedSubString, actualString, comparisonType);
        return actualString;
    }

    public static IEnumerable<string> ShouldNotContain(this IEnumerable<string> collection, string expected, StringComparer comparer)
    {
        Assert.DoesNotContain(expected, collection, comparer);
        return collection;
    }

    public static IEnumerable<T> ShouldNotContain<T>(this IEnumerable<T> collection, T expected)
    {
        Assert.DoesNotContain(expected, collection);
        return collection;
    }

    public static IEnumerable<T> ShouldNotContain<T>(this IEnumerable<T> collection, T expected, IEqualityComparer<T> equalityComparer)
    {
        Assert.DoesNotContain(expected, collection, equalityComparer);
        return collection;
    }



    public static Exception ShouldBeThrownBy(this Type exceptionType, Assert.ThrowsDelegate method)
    {
        return Assert.Throws(exceptionType, method);
    }

    public static void ShouldNotThrow(this Assert.ThrowsDelegate method)
    {
        Assert.DoesNotThrow(method);
    }



    public static T IsInRange<T>(this T actual, T low, T high) where T : IComparable
    {
        Assert.InRange(actual, low, high);
        return actual;
    }

    public static T IsInRange<T>(this T actual, T low, T high, IComparer<T> comparer)
    {
        Assert.InRange(actual, low, high, comparer);
        return actual;
    }



    public static T IsNotInRange<T>(this T actual, T low, T high) where T : IComparable
    {
        Assert.NotInRange(actual, low, high);
        return actual;
    }

    public static T IsNotInRange<T>(this T actual, T low, T high, IComparer<T> comparer)
    {
        Assert.NotInRange(actual, low, high, comparer);
        return actual;
    }

    public static IEnumerable IsEmpty(this IEnumerable collection)
    {
        Assert.Empty(collection);
        return collection;
    }

    public static T IsType<T>(this T actual, Type expectedType)
    {
        Assert.IsType(expectedType, actual);
        return actual;
    }

    public static T IsNotType<T>(this T actual, Type expectedType)
    {
        Assert.IsNotType(expectedType, actual);
        return actual;
    }

    public static T IsAssignableFrom<T>(this T actual, Type expectedType)
    {
        Assert.IsAssignableFrom(expectedType, actual);
        return actual;
    }


/* Consider implementing these later 
 
    public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
    {
        StringAssert.AreEqualIgnoringCase(expected, actual);
        return actual;
    }

    public static string ShouldStartWith(this string actual, string expected)
    {
        StringAssert.StartsWith(expected, actual);
        return actual;
    }

    public static string ShouldEndWith(this string actual, string expected)
    {
        StringAssert.EndsWith(expected, actual);
        return actual;
    }

    public static void ShouldBeSurroundedWith(this string actual, string expectedStartDelimiter, string expectedEndDelimiter)
    {
        StringAssert.StartsWith(expectedStartDelimiter, actual);
        StringAssert.EndsWith(expectedEndDelimiter, actual);
    }

    public static void ShouldBeSurroundedWith(this string actual, string expectedDelimiter)
    {
        StringAssert.StartsWith(expectedDelimiter, actual);
        StringAssert.EndsWith(expectedDelimiter, actual);
    }

    public static void ShouldContainErrorMessage(this Exception exception, string expected)
    {
        StringAssert.Contains(expected, exception.Message);
    }
*/
}
