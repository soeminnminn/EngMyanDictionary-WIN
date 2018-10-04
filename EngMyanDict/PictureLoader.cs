using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using ICSharpCode.SharpZipLib.Zip;

namespace EngMyanDict
{
    internal class PictureLoader : IDisposable
    {
        #region Variables
        private string mPicZipPath = string.Empty;
        private Dictionary<long, Image> mImagesCache = null;
        #endregion

        #region Constructor
        public PictureLoader(string appPath)
        {
            this.mPicZipPath = Path.Combine(appPath, "Data\\pics.zip");
            this.mImagesCache = new Dictionary<long, Image>();
        }
        #endregion

        #region Methods
        public Image LoadPicture(DictionaryItem item)
        {
            if (this.mImagesCache.ContainsKey(item.Id))
            {
                return this.mImagesCache[item.Id];
            }

            Image image = null;
            if (File.Exists(this.mPicZipPath))
            {
                using (ZipFile zipFile = new ZipFile(this.mPicZipPath))
                {
                    ZipEntry entry = zipFile.GetEntry(item.FileName + ".png");
                    if (entry != null)
                    {
                        using (Stream stream = zipFile.GetInputStream(entry))
                        {
                            try
                            {
                                image = Bitmap.FromStream(stream);
                                this.mImagesCache.Add(item.Id, image);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                            }

                        }
                    }
                }
            }
            return image;
        }

        public void Dispose()
        {
            foreach (Image image in this.mImagesCache.Values)
            {
                image.Dispose();
            }
        }
        #endregion

        #region Properties
        #endregion
    }
}
