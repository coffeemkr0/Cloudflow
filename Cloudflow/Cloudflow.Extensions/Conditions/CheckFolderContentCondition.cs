using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace Cloudflow.Extensions.Conditions
{
    [ExportConfigurableExtension("45C9872C-70DC-41E4-B769-3C27447F9E84", typeof(CheckFolderContentCondition), "2822B8DB-56BF-42C2-869D-C4C658CF8A34",
    "CheckFolderContentName", "CheckFolderContentDescription", "Folder")]
    public class CheckFolderContentCondition : Condition
    {
        private CheckFolderContentConditionConfiguration CheckFolderContentConditionConfiguration
        {
            get { return (CheckFolderContentConditionConfiguration)ConditionConfiguration; }
        }

        [ImportingConstructor]
        public CheckFolderContentCondition([Import("ExtensionConfiguration")]ExtensionConfiguration conditionConfiguration) : base(conditionConfiguration)
        {
            
        }

        public override bool CheckCondition()
        {
            foreach (var fileNameMask in CheckFolderContentConditionConfiguration.FileNameMasks)
            {
                if (Directory.GetFiles(CheckFolderContentConditionConfiguration.Folder, fileNameMask, SearchOption.TopDirectoryOnly).Count() > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
