using System;

namespace QicRecVisualizer.WpfCore.Commands
{
    internal static class CommandHelpers
    {
        public static T CheckCastParameter<T>(this object parameter)
        {
            if (parameter is T casted)
            {
                return casted;
            }
            throw new InvalidCastException($"the parameter provided is not of the expected type: {parameter} - Current type: {parameter.GetType().Name} - Expected : {typeof(T).Name}");
        }
    }
}
