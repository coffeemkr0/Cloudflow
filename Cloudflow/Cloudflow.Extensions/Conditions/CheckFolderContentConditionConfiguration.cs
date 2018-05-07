using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.Collections.Generic;

namespace Cloudflow.Extensions.Conditions
{
    public class CheckFolderContentConditionConfiguration
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
