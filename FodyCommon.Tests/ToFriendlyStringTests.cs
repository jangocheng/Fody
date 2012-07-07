﻿using System;
using NUnit.Framework;

[TestFixture]
public class ToFriendlyStringTests
{
    [Test]
    public void ToFriendlyName()
    {
        var currentDirectory = AssemblyLocation.CurrentDirectory().ToLowerInvariant()
                .Replace(@"bin\debug", string.Empty)
                .Replace(@"bin\release", string.Empty);
        try
        {
            ThrowException1();

        }
        catch (Exception exception)
        {
            var friendlyString = exception.ToFriendlyString().ToLowerInvariant();
            friendlyString = friendlyString
                .Replace(currentDirectory, string.Empty);
            var expected = @"an unhandled excepiton occured:
exception:
foo
stacktrace:
   at tofriendlystringtests.throwexception2() in tofriendlystringtests.cs:line 46
   at tofriendlystringtests.throwexception1() in tofriendlystringtests.cs:line 41
   at tofriendlystringtests.tofriendlyname() in tofriendlystringtests.cs:line 15
source:
fodycommon.tests
targetsite:
void throwexception2()
";
            Assert.AreEqual(expected, friendlyString);
        }
    }

    void ThrowException1()
    {
        ThrowException2();
    }

    void ThrowException2()
    {
        throw new Exception("Foo");
    }
}