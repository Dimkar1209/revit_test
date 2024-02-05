using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitConduitTable.Helpers
{
    internal static class RevitDocumentHelper
    {
        public static UIApplication UiApplication { get; set; }

        /// <summary>
        /// Load shared parameters definitions
        /// </summary>
        /// <returns>Collection of <see cref="ExternalDefinition"/></returns>
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

        /// <summary>
        /// Get assingned shared parameters in doc
        /// </summary>
        /// <param name="externalDefinitions">Shared parameter definitions</param>
        /// <returns>Collection of <see cref="SharedParameterElement"/></returns>
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
                catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                {
                    // Parameter already exist
                    sharedParameterElements.Add(SharedParameterElement.Lookup(doc, externalDefinition.GUID));
                }
                
            }

            return sharedParameterElements;
        }

        public static bool TryChangeParameterValue(Element element, Guid paramGuid, string newValue)
        {
            Parameter parameter = element.get_Parameter(paramGuid);

            if (parameter != null && !parameter.IsReadOnly)
            {

                using (Transaction trans = new Transaction(element.Document))
                {
                    trans.Start();
                    bool setResult = parameter.Set(newValue);
                    trans.Commit();

                    return setResult;
                }
            }

            return false;
        }

        public static IDictionary<string, Guid> GetSharedParameterNames()
        {
            IEnumerable<ExternalDefinition> externalDefinitions = LoadExternalDefinitions();
            var parameters = new Dictionary<string, Guid>();

            foreach (ExternalDefinition definition in externalDefinitions)
            {
                parameters.Add(definition.Name, definition.GUID);
            }

            return parameters;
        }

    }

}
