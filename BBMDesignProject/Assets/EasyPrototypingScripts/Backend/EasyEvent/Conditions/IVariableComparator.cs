using Backend.CustomVariableFeature;
using UnityEngine;

namespace Backend.EasyEvent.Conditions
{
    public static class VariableComparatorFactory
    {
        public static IVariableComparator GetComparator(VariableType type)
        {
            return type switch
            {
                VariableType.Integer => new IntComparator(),
                VariableType.Float => new FloatComparator(),
                VariableType.Boolean => new BoolComparator(),
                VariableType.String => new StringComparator(),
                _ => null
            };
        }
    }
    
    public interface IVariableComparator
    {
        bool Compare(string targetValue, object actualValue, CheckValueCondition.CheckType checkType);
    }

    public class IntComparator : IVariableComparator
    {
        public bool Compare(string targetValue, object actualValue, CheckValueCondition.CheckType checkType)
        {
            if (!(actualValue is int actual)) return false;
            if (!int.TryParse(targetValue, out var target)) return false;

            return checkType switch
            {
                CheckValueCondition.CheckType.Equals => actual == target,
                CheckValueCondition.CheckType.GreaterThan => actual > target,
                CheckValueCondition.CheckType.GreaterEqualThan => actual >= target,
                CheckValueCondition.CheckType.LessThan => actual < target,
                CheckValueCondition.CheckType.LessEqualThan => actual <= target,
                _ => false
            };
        }
    }

    public class FloatComparator : IVariableComparator
    {
        public bool Compare(string targetValue, object actualValue, CheckValueCondition.CheckType checkType)
        {
            if (!(actualValue is float actual)) return false;
            if (!float.TryParse(targetValue, out var target)) return false;

            return checkType switch
            {
                CheckValueCondition.CheckType.Equals => Mathf.Approximately(actual, target),
                CheckValueCondition.CheckType.GreaterThan => actual > target,
                CheckValueCondition.CheckType.GreaterEqualThan => actual >= target,
                CheckValueCondition.CheckType.LessThan => actual < target,
                CheckValueCondition.CheckType.LessEqualThan => actual <= target,
                _ => false
            };
        }
    }

    public class BoolComparator : IVariableComparator
    {
        public bool Compare(string targetValue, object actualValue, CheckValueCondition.CheckType checkType)
        {
            return actualValue is bool actual &&
                   bool.TryParse(targetValue, out var target) &&
                   checkType == CheckValueCondition.CheckType.Equals &&
                   actual == target;
        }
    }

    public class StringComparator : IVariableComparator
    {
        public bool Compare(string targetValue, object actualValue, CheckValueCondition.CheckType checkType)
        {
            return actualValue is string actual &&
                   checkType == CheckValueCondition.CheckType.Equals &&
                   actual == targetValue;
        }
    }
}