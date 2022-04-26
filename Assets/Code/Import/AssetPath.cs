using System;
using System.Linq;

namespace Code.Import
{
    public static class AssetPath
    {
        public class Folder
        {
            public string Name { get; }
            public string Path { get; }

            public Folder(string path)
            {
                Path = path;
                Name = Path.Split(new[] { Separator}, StringSplitOptions.RemoveEmptyEntries).Last();
            }
        }

        public static string Root => "Assets";
        public static string Textures { get; } = Combine(Root, "Textures");
        public static string Prefabs { get; } = Combine(Root, "Prefabs");
        public static string Data { get; } = Combine(Root, "Data");

        private static string Separator => "/"; 

        public static string Combine(params string[] paths)
        {
            return string.Join(Separator, paths);
        }
    }
}