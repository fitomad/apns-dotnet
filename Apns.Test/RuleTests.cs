using Apns.Validation;
using Apns.Test.Mocks;
namespace Apns.Test;

public class RuleTests
{
    [Fact]
    public void TestRuleTrue()
    {
        IRule numberRules = new Rule()
            .Check(() => 1 == 1);
            
        Assert.True(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleFalse()
    {
        IRule numberRules = new Rule()
            .Check(() => 1 == 2);
            
        Assert.False(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleCollectionTrue()
    {
        IRule numberRules = new Rule()
            .Check(() => 1 == 1)
            .Check(() => 2 == 2)
            .Check(() => 3 == 3);
            
        Assert.True(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleCollectionAllFalse()
    {
        IRule numberRules = new Rule()
            .Check(() => 1 == 2)
            .Check(() => 2 == 3)
            .Check(() => 3 == 4);
            
        Assert.False(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleCollectionOnlyOneFalse()
    {
        IRule numberRules = new Rule()
            .Check(() => 1 == 1)
            .Check(() => 2 == 2)
            .Check(() => 3 == 4);
            
        Assert.False(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleWithFilterResultOk()
    {
        IRule numberRules = new Rule()
            .When(() => true)
            .Check(() => 1 == 1);
        
        Assert.True(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleWithFilterResultFailure()
    {
        IRule numberRules = new Rule()
            .When(() => true)
            .Check(() => 1 == 2);
        
        Assert.False(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleWithFilterNotSatisfied()
    {
        IRule numberRules = new Rule()
            .When(() => 1 > 2)
            .Check(() => 1 == 2);
        
        Assert.True(numberRules.Validate());
    }

    [Fact]
    public void TestRuleFailureExecution()
    {
        Assert.Throws<ArithmeticException>(() =>
        {
            new Rule()
                .Check(() => 1 == 2)
                .OnFailure(() => throw new ArithmeticException())
                .Validate();
        });
    }
    
    [Fact]
    public void TestRuleOneFailureExecution()
    {
        Assert.Throws<ArithmeticException>(() =>
        {
            new Rule()
                .Check(() => 1 == 1)
                .Check(() => 2 == 3)
                .OnFailure(() => throw new ArithmeticException())
                .Validate();
        });
    }
    
    [Fact]
    public void TestRuleFailureNotThrowExecution()
    {
        IRule numberRules = new Rule()
            .Check(() => 1 == 1)
            .OnFailure(() => throw new ArithmeticException());
        
        Assert.True(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleNoFailureNotThrowExecution()
    {
        IRule numberRules = new Rule()
            .Check(() => 1 == 1)
            .Check(() => 2 == 2)
            .OnFailure(() => throw new ArithmeticException());
        
        Assert.True(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleNoFailureNotThrowExecutionWithFilterOn()
    {
        IRule numberRules = new Rule()
            .When(() => true)
            .Check(() => 1 == 1)
            .Check(() => 2 == 2)
            .OnFailure(() => throw new ArithmeticException());
        
        Assert.True(numberRules.Validate());
    }
    
    [Fact]
    public void TestRuleNoFailureNotThrowExecutionWithFilterOff()
    {
        Assert.Throws<ArithmeticException>(() =>
        {
            new Rule()
                .When(() => true)
                .Check(() => 1 == 1)
                .Check(() => 2 == 3)
                .OnFailure(() => throw new ArithmeticException())
                .Validate();
        });
    }

    [Fact]
    public void TestPropertiesEqualsString()
    {
        var sampleUser = new MockUser
        {
            Name = "John Doe"
        };

        var userValidator = new Rule()
            .Property(sampleUser.Name).IsEqualsTo("John Doe")
            .Validate();
        
        Assert.True(userValidator);
    }
    
    [Fact]
    public void TestPropertiesEqualsInt()
    {
        var sampleUser = new MockUser
        {
            Age = 49
        };

        var userValidator = new Rule()
            .Property(sampleUser.Age).IsEqualsTo(49)
            .Validate();
        
        Assert.True(userValidator);
    }
    
    [Fact]
    public void TestPropertiesEqualsDouble()
    {
        var sampleUser = new MockUser
        {
            Height = 1.75
        };

        var userValidator = new Rule()
            .Property(sampleUser.Height).IsEqualsTo(1.75)
            .Validate();
        
        Assert.True(userValidator);
    }
    
    [Fact]
    public void TestMultiplePropertiesEqualsOK()
    {
        var sampleUser = new MockUser
        {
            Name = "John Doe",
            Age = 49,
            Height = 1.75
        };

        var userValidator = new Rule()
            .Property(sampleUser.Name).IsEqualsTo("John Doe")
            .Property(sampleUser.Age).IsEqualsTo(49)
            .Property(sampleUser.Height).IsEqualsTo(1.75)
            .Validate();
        
        Assert.True(userValidator);
    }
    
    [Fact]
    public void TestMultiplePropertiesEqualsFailureByOne()
    {
        var sampleUser = new MockUser
        {
            Name = "John Doe",
            Age = 49,
            Height = 1.75
        };

        var userValidator = new Rule()
            .Property(sampleUser.Name).IsEqualsTo("Jane Doe")
            .Property(sampleUser.Age).IsEqualsTo(49)
            .Property(sampleUser.Height).IsEqualsTo(1.75)
            .Validate();
        
        Assert.False(userValidator);
    }
    
    [Fact]
    public void TestMultiplePropertiesEqualsFailureByTwo()
    {
        var sampleUser = new MockUser
        {
            Name = "John Doe",
            Age = 49,
            Height = 1.75
        };

        var userValidator = new Rule()
            .Property(sampleUser.Name).IsEqualsTo("Jane Doe")
            .Property(sampleUser.Age).IsEqualsTo(50)
            .Property(sampleUser.Height).IsEqualsTo(1.75)
            .Validate();
        
        Assert.False(userValidator);
    }
    
    [Fact]
    public void TestPropertiesRangeIntOK()
    {
        var sampleUser = new MockUser
        {
            Age = 49
        };

        var userValidator = new Rule()
            .Property(sampleUser.Age).InRange(lowerBound:40, upperBound:50)
            .Validate();
        
        Assert.True(userValidator);
    }
    
    [Fact]
    public void TestPropertiesRangeIntFailureUpperBound()
    {
        var sampleUser = new MockUser
        {
            Age = 49
        };

        var userValidator = new Rule()
            .Property(sampleUser.Age).InRange(lowerBound:40, upperBound:45)
            .Validate();
        
        Assert.False(userValidator);
    }
    
    [Fact]
    public void TestPropertiesRangeIntFailureLowerBound()
    {
        var sampleUser = new MockUser
        {
            Age = 49
        };

        var userValidator = new Rule()
            .Property(sampleUser.Age).InRange(lowerBound:50, upperBound:55)
            .Validate();
        
        Assert.False(userValidator);
    }
    
    [Fact]
    public void TestPropertiesRangeDoubleOK()
    {
        var sampleUser = new MockUser
        {
            Height = 1.75
        };

        var userValidator = new Rule()
            .Property(sampleUser.Height).InRange(lowerBound:1.70, upperBound:1.80)
            .Validate();
        
        Assert.True(userValidator);
    }
    
    [Fact]
    public void TestPropertiesRangeDoubleFailureUpperBound()
    {
        var sampleUser = new MockUser
        {
            Height = 1.75
        };

        var userValidator = new Rule()
            .Property(sampleUser.Height).InRange(lowerBound: 1.70, upperBound: 1.74)
            .Validate();
        
        Assert.False(userValidator);
    }
    
    [Fact]
    public void TestPropertiesRangeDoubleFailureLowerBound()
    {
        var sampleUser = new MockUser
        {
            Height = 1.75
        };

        var userValidator = new Rule()
            .Property(sampleUser.Height).InRange(lowerBound: 1.80, upperBound: 1.90)
            .Validate();
        
        Assert.False(userValidator);
    }

    [Fact]
    public void TestRuleAndProperties()
    {
        var sampleUser = new MockUser
        {
            Name = "John Doe",
            Age = 49,
            Height = 1.75
        };
        
        var validationResult = new Rule()
            .When(() => 1 == 1)
            .Property(sampleUser.Name).IsEqualsTo("John Doe")
            .Check(() => sampleUser.Age == 49)
            .Property(sampleUser.Height).InRange(lowerBound: 1.70, upperBound: 1.80)
            .OnSuccess(() => Console.WriteLine("Success! 🎉"))
            .Validate();
        
        Assert.True(validationResult);
    }
    
    [Fact]
    public void TestRuleAndPropertiesFailure()
    {
        var sampleUser = new MockUser
        {
            Name = "John Doe",
            Age = 49,
            Height = 1.75
        };

        Assert.Throws<InvalidOperationException>(() =>
        {
            new Rule()
                .When(() => 1 == 1)
                .Property(sampleUser.Name).IsEqualsTo("Not The Real John Doe")
                .Check(() => sampleUser.Age == 49)
                .Property(sampleUser.Height).InRange(lowerBound: 1.70, upperBound: 1.80)
                .OnFailure(() => throw new InvalidOperationException())
                .Validate();
        });
    }

    [Fact]
    public void TestPropertyReferenceNull()
    {
        string sampleTest = null;

        var result = new Rule()
            .Property(sampleTest).IsNull()
            .Validate();
        
        Assert.True(result);
    }
    
    [Fact]
    public void TestPropertyReferenceNotNull()
    {
        var sampleTest = "Hello world!";

        var result = new Rule()
            .Property(sampleTest).IsNotNull()
            .Validate();
        
        Assert.True(result);
    }
    
    [Fact]
    public void TestPropertyNullableTypeNull()
    {
        int? sampleTest = null;
        var result = new Rule()
            .Property(sampleTest).IsNull()
            .Validate();
        
        Assert.True(result);
    }
    
    [Fact]
    public void TestPropertyNullableTypeNotNull()
    {
        int? sampleTest = 50;

        var result = new Rule()
            .Property(sampleTest).IsNotNull()
            .Validate();
        
        Assert.True(result);
    }
    
    [Fact]
    public void TestPropertyNullableTypeRange()
    {
        int? sampleTest = 50;

        var result = new Rule()
            .Property(sampleTest).InRange(lowerBound:25, upperBound:75)
            .Validate();
        
        Assert.True(result);
    }
    
    [Fact]
    public void TestPropertyNullableTypeNullRange()
    {
        int? sampleTest = null;

        var result = new Rule()
            .Property(sampleTest).InRange(lowerBound:25, upperBound:75)
            .Validate();
        
        Assert.False(result);
    }

    [Fact]
    public void TestTheBigOneOK()
    {
        var mockUser = new MockUser
        {
            Name = "John Doe",
            Age = 49,
            Height = 1.75,
            ElectricCarsCounter = null
        };

        var validationResult = new Rule()
            .When(() => "Madrid" == "Madrid")
            .Check(() => "Spain" == "Spain")
            .Property(mockUser.Name).IsEqualsTo("John Doe")
            .Property(mockUser.Age).InRange(lowerBound: 25, upperBound: 75)
            .Property(mockUser.Height).InRange(lowerBound: 1.70, upperBound: 1.90)
            .Property(mockUser.ElectricCarsCounter).IsNull()
            .OnSuccess(() => Console.WriteLine("Success! 🎉"))
            .OnFailure(() => throw new Exception())
            .Validate();
        
        Assert.True(validationResult);
    }
}