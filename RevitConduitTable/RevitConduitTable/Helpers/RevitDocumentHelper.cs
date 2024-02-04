using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;

using System.Collections.Generic;
using System.Linq;

namespace RevitConduitTable.Helpers
{
    internal static class RevitDocumentHelper
    {
        public static UIApplication UiApplication { get; set; }

        public static IEnumerable<ExternalDefinition> LoadExternalDefinitions()
        {
            Document doc = UiApplication.ActiveUIDocument.Document;
            var externalDefinitions = new List<ExternalDefinition>();


            var sharedParamsFile = doc.Application.OpenSharedParameterFile();
            if (sharedParamsFile != null)
            {
                externalDefinitions = sharedParamsFile.Groups
                    .Cast<DefinitionGroup>()
                    .SelectMany(group => group.Definitions)
                    .OfType<ExternalDefinition>()
                    .ToList();
            }

            return externalDefinitions;
        }

        public static IEnumerable<SharedParameterElement> LoadSharedParametersDefinitions(IEnumerable<ExternalDefinition> externalDefinitions)
        {
            Document doc = UiApplication.ActiveUIDocument.Document;
            var sharedParameterElements = new List<SharedParameterElement>();

            foreach (ExternalDefinition externalDefinition in externalDefinitions)
            {
                try
                {
                    sharedParameterElements.Add(SharedParameterElement.Create(doc, externalDefinition));
                }
                catch (InvalidOperationException)
                {
                    // Parameter already exist
                    sharedParameterElements.Add(SharedParameterElement.Lookup(doc, externalDefinition.GUID));
                }
                
            }

            return sharedParameterElements;
        }
    }

}
