using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Folder
    {
        public Folder(string name)
            : this(name, null)
        {
        }

        public Folder(string name, Folder parentFolder)
        {
            Name = name;    
            ParentFolder = parentFolder;
        }

        public string Name { get; set; }
        public Folder ParentFolder { get; set; }
        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        public List<long> Files { get; set; } = new List<long>();

        public List<Folder> GetAllFolders()
        {
            var folders = new List<Folder>();
            folders.Add(this);
            folders.AddRange(SubFolders.SelectMany(x => x.GetAllFolders()));
            return folders;
        }
        public long GetSize()
        {
            return Files.Sum() + SubFolders.Select(x => x.GetSize()).Sum();
        }
    }
}
