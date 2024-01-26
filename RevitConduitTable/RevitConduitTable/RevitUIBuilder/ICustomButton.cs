using Autodesk.Revit.UI;

namespace RevitConduitTable.RevitUIBuilder
{
    internal interface ICustomButton : IExternalCommand
    {
        string Name { get; }
        string ClassName { get; }
        string ButtonText { get; }
        string ImagePath { get; }
        string Tooltip { get; }
        bool IsEnabled { get; }
    }
}
