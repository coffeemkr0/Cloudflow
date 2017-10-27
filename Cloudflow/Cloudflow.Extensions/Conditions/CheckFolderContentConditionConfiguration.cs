using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.FileNameMasks = new List<string>();
        }
        #endregion
    }
}
