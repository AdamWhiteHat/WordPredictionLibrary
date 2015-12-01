using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordPredictionLibrary
{
	public static class FileInfoExtensionMethods
	{
		public static FileInfo RenameIfExists(this FileInfo source)
		{
			if (source != null)
			{
				int counter = 1;
				string newFilename = source.FullName;

				while (File.Exists(newFilename))
				{
					newFilename = Path.Combine(source.DirectoryName,string.Format("{0} ({1}){2}", Path.GetFileNameWithoutExtension(source.Name), counter++, Path.GetExtension(source.Name)));
				}
				return new FileInfo(newFilename);
			}
			return default(FileInfo);
		}
	}
}
