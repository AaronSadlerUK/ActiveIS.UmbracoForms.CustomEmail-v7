using Umbraco.Core.IO;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace ActiveIS.UmbracoForms.CustomEmailV7.Trees
{
    [Tree("forms", "activeisCustomEmailTemplates", "Custom Email Templates", "icon-folder", "icon-folder-open", false, 0)]
    [PluginController("ActiveISCustomEmail")]
    public class ActiveISCustomEmailTemplateTreeController : FileSystemTreeController
    {
        private static readonly string[] ExtensionsStatic = new string[1]
        {
            "html"
        };

        protected override IFileSystem2 FileSystem => (IFileSystem2)new PhysicalFileSystem("~/Views/Partials/Forms/CustomEmails");

        protected override string[] Extensions => ExtensionsStatic;

        protected override string FileIcon => "icon-article";
    }
}
