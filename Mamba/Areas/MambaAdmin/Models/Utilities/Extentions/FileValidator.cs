using Mamba.Areas.MambaAdmin.Models.Utilities.Enums;

namespace Mamba.Areas.MambaAdmin.Models.Utilities.Extentions
{
    public static class FileValidator
    {
        public static bool IsValidType(this IFormFile file,FileType type)
        {
            if (file is not null)
            {
                if (type == FileType.Image)
                {
                    if (file.ContentType.Contains("image/")) return true;
                    return false;
                }
                if (type == FileType.Video)
                {
                    if (file.ContentType.Contains("video/")) return true;
                    return false;
                }
                if (type == FileType.Audio)
                {
                    if (file.ContentType.Contains("audio/")) return true;
                    return false;
                }
                return false;
            }
            else return false;
        }

        public static bool IsValidSize(this IFormFile file,int maxSize,FileSize size=FileSize.Kilobyte)
        {
            if(size == FileSize.Kilobyte)
            {
                if(file.Length <= maxSize * 1024) return true;
                return false;
            }
            if(size==FileSize.Megabyte)
            {
                if(file.Length <= maxSize * 1024 * 1024) return true;
                return false;
            }
            if (size == FileSize.Gigabyte)
            {
                if (file.Length <= maxSize * 1024 * 1024 * 1024) return true;
                return false;
            }
            else return false;

        }

        public async static Task<string> CreateAsync(this IFormFile file,string rootpath, params string[] folders)
        {
            string fileName = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
            string path = rootpath;
            for(int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);

            using(FileStream newFile = new(path, FileMode.Create))
            {
                await file.CopyToAsync(newFile);
            }
            return fileName;

        }

        public static void DeleteAsync(this string fileName,string rootpath,params string[] folders)
        {
            string path = rootpath;
            for(int i = 0;i < folders.Length; i++)
            {
                path=Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path,fileName);

            if(File.Exists(path))
            {
                File.Delete(path);
            }

        }


    }
}
