using System.IO;

namespace Piksel.Graphics.ColourPicker.Utilities
{
    public static class Resources
    {
        /// <summary>
        /// Retrieves a file resource stream from assembly.
        /// </summary>
        /// <param name="resourceFile">The name of the resource file that is 
        /// to be retrieved.</param>
        /// <returns>A stream containing the contents of the file.</returns>

        public static Stream GetFileResource<T>(string resourceFile, T assemblyObject = null) where T : class
            => (resourceFile != null & resourceFile.Length > 0)
                ? GetResourceStream<T>(resourceFile)
                : null;

        /// <summary>
        /// Returns resource stream.
        /// </summary>
        /// <param name="resourceFile">A valid string representing the file 
        /// name of the resource.</param>
        /// <returns>A stream representing the resource.</returns>

        private static Stream GetResourceStream<T>(string resourceFile, T obj = null) where T : class
        {
            var type = typeof(T);
            var resPath = CreateFullResourcePath(type.Namespace, resourceFile);
            return type.Assembly.GetManifestResourceStream(resPath);
        }

        /// <summary>
        /// Concatenates resource namespace and file name. Checks to make sure 
        /// that the namespace is not empty before appending the resource file
        /// name.
        /// </summary>
        /// <param name="resourceNamespace">A valid string representing the 
        /// namespace in which the resource resides.</param>
        /// <param name="resourceFile">A valid string representing the file 
        /// name of the resource.</param>
        /// <returns>A concatenated resource path.</returns>
        private static string CreateFullResourcePath(string resourceNamespace, string resourceFile)
            => (!string.IsNullOrEmpty(resourceNamespace))
                ? $"{resourceNamespace}.{resourceFile}"
                : resourceFile;


    }
}
