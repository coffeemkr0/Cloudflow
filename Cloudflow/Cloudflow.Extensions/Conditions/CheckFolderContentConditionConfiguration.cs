using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.Collections.Generic;

namespace Cloudflow.Extensions.Conditions
{
    [ExportExtension("2822B8DB-56BF-42C2-869D-C4C658CF8A34", typeof(CheckFolderContentConditionConfiguration))]
    public class CheckFolderContentConditionConfiguration : ExtensionConfiguration
    {
        #region Properties
        [DisplayOrder(1)]
        [LabelTextResourceAttribute("FolderLabel")]
        public string Folder { get; set; }

        [LabelTextResourceAttribute("FileNameMasksLabel")]
        public List<string> FileNameMasks { get; set; }
        #endregion

        #region Constructors
        public CheckFolderContentConditionConfiguration()
        {
            FileNameMasks = new List<string>();
        }
        #endregion
    }
}
