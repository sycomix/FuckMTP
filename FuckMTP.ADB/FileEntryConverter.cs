﻿using System.Collections.Generic;
using FuckMTP.Core.Contracts;
using Managed.Adb;

namespace FuckMTP.ADB
{

    internal static class FileEntryConverter
    {
        public static IDirectory Convert(List<FileEntry> entries)
        {
            DirectoryDto root = new DirectoryDto
            {
                Name = "data",
                Path = "/"
            };

            FillRecursively(root, entries);

            return root;
        }

        private static void FillRecursively(DirectoryDto parent, IList<FileEntry> children)
        {
            foreach (FileEntry child in children)
            {
                if (child.IsDirectory)
                {
                    DirectoryDto directory = new DirectoryDto
                    {
                        Name = child.Name,
                        Path = child.FullPath
                    };

                    FillRecursively(directory, child.Children);

                    parent.ListOfDirectories.Add(directory);
                }
                else
                {
                    parent.ListOfFiles.Add(new FileDto
                    {
                        Name = child.Name,
                        Path = child.FullPath
                    });
                }
            }
        }

        private sealed class DirectoryDto : IDirectory
        {
            public readonly IList<IDirectory> ListOfDirectories = new List<IDirectory>();
            public readonly IList<IFile> ListOfFiles = new List<IFile>();

            public string Name { get; set; }

            public string Path { get; set; }

            public IEnumerable<IDirectory> Directories => ListOfDirectories;

            public IEnumerable<IFile> Files => ListOfFiles;
        }

        private sealed class FileDto : IFile
        {
            public string Name { get; set; }

            public string Path { get; set; }
        }
    }
}
