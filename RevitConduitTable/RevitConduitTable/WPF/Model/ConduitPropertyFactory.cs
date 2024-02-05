namespace RevitConduitTable.WPF.Model
{
    internal static class ConduitPropertyFactory
    {
        public static ConduitProperty CreateDefault(string parameterName, object parameterValue, bool isReadonly = false, bool isVisible = true)
        {
            return new ConduitProperty
            {
                ParameterName = parameterName,
                ParameterValue = parameterValue,
                IsReadonly = isReadonly,
                IsVisible = isVisible,
                PropeptyRule = new DefaultValuePercentageRule()
            };
        }
    }
}
