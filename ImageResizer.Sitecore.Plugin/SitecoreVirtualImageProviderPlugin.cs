using ImageResizer.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer.Sitecore.Plugin
{
    public class SitecoreVirtualImageProviderPlugin : IPlugin, IVirtualImageProvider
    {
        public IPlugin Install(global::ImageResizer.Configuration.Config c)
        {
            c.Plugins.add_plugin(this);
            return this;
        }

        public bool Uninstall(global::ImageResizer.Configuration.Config c)
        {
            c.Plugins.remove_plugin(this);
            return true;
        }

        private string FixVirtualPath(string virtualPath)
        {
            var subIndex = virtualPath.LastIndexOf("~");
            if (subIndex < 0)
            {
                subIndex = virtualPath.LastIndexOf("-");
            }

            if (subIndex > 0)
            {
                return virtualPath.Substring(subIndex);
            }
            else
            {
                return virtualPath;
            }
        }

        public bool FileExists(string virtualPath, NameValueCollection queryString)
        {
            if (virtualPath.StartsWith("/sitecore/shell/-"))
            {
                return false;
            }
            
            virtualPath = FixVirtualPath(virtualPath);
            DynamicLink dynamicLink;

            return queryString.Count > 0 && DynamicLink.TryParse(virtualPath, out dynamicLink);
        }

        public IVirtualFile GetFile(string virtualPath, NameValueCollection queryString)
        {
            virtualPath = FixVirtualPath(virtualPath);

            return new SitecoreVirtualFile(virtualPath);
        }
    }
}
