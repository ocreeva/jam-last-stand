using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Moyba.Editor
{
    public class ScriptTemplateProcessor : AssetModificationProcessor
    {
        private const string _FeatureParameter = "#FEATURE#";
        private const string _NamespaceParameter = "#NAMESPACE#";
        private const string _TraitParameter = "#TRAIT#";

        private const string _ContractsDirectoryName = "Contracts";
        private const string _CoreDirectoryName = "Core";
        private const string _EditorDirectoryName = "Editor";
        private const string _ScriptsDirectoryName = "Scripts";

        private const string _ScriptMetaFileExtension = ".cs.meta";

        public static void OnWillCreateAsset(string assetPath)
        {
            // only process C# script meta files
            if (!assetPath.EndsWith(_ScriptMetaFileExtension)) return;

            // get the path of the C# script, and parse it
            var scriptPath = AssetDatabase.GetAssetPathFromTextMetaFilePath(assetPath);
            var scriptName = Path.GetFileNameWithoutExtension(scriptPath);
            var directorySegments = Path.GetDirectoryName(scriptPath).Split(Path.DirectorySeparatorChar);
            var featureHierarchy = _GetFeatureHierarchy(directorySegments);
            var featureName = featureHierarchy.FirstOrDefault();
            var isEditorScript = directorySegments.Any(s => s.Equals(_EditorDirectoryName));

            // create a lookup for template values
            var templateParameters = new Dictionary<string, string>
            {
                { _FeatureParameter, featureHierarchy.FirstOrDefault() },
                { _NamespaceParameter, _GenerateNamespaceValue(featureHierarchy, isEditorScript) },
                { _TraitParameter, featureName != null ? scriptName.Replace(featureName, String.Empty) : scriptName },
            };

            // read the asset file
            var originalContent = File.ReadAllText(scriptPath);

            // replace the template values
            var updatedContent = originalContent;
            foreach (var parameter in templateParameters)
            {
                updatedContent = updatedContent.Replace(parameter.Key, parameter.Value);
            }

            // update the asset file if its content changed
            if (!originalContent.Equals(updatedContent))
            {
                var lastWriteTime = File.GetLastWriteTimeUtc(scriptPath);
                File.WriteAllText(scriptPath, updatedContent);

                // HACK: reset the last write time, lest AssetDatabase spam the console with warnings
                File.SetLastWriteTimeUtc(scriptPath, lastWriteTime);
            }
        }

        private static string _GenerateNamespaceValue(string[] featureHierarchy, bool isEditorScript)
        {
            // prepend the project's root namespace
            var namespaceSegments = featureHierarchy.Prepend(EditorSettings.projectGenerationRootNamespace);

            // append an ending Editor segment for Editor scripts
            if (isEditorScript) namespaceSegments = namespaceSegments.Append(_EditorDirectoryName);

            // generate the namespace
            return String.Join('.', namespaceSegments);
        }

        private static string[] _GetFeatureHierarchy(IEnumerable<string> directorySegments)
        {
            // strip the root Assets directory
            directorySegments = directorySegments.Skip(1);

            // strip the Scripts directory at the second level of the hierarchy
            directorySegments = directorySegments.Where((s, i) => i != 1 || !s.Equals(_ScriptsDirectoryName));

            // strip the top-level Core directory
            directorySegments = directorySegments.Where((s, i) => i != 0 || !s.Equals(_CoreDirectoryName));

            // strip all Editor directories, anywhere in the hierarchy
            directorySegments = directorySegments.Where(s => !s.Equals(_EditorDirectoryName));

            // strip the Contracts directory at the second level of the remaining hierarchy
            directorySegments = directorySegments.Where((s, i) => i != 1 || !s.Equals(_ContractsDirectoryName));

            return directorySegments.ToArray();
        }
    }
}
